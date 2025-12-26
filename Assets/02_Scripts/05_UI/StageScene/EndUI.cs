using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    [SerializeField] private Button mReturnLobbyBtn;
    [SerializeField] private GameObject mRootPannel;
    [SerializeField] private TextMeshProUGUI mGoldText;
    [SerializeField] private TextMeshProUGUI mExpNumText;
    [SerializeField] private TextMeshProUGUI mWaveNumText;
    [SerializeField] private TextMeshProUGUI mChapterNumText;
    [SerializeField] private Image mFillImage;
    [SerializeField] private TextMeshProUGUI mExpProgressText;
    [SerializeField] private TextMeshProUGUI mLevelText;

    //골드받아오기
    [SerializeField] private TextMeshProUGUI mCurrentGetGoldAmountText;

    [Header("이벤트 구독")]
    [SerializeField] private IntTripleEventChannelSO mOnStageClear;        //StageManager.cs 가 발행
    [SerializeField] private IntTripleEventChannelSO mOnShowEndUIRequest;  //StageManager.cs 가 발행


    //로비 상 플레이어의 레벨 프로그레스 바 관련
    //DataManager 생성 -> 골드, 경험치, 최고기록, 장비를 하게되면 인벤토리까지? 저장관련 싹다
    private float mfillAmount;

    private void Awake()
    {
        mReturnLobbyBtn.onClick.RemoveAllListeners();
        mReturnLobbyBtn.onClick.AddListener(OnClickReturnToLobby);
    }
    private void OnEnable()
    {
        mOnStageClear.onEvent += HandleStageClear;
        mOnShowEndUIRequest.onEvent += HandleShowEndUIRequest;

        mRootPannel.SetActive(false);
    }
    private void OnDisable()
    {
        mOnStageClear.onEvent -= HandleStageClear;
        mOnShowEndUIRequest.onEvent -= HandleShowEndUIRequest;

        mRootPannel.SetActive(false);
    }
    private void OnClickReturnToLobby()
    {
        Managers.Game.LoadLobbyScene();
    }
    private void HandleStageClear(int killCount, int roomIndex, int stageNumber)
    {
        ShowEndUI(killCount, roomIndex, stageNumber);

        //클리어 시 는 효과음과 애니메이션을 다르게


    }
    private void HandleShowEndUIRequest(int killCount, int roomIndex, int stageNumber)
    {
        ShowEndUI(killCount, roomIndex, stageNumber);

        //죽어서 나오는 EndUI, 효과음, 애니메이션 다르게

    }

    private void ShowEndUI(int killCount, int roomIndex, int stageNumber)
    {
        mRootPannel.SetActive(true);

        mWaveNumText.SetText(Utils.IntAppend(roomIndex + 1));
        mChapterNumText.SetText(Utils.IntAppend(stageNumber));

        //일단은 획득경험치만 표기
        mExpProgressText.SetText(Utils.IntAppend(CalculateExp(killCount, roomIndex, stageNumber)));
        mExpNumText.SetText(Utils.IntAppend(CalculateExp(killCount, roomIndex, stageNumber)));
        mGoldText.SetText(Utils.StringAppend(mCurrentGetGoldAmountText.text));

        //레벨텍스트와 프로그레스텍스트의 경험치 총량은 DataManager.cs 설계 후 적용

    }

    //획득 한 경험치 프리팹 기준으로 다시 설정하자 =====================================================
    private int CalculateExp(int killCount, int roomIndex, int stageNumber)
    {
        int result = 0;
        //킬카운트와 진행도 기반 Exp 계산
        result = Mathf.RoundToInt(killCount * 10 * (roomIndex + 1) * 0.1f * stageNumber);

        return result;
    }
    private int CalculateGold(int killCount, int roomIndex, int stageNumber)
    {
        int result = 0;
        //킬카운트와 진행도 기반 Gold 계산
        result = Mathf.RoundToInt(killCount * 10 * (roomIndex + 1) * 0.1f * stageNumber * 4);

        return result;
    }
    //획득 한 경험치 프리팹 기준으로 다시 설정하자 =====================================================

}
