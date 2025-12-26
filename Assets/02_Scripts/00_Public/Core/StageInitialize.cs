using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInitialize : MonoBehaviour
{
    [Header("이번 스테이지 데이터")]
    [SerializeField] private StageDataSO mStageData;

    [Header("참조 변수")]
    [SerializeField] private PlayerController mPlayer;

    [SerializeField] private List<Transform> mSpawnPoints;

    [SerializeField] private GameObject mDoorObject;

    private List<int> mBossRoomList = new List<int>();

    private void Start()
    {
        if (mStageData == null || mPlayer == null)
        {
            Utils.Log("[StageInitializer] 데이터나 플레이어가 연결되지 않았습니다!");
            return;
        }

        CreateMapPools();
        CreateEnemyPools();

        // StageManager(싱글톤매니저)에게 현재 씬의 모든 정보를 넘겨주고 초기화를 요청합니다.
        Managers.Stage.SetupStage(mStageData, mPlayer, mSpawnPoints, mDoorObject, mBossRoomList);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Managers.Game.LoadStageScene();
        }
    }

    private void CreateMapPools()
    {
        HashSet<string> registeredMaps = new HashSet<string>();

        foreach (var room in mStageData.RoomDataList)
        {
            if (room.MapPrefab != null && !registeredMaps.Contains(room.MapPrefab.name))
            {
                StageMap mapComponent = room.MapPrefab.GetComponent<StageMap>();

                if (mapComponent != null)
                {
                    Managers.Pool.CreatePool(mapComponent, 1, Managers.Pool.transform);
                    registeredMaps.Add(room.MapPrefab.name);
                }
            }
        }
    }
    private void CreateEnemyPools()
    {
        // 중복 생성을 막기 위해 HashSet 사용
        HashSet<string> registeredNames = new HashSet<string>();

        int roomNum = 0;
        foreach (var room in mStageData.RoomDataList)
        {
            roomNum++;
            foreach (var wave in room.WaveDataList)
            {
                foreach (var info in wave.spawnInfoList)
                {
                    GameObject prefabGo = info.enemyPrefab;
                    EnemyBase prefab = prefabGo != null ? prefabGo.GetComponent<EnemyBase>() : null;

                    if (prefab != null && !registeredNames.Contains(prefab.name))
                    {
                        // 풀 매니저에게 생성 요청 (기본 5)
                        switch (prefab.EEnemyType)
                        {
                            case EEnemyType.None:
                            case EEnemyType.Melee:
                            case EEnemyType.Range:
                                Managers.Pool.CreatePool(prefab, 6, Managers.Pool.transform);
                                break;
                            case EEnemyType.Boss:
                                Managers.Pool.CreatePool(prefab, 1, Managers.Pool.transform);
                                mBossRoomList.Add(roomNum);
                                break;
                            default:
                                Utils.Log("EEnemyTpye 이 알맞지 않습니다");
                                break;
                        }

                        // 등록 명단에 추가
                        registeredNames.Add(prefab.name);
                    }
                }
            }
        }
    }
}
