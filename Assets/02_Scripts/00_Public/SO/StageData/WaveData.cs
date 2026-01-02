using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnInfo
{
    //각각의 에너미 프리팹의 이름
    public GameObject enemyPrefab;
    //인덱스에 따라 스폰 포인트 지정(예, 10군데의 스폰포인트를 만들어 놓고, 0~9까지의 인덱스로 지정)
    public int spawnPointIndex;
    //줘도 되고 안줘도 되는 값, 줬을 경우 해당 시간만큼 딜레이 후 스폰
    public float spawnDelay;
    //한번에 소환 될 숫자
    public int spawnCount;
}

[System.Serializable]
public class WaveData
{
    public List<SpawnInfo> spawnInfoList;
}