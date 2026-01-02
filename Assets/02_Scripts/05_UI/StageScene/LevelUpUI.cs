using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private SkillAnimation[] mSkillAnimations;
    [Header("UI 할당")]
    [SerializeField] private SkillSlotUI[] mSkillSlots; // 인스펙터에서 3개 할당
    [SerializeField] private GameObject mRootPanel;   // 껐다 켰다 할 패널
    [SerializeField] private GameObject mRootStageProgress;
    [SerializeField] private GameObject mAlwaysRoomImage;

    [Header("이벤트 구독")]
    [SerializeField] private PlayerAttackEventChannelSO mOnLevelUpPlayer;   // StageManager가 발송


    [Header("스킬등급 확률")]
    [SerializeField] private float mLegendChance = 0.1f;
    [SerializeField] private float mEpicChance = 0.2f;
    [SerializeField] private float mExpertChance = 0.3f;

    // 이번 스테이지에 등장 가능한 모든 스킬 리스트를 담은 SO
    [SerializeField] private SkillContainerSO mSkills;

    private PlayerAttack mTargetPlayer;

    private Dictionary<ESkillGrade, List<SkillDataSO>> mSkillDic = new Dictionary<ESkillGrade, List<SkillDataSO>>();

    public Dictionary<ESkillGrade, List<SkillDataSO>> SkillDic => mSkillDic;
    public SkillContainerSO SkillContainer => mSkills;
    public event Action<SkillDataSO> onSelectSkill;


    private void Start()
    {
        mSkillDic.Clear();
        List<SkillDataSO> list = new List<SkillDataSO>(SkillContainer.AllSkills);
        foreach (var skill in list)
        {
            if (!mSkillDic.ContainsKey(skill.skillGrade))
            {
                mSkillDic[skill.skillGrade] = new List<SkillDataSO>();
            }
            mSkillDic[skill.skillGrade].Add(skill);
        }
        mRootPanel.SetActive(false);
    }
    private void OnEnable()
    {
        mOnLevelUpPlayer.onEvent += ShowLevelUpPanel;
    }
    private void OnDisable()
    {
        mOnLevelUpPlayer.onEvent -= ShowLevelUpPanel;
    }

    // 외부(StageManager)에서 레벨업 시 이벤트로 호출
    public void ShowLevelUpPanel(PlayerAttack player)
    {
        mTargetPlayer = player;

        // 1. 게임 일시정지
        Time.timeScale = 0f;
        mRootPanel.SetActive(true);
        mRootStageProgress.SetActive(true);
        mAlwaysRoomImage.SetActive(false);

        // 2. 랜덤 스킬 3개 뽑기 (중복 없이)
        List<SkillDataSO> randomSkills = GetRandomSkills(3, GetGradeAsChance());

        //각각의 스킬애니메이션 실행
        for (int i = 0; i < randomSkills.Count; i++)
        {
            mSkillAnimations[i].StartSlotAnimation(randomSkills[i], mSkillDic, i);
        }

        // 3. 슬롯에 데이터 세팅 및 클릭 이벤트 연결
        for (int i = 0; i < mSkillSlots.Length; i++)
        {
            if (i < randomSkills.Count)
            {
                mSkillSlots[i].gameObject.SetActive(true);
                mSkillSlots[i].Setup(randomSkills[i], OnSkillSelected);
            }
        }
    }

    private ESkillGrade GetGradeAsChance()
    {
        float roll = UnityEngine.Random.Range(0.0f, 1.0f);
        if (roll < mLegendChance) return ESkillGrade.Legend;
        if (roll < mEpicChance + mLegendChance) return ESkillGrade.Epic;
        if (roll < mExpertChance + mEpicChance + mLegendChance) return ESkillGrade.Expert;
        return ESkillGrade.Normal;
    }
    // 슬롯이 클릭되었을 때 실행될 함수
    private void OnSkillSelected(SkillDataSO selectedSkill)
    {
        // 1. 플레이어에게 스킬 주입
        if (mTargetPlayer != null)
        {
            mTargetPlayer.AddSkill(selectedSkill);
            onSelectSkill?.Invoke(selectedSkill);
            //스택킹 스킬이 아니면 목록에서 제거(앞으로 안 보여줌)
            if (!selectedSkill.isStacking)
            {
                ESkillGrade grade = selectedSkill.skillGrade;
            }

            Utils.Log($"Skill Added: {selectedSkill.skillName}");
        }

        // 2. 팝업 닫기 및 게임 재개
        Managers.Game.CanPause = true;
        Close();
    }

    private void Close()
    {
        mRootPanel.SetActive(false);
        mRootStageProgress.SetActive(false);
        mAlwaysRoomImage.SetActive(true);
        Time.timeScale = 1f; // 게임 시간 정상화
    }

    // 랜덤 스킬 뽑기 유틸리(업그레이드 필요, 같은 스킬 업그레이드 등)
    private List<SkillDataSO> GetRandomSkills(int count, ESkillGrade grade)
    {
        Debug.Log($"[Debug] 요청 등급: {grade}, 실제 리스트 개수: {mSkillDic[grade].Count}");

        List<SkillDataSO> result = new List<SkillDataSO>();
        List<SkillDataSO> tempList = new List<SkillDataSO>(mSkillDic[grade]);

        Utils.Log($"tempList 의 갯수 : {tempList.Count}");
        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            int rnd = UnityEngine.Random.Range(i, tempList.Count);

            SkillDataSO temp = tempList[i];
            tempList[i] = tempList[rnd];
            tempList[rnd] = temp;

            result.Add(tempList[i]);
        }
        return result;
    }
}
