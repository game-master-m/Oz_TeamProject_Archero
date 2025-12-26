using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpProgressController : MonoBehaviour
{
    [SerializeField] private Image mExpFillImage;
    [SerializeField] private TextMeshProUGUI mLevelText;
    [SerializeField] private TextMeshProUGUI mGoldText;

    [SerializeField] private LevelUpController mLevelUpController;

    private int mCurrentGoldAmount;
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
    }
    private void OnDisable()
    {
        //해제
        mLevelUpController.onExpChange -= HandleExpChange;
        mLevelUpController.onGoldChange -= HandleGoldChange;
        mLevelUpController.onLevelChange -= HandleLevelChange;
    }

    private void HandleExpChange(float fillAmount)
    {
        mExpFillImage.fillAmount = fillAmount;
        mGetExpCount++;
    }
    private void HandleGoldChange(int getGoldAmount)
    {
        mCurrentGoldAmount += getGoldAmount;
        mGoldText.SetText(Utils.GoldIntAppend(mCurrentGoldAmount));
    }
    private void HandleLevelChange(int level)
    {
        mLevelText.SetText(Utils.LevelIntAppend(level));
    }

}
