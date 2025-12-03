using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW StageDataSO", menuName = "Archero/StageData/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    //스테이지 이름, 필요없으면 빼도 무관
    [SerializeField] private string stageName;
    //스테이지 식별자
    [SerializeField] private int chapterID;

    [Header("Room Data")]
    [SerializeField] private List<RoomDataSO> roomDataList = new List<RoomDataSO>();

    public string StageName => stageName;
    public int ChapterID => chapterID;
    public List<RoomDataSO> RoomDataList => roomDataList;
}
