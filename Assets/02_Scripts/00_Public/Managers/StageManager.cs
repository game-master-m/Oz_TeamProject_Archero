using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : MonoBehaviour
{
    [Header("이벤트 발송")]
    [SerializeField] private VoidEventChannelSO mOnRoomClear;                   //몬스터경험치 프리팹이 구독
    [SerializeField] private PlayerAttackEventChannelSO mOnLevelUpPlayer;       //레벨업 UI가 구독
    [SerializeField] private IntTripleEventChannelSO mOnStageClear;             //EndUI(SuccessUI를 따로 만들지 고민)가 구독, KillCount기반 로비플레이어 레벨업 및 골드획득
    [SerializeField] private IntTripleEventChannelSO mOnShowEndUIRequest;       //EndUI가 구독, KillCount기반 로비플레이어 레벨업 및 골드획득
    [SerializeField] private VoidEventChannelSO mOnNoticeLastRoom;              //LevelUpController.cs가 구독

    [Header("이벤트 구독")]
    [SerializeField] private VoidEventChannelSO mOnPlayerDie;   //PlayerStat.cs 가 발행

    // 런타임 참조 변수 (Initializer에게서 받음)
    private StageDataSO mStageData;
    private PlayerController mPlayer;
    private List<Transform> mSpawnPoints;
    private GameObject mDoorObject;

    // 상태 변수
    private int mCurrentRoomIndex = 0;
    private int mAliveEnemyCount = 0;
    private bool bIsBattleActive = false;

    private StageMap mCurrentMapInstance;

    // 천사슬라임 여부
    private bool bIsAngelSlimeTurn = false;
    private bool bIsAngelSlimeEnded = false;

    // 코루틴 참조 변수
    private WaitForSeconds mWaitOneSec;
    private WaitForSeconds mWaitTwoSec;
    private float mOneSec = 1.0f;
    private float mTwoSec = 2.0f;
    private Coroutine mWaitNextRoomCo;

    private int mKillCount;

    private void Awake()
    {
        mWaitOneSec = new WaitForSeconds(mOneSec);
        mWaitTwoSec = new WaitForSeconds(mTwoSec);
    }

    private void OnEnable()
    {
        mOnPlayerDie.onEvent += HandlePlayerDie;
    }
    private void OnDisable()
    {
        mOnPlayerDie.onEvent -= HandlePlayerDie;
    }

    public void LevelUp()
    {
        mOnLevelUpPlayer.Raised(mPlayer.Attack);
    }

    // 1. 스테이지 초기화 (Initializer가 호출)
    public void SetupStage(StageDataSO data, PlayerController player, List<Transform> points, GameObject door)
    {

        mStageData = data;
        mPlayer = player;
        mSpawnPoints = points;
        mDoorObject = door;

        // 변수 리셋
        if (mWaitNextRoomCo != null) StopCoroutine(mWaitNextRoomCo);
        mCurrentRoomIndex = 0;
        mAliveEnemyCount = 0;
        mKillCount = 0;
        bIsBattleActive = false;

        //시작 시, 스킬 하나 먼저 선택
        StartCoroutine(StartRoomCo());
    }

    // 2. 방 시작 로직
    public void StartRoom()
    {
        if (mCurrentRoomIndex >= mStageData.RoomDataList.Count)
        {
            Utils.Log("스테이지 클리어!");

            //스테이지 클리어 이벤트 발행(현재까지의 킬 카운트, 현재 룸 번호(-1), 현재 스테이지 넘버)
            mOnStageClear.Raised(mKillCount, mCurrentRoomIndex, mStageData.ChapterID);

            return;
        }

        // [맵 교체 로직 시작] ==============================================
        // 1. 이전 맵이 있다면 반납 (청소)
        if (mCurrentMapInstance != null)
        {
            Managers.Pool.ReturnToPool(mCurrentMapInstance);
            mCurrentMapInstance = null;
        }

        // 2. 새 방 데이터 가져오기
        RoomDataSO currentRoomData = mStageData.RoomDataList[mCurrentRoomIndex];

        // 3. 새 맵 소환 (풀에서 꺼내기)
        if (currentRoomData.MapPrefab != null)
        {
            StageMap mapPrefab = currentRoomData.MapPrefab.GetComponent<StageMap>();
            if (mapPrefab != null)
            {
                mCurrentMapInstance = Managers.Pool.GetFromPool(mapPrefab);

                mCurrentMapInstance.transform.position = Vector3.zero;
                //mCurrentMapInstance.transform.rotation = Quaternion.identity;

                // 맵 초기화 함수 호출 (필요 시)
                mCurrentMapInstance.OnMapSpawn();
            }
        }

        // [맵 교체 로직 끝] ================================================

        bIsBattleActive = true;

        //문 사용 시 추가할 내용들 ------------------------------------------------
        //if (mDoorObject != null) mDoorObject.SetActive(true); // 문 닫기

        // ---------------------------------------------------------------------

        StartCoroutine(ProcessWaveRoutine());
    }

    // 3. 웨이브 진행 코루틴
    private IEnumerator ProcessWaveRoutine()
    {
        RoomDataSO currentRoom = mStageData.RoomDataList[mCurrentRoomIndex];

        // 웨이브 순차 실행
        for (int i = 0; i < currentRoom.WaveDataList.Count; i++)
        {
            WaveData wave = currentRoom.WaveDataList[i];
            SpawnWaveEnemies(wave);

            // 몬스터가 다 죽을 때까지 대기
            yield return new WaitUntil(() => mAliveEnemyCount <= 0);

            // 다음 웨이브 전 1초 휴식
            if (i < currentRoom.WaveDataList.Count - 1) yield return mWaitOneSec;
        }

        // 모든 웨이브 종료 -> 방 클리어
        RoomClear();
    }

    // 4. 스폰 실행
    private void SpawnWaveEnemies(WaveData wave)
    {
        foreach (var info in wave.spawnInfoList)
        {
            mAliveEnemyCount++;
            StartCoroutine(SpawnEnemyWithDelay(info));
        }
    }

    private IEnumerator SpawnEnemyWithDelay(SpawnInfo info)
    {
        if (info.spawnDelay > 0) yield return new WaitForSeconds(info.spawnDelay);

        if (info.enemyPrefab == null)
        {
            yield break;
        }
        //NavMesh 로딩대기 1프레임
        yield return null;

        // PoolManager 몬스터 생성 요청
        EnemyBase enemyPrefab = info.enemyPrefab.GetComponent<EnemyBase>();
        EnemyBase enemy = Managers.Pool.GetFromPool(enemyPrefab);

        if (enemy != null)
        {
            // 위치 설정
            int index = info.spawnPointIndex % mSpawnPoints.Count;
            enemy.transform.position = mSpawnPoints[index].position;

            Physics.SyncTransforms(); // 물리 갱신

            //NavMesh 로딩대기 1프레임(혹시 몰라 한번 더 대기)
            yield return null;
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            //필요 시 Rotation도 설정 가능

            if (agent != null)
            {
                agent.enabled = true;
                if (agent.isOnNavMesh)
                {
                    agent.isStopped = false;
                }
            }
            else
            {
                enemy.transform.position = mSpawnPoints[index].position;
            }

            // 몬스터 세팅

            enemy.onEnemyDie -= HandleEnemyDeath; // 중복 제거
            enemy.onEnemyDie += HandleEnemyDeath; // 사망 구독

            // 타겟(플레이어) 주입
            enemy.SetTarget(mPlayer.transform);
        }
    }

    // 5. 몬스터 사망 콜백 (Observer)
    private void HandleEnemyDeath(EnemyBase enemy)
    {
        mAliveEnemyCount--;
        Utils.Log($"Enemy Died: {enemy.name}, Alive Count: {mAliveEnemyCount}");
        if (mAliveEnemyCount < 0) mAliveEnemyCount = 0;

        //죽인 에너미 수 누적
        mKillCount++;

        // UI 갱신 등 추가 로직 가능
    }

    // 6. 방 클리어
    private void RoomClear()
    {
        Utils.Log($"Room {mCurrentRoomIndex} Clear!");
        bIsBattleActive = false;

        if (mDoorObject != null) mDoorObject.SetActive(false); // 문 열기

        mCurrentRoomIndex++; // 다음 방 인덱스로 증가
        if (mCurrentRoomIndex >= mStageData.RoomDataList.Count)
        {
            //마지막 룸은 Skill선택UI 보이지 않게
            mOnNoticeLastRoom.Raised();
        }

        //룸 클리어 이벤트 발송 -> 몬스터에서 떨어진 경험치 획득로직
        mOnRoomClear.Raised();

        //다음 방으로 이동 대기 코루틴 시작
        if (mWaitNextRoomCo != null) StopCoroutine(mWaitNextRoomCo);
        mWaitNextRoomCo = StartCoroutine(WaitNextRoomCo());
    }

    // 7. 다음 방으로 이동 코루틴
    private IEnumerator WaitNextRoomCo()
    {
        if (bIsAngelSlimeTurn)
        {
            yield return new WaitUntil(() => bIsAngelSlimeEnded);
        }

        yield return mWaitTwoSec;

        StartRoom();
    }

    // StateScene 진입 시 살짝 대기 후 스킬선택 창 보여주고, StartRoom
    private IEnumerator StartRoomCo()
    {
        yield return mWaitOneSec;
        mOnLevelUpPlayer.Raised(mPlayer.Attack);
        Managers.Game.CanPause = false;

        yield return mWaitOneSec;
        StartRoom();
    }

    private void HandlePlayerDie()
    {
        //플레이어 죽음 관련 처리들

        //1. 죽음발송(현재까지의 킬 카운트, 현재 룸 번호(-1), 현재 스테이지 넘버)
        mOnShowEndUIRequest.Raised(mKillCount, mCurrentRoomIndex, mStageData.ChapterID);
    }
}