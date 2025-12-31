using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("이벤트 구독")]
    [SerializeField] private VoidEventChannelSO mOnGameResume;   //GameManager 발행
    [SerializeField] private VoidEventChannelSO mOnGamePause;    //GameManager 발행

    [Header("참조")]
    [SerializeField] private GameObject mPausePannel;
    [SerializeField] private Button mContinueBtn;
    [SerializeField] private Button mExitBtn;

    [Header("획득 스킬 표시 용")]
    [SerializeField] private LevelUpUI mLevelUpUI;
    [SerializeField] private SkillIconPrefab mSkillIconPrefab;
    [SerializeField] private Transform mRoot_SkillIconPrefab;
    [SerializeField] private Sprite[] mFrameSprites;

    [Header("이벤트 구독")]
    [SerializeField] private VoidEventChannelSO mOnSceneChanged;    //GameManager 발행

    private List<SkillDataSO> mSkillDataSOList = new List<SkillDataSO>();
    private List<SkillIconPrefab> mSkillIconPrefabList = new List<SkillIconPrefab>();

    private void Awake()
    {
        mPausePannel.SetActive(false);
        mContinueBtn.onClick.RemoveAllListeners();
        mExitBtn.onClick.RemoveAllListeners();
        mContinueBtn.onClick.AddListener(OnClickContinueBtn);
        mExitBtn.onClick.AddListener(OnClickExitBtn);

        //스킬표시
        Managers.Pool.CreatePool(mSkillIconPrefab, 30, Managers.Pool.transform);
    }
    private void OnEnable()
    {
        //이벤트 발생 시 실행 할 메서드 연결
        mOnGameResume.onEvent += HandleGameResume;
        mOnGamePause.onEvent += HandleGamePause;

        //스킬표시
        mLevelUpUI.onSelectSkill += HandleSelectSkill;
        mOnSceneChanged.onEvent += HandleSceneChange;

        mSkillIconPrefabList.Clear();
    }
    private void OnDisable()
    {
        //메서드 연결 해제
        mOnGameResume.onEvent -= HandleGameResume;
        mOnGamePause.onEvent -= HandleGamePause;

        //스킬표시
        mLevelUpUI.onSelectSkill -= HandleSelectSkill;
        mOnSceneChanged.onEvent -= HandleSceneChange;

        ReturnPoolAll();
    }

    private void Update()
    {
        //편의상 esc키 남겨둠
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.Game.TogglePause();
        }
    }
    private void HandleSceneChange()
    {
        mSkillDataSOList.Clear();
        ReturnPoolAll();
        mSkillIconPrefabList.Clear();
    }
    private void HandleSelectSkill(SkillDataSO skillData)
    {
        mSkillDataSOList.Add(skillData);
    }
    private void OnClickContinueBtn()
    {
        Managers.Game.TogglePause();
    }
    private void OnClickExitBtn()
    {
        Managers.Game.LoadLobbyScene();
    }
    private void HandleGamePause()
    {
        if (mSkillIconPrefabList.Count > 0) ReturnPoolAll();

        //프리팹 뿌리기
        foreach (var item in mSkillDataSOList)
        {
            SkillIconPrefab iconPrefab = Managers.Pool.GetFromPool(mSkillIconPrefab);

            iconPrefab.transform.SetParent(mRoot_SkillIconPrefab, false);
            iconPrefab.IconImage.sprite = item.icon;

            switch (item.skillGrade)
            {
                case ESkillGrade.Normal:
                    iconPrefab.FrameImage.sprite = mFrameSprites[0];
                    break;
                case ESkillGrade.Expert:
                    iconPrefab.FrameImage.sprite = mFrameSprites[1];
                    break;
                case ESkillGrade.Epic:
                    iconPrefab.FrameImage.sprite = mFrameSprites[2];
                    break;
                case ESkillGrade.Legend:
                    iconPrefab.FrameImage.sprite = mFrameSprites[3];
                    break;
                default:
                    iconPrefab.FrameImage.sprite = mFrameSprites[0];
                    break;
            }

            mSkillIconPrefabList.Add(iconPrefab);
        }

        mPausePannel.SetActive(true);
    }
    private void HandleGameResume()
    {
        ReturnPoolAll();
        mPausePannel.SetActive(false);
    }

    private void ReturnPoolAll()
    {
        foreach (var item in mSkillIconPrefabList)
        {
            item.transform.SetParent(Managers.Pool.transform);
            Managers.Pool.ReturnToPool(item);
        }
        mSkillIconPrefabList.Clear();
    }
}
