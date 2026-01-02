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

        //클리어 효과음
        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mGameClearSound);

    }
    private void HandleShowEndUIRequest(int killCount, int roomIndex, int stageNumber)
    {
        ShowEndUI(killCount, roomIndex, stageNumber);

        //죽어서 나오는 EndUI 효과음
        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mGameOverSound);
    }

    private void ShowEndUI(int killCount, int roomIndex, int stageNumber)
    {
        Managers.Data.SetBestScore(roomIndex + 1, stageNumber);
        mRootPannel.SetActive(true);

        mWaveNumText.SetText(Utils.IntAppend(roomIndex + 1));
        mChapterNumText.SetText(Utils.IntAppend(stageNumber));

        //표기해주는 경험치는 로비상 플레이어 영구 레벨업 진척도
        int[] expProgress = Managers.Data.GetExpProgress();

        int lobbyExp = expProgress[0];
        int requiredExp = expProgress[1];
        int currentGetExp = expProgress[2];
        int currentLevel = expProgress[3];

        int prevRequiredExp = Managers.Data.GetRequiredExp(currentLevel - 1);

        mExpProgressText.SetText(Utils.IntSlashInt(lobbyExp, requiredExp));
        mExpNumText.SetText(Utils.IntAppend(currentGetExp));
        mLevelText.SetText(Utils.IntAppend(currentLevel));
        mGoldText.SetText(Utils.StringAppend(mCurrentGetGoldAmountText.text));

        float virtualExp = (currentLevel != 1) ? lobbyExp - prevRequiredExp : lobbyExp;
        float virtualRequiredExp = (currentLevel != 1) ? requiredExp - prevRequiredExp : requiredExp;

        mFillImage.fillAmount = virtualExp / virtualRequiredExp;
    }

}
