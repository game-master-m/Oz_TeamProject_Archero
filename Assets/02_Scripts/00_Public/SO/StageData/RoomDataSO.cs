using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW RoomDataSO", menuName = "Archero/StageData/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    [SerializeField] private bool bIsBoosRoom;
    [SerializeField] private GameObject mapPrefab;

    [Header("Wave Data")]
    [SerializeField] private List<WaveData> waveDataList = new List<WaveData>();

    public bool IsBossRoom => bIsBoosRoom;
    public GameObject MapPrefab => mapPrefab;
    public List<WaveData> WaveDataList => waveDataList;
}
