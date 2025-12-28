using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mGoldText;
    [SerializeField] private TextMeshProUGUI mLevelText;
    [SerializeField] private TextMeshProUGUI mExpProgressText;
    [SerializeField] private TextMeshProUGUI mRemainingExpText;
    [SerializeField] private Image mExpProgressFillImage;

    [Header("이벤트 구독")]
    [SerializeField] private IntTripleEventChannelSO mOnLobbyStartRequest;    //DataManager 발송
    //[SerializeField] private IntEventChannelSO mOnGoldChanged;                  //상점 UI?

    private int mGoldAmount;
    private int mLevelAmount;
    private int mExpAmount;
    private int mRemainingExpAmount;
    private int mRequiredExpAmount;

    private void OnEnable()
    {
        mOnLobbyStartRequest.onEvent += UpdateAll;
    }
    private void OnDisable()
    {
        mOnLobbyStartRequest.onEvent -= UpdateAll;
    }

    //Update 골드
    private void UpdateGoldText(int changedGoldAmount)
    {

    }

    //로비씬 전환될 때, UI 전부 업데이트
    private void UpdateAll(int level, int expAmount, int goldAmount)
    {
        mLevelAmount = level;
        mExpAmount = expAmount;
        mGoldAmount = goldAmount;

        mRequiredExpAmount = Define.RequiredExp * Mathf.RoundToInt(Mathf.Pow(Define.NextExpMultiplier, mLevelAmount - 1));

        mRemainingExpAmount = mRequiredExpAmount - mExpAmount;

        mGoldText.SetText(Utils.ShortenIntAppend(mGoldAmount));
        mLevelText.SetText(Utils.IntAppend(mLevelAmount));
        mRemainingExpText.SetText(Utils.ShortenIntAppend(mRemainingExpAmount));
        mExpProgressText.SetText(Utils.ShortenIntSlashInt(mExpAmount, mRequiredExpAmount));

        mExpProgressFillImage.fillAmount = mExpAmount / mRequiredExpAmount;
    }
}
