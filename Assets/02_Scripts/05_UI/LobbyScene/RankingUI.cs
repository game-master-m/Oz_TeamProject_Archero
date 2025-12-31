using TMPro;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] mBestStageNumbers;
    [SerializeField] TextMeshProUGUI[] mBestRoomNumbers;

    private void OnEnable()
    {
        int[] stageNumbers = Managers.Data.GetBestStageNumbers();
        int[] roomNumbers = Managers.Data.GetBestRoomNumbers();

        for (int i = 0; i < stageNumbers.Length; i++)
        {
            Utils.Log($"{stageNumbers[i]}");
            Utils.Log($"{mBestStageNumbers[i]}");
            mBestStageNumbers[i].SetText(Utils.IntAppend(stageNumbers[i]));
            mBestRoomNumbers[i].SetText(Utils.IntAppend(roomNumbers[i]));
        }
    }
}
