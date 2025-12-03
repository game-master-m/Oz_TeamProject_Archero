using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : MonoBehaviour
{
    // 런타임 참조 변수 (Initializer에게서 받음)
    private StageDataSO mStageData;
    private PlayerController mPlayer;
    private List<Transform> mSpawnPoints;
    private GameObject mDoorObject;

    // [상태 변수]
    private int mCurrentRoomIndex = 0;
    private int mAliveEnemyCount = 0;
    private bool bIsBattleActive = false;
    private StageMap mCurrentMapInstance;

    // 1. 스테이지 초기화 (Initializer가 호출)
    public void SetupStage(StageDataSO data, PlayerController player, List<Transform> points, GameObject door)
    {
        mStageData = data;
        mPlayer = player;
        mSpawnPoints = points;
        mDoorObject = door;

        // 변수 리셋
        mCurrentRoomIndex = 0;
        mAliveEnemyCount = 0;
        bIsBattleActive = false;


        // 첫 번째 방 시작
        StartRoom();
    }

    // 2. 방 시작 로직
    public void StartRoom()
    {
        if (mCurrentRoomIndex >= mStageData.RoomDataList.Count)
        {
            Utils.Log("스테이지 클리어!");
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
            if (i < currentRoom.WaveDataList.Count - 1)
                yield return new WaitForSeconds(1.0f);
        }

        // 모든 웨이브 종료 -> 방 클리어
        RoomClear();
    }

    // 4. 스폰 실행
    private void SpawnWaveEnemies(WaveData wave)
    {
        foreach (var info in wave.spawnInfoList)
        {
            StartCoroutine(SpawnEnemyWithDelay(info));
        }
    }

    private IEnumerator SpawnEnemyWithDelay(SpawnInfo info)
    {
        if (info.spawnDelay > 0) yield return new WaitForSeconds(info.spawnDelay);

        // PoolManager 몬스터 생성 요청
        if (info.enemyPrefab == null)
        {
            yield break;
        }
        EnemyBase enemyPrefab = info.enemyPrefab.GetComponent<EnemyBase>();
        EnemyBase enemy = Managers.Pool.GetFromPool(enemyPrefab);

        if (enemy != null)
        {
            // 위치 설정
            int index = info.spawnPointIndex % mSpawnPoints.Count;
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                agent.Warp(mSpawnPoints[index].position);
            }
            else
            {
                enemy.transform.position = mSpawnPoints[index].position;
            }

            // 몬스터 세팅
            mAliveEnemyCount++;
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
        if (mAliveEnemyCount < 0) mAliveEnemyCount = 0;

        // UI 갱신 등 추가 로직 가능
    }

    // 6. 방 클리어
    private void RoomClear()
    {
        bIsBattleActive = false;
        if (mDoorObject != null) mDoorObject.SetActive(false); // 문 열기
        mCurrentRoomIndex++; // 다음 방 인덱스로 증가
    }
}