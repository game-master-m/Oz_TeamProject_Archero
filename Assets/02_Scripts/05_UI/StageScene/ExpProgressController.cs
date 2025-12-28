using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpProgressController : MonoBehaviour
{
    [SerializeField] private Image mExpFillImage;
    [SerializeField] private TextMeshProUGUI mLevelText;
    [SerializeField] private TextMeshProUGUI mGoldText;

    [SerializeField] private Button mPauseButton;

    [SerializeField] private LevelUpController mLevelUpController;

    private int mCurrentGoldAmount;
    //경험치 방식? 현재는 킬 카운트 * 룸진행도에 따라 다름
    private int mGetExpCount;
    private void Awake()
    {
        mCurrentGoldAmount = 0;
        mGetExpCount = 0;
    }
    private void OnEnable()
    {
        //등록
        mLevelUpController.onExpChange += HandleExpChange;
        mLevelUpController.onGoldChange += HandleGoldChange;
        mLevelUpController.onLevelChange += HandleLevelChange;

        //버튼
        mPauseButton.onClick.RemoveAllListeners();
        mPauseButton.onClick.AddListener(OnClickPauseBtn);
    }
    private void OnDisable()
    {
        //해제
        mLevelUpController.onExpChange -= HandleExpChange;
        mLevelUpController.onGoldChange -= HandleGoldChange;
        mLevelUpController.onLevelChange -= HandleLevelChange;

        mPauseButton.onClick.RemoveListener(OnClickPauseBtn);
    }
    private void OnClickPauseBtn()
    {
        Managers.Game.TogglePause();
    }
    private void HandleExpChange(float fillAmount)
    {
        mExpFillImage.fillAmount = fillAmount;
        mGetExpCount++;
    }
    private void HandleGoldChange(int getGoldAmount)
    {
        mCurrentGoldAmount += getGoldAmount;
        mGoldText.SetText(Utils.ShortenIntAppend(mCurrentGoldAmount));
    }
    private void HandleLevelChange(int level)
    {
        mLevelText.SetText(Utils.LevelIntAppend(level));
    }

}
