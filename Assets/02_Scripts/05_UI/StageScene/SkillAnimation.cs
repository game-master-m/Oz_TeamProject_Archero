using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class SkillAnimation : MonoBehaviour
{
    [Header("Modules")]
    [SerializeField] private SkillSlotEffect mSlotEffect;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup mShowPannelGroup;
    [SerializeField] private Image mLightSlotImage;
    [SerializeField] private Image mGradeImage;
    [SerializeField] private CanvasGroup mLightBase;
    [SerializeField] private CanvasGroup mLightHighlight;
    [SerializeField] private CanvasGroup mIconFrame;

    [SerializeField] private TMP_Text mNameText;
    [SerializeField] private TMP_Text mGradeText;

    [Header("Settings 테스트용")]
    [SerializeField] private float mSpinDuration = 2.5f; // 몇 초 동안 돌릴지
    [SerializeField] private Sprite mFinalSprite;

    [Header("Init Setting value")]
    [SerializeField] private float mAlpahBase = 1.0f;
    [SerializeField] private float mAlpahHight = 0.6f;
    [SerializeField] private Color mNameTextColor;

    [Header("Grade Sprite")]
    [SerializeField] Sprite mGradeLegend;   //W:130 H:95 Y:148
    [SerializeField] Sprite mGradeEpic;     //W:135 H80 Y:142.5
    [SerializeField] Sprite mGradeExpert;   //W:85 H:40 Y:141
    [SerializeField] Sprite mGradeNormal;   //w:85 H:38 Y:140

    private readonly Vector3 mGradeOffsetLegend = new Vector3(130f, 86f, 149f);
    private readonly Vector3 mGradeOffsetEpic = new Vector3(135f, 80f, 146.5f);
    private readonly Vector3 mGradeOffsetExpert = new Vector3(85f, 40f, 141.5f);
    private readonly Vector3 mGradeOffsetNormal = new Vector3(85f, 38f, 140f);
    private readonly float mGradeTextOffsetYLegend = -9.2f;
    private readonly float mGradeTextOffsetYEpic = -5.6f;
    private readonly float mGradeTextOffsetYExpert = -3.8f;
    private readonly float mGradeTextOffsetYNormal = -3.7f;

    private LevelUpUI mLevelUpUI;

    private Vector2 initGradePos;
    private Vector2 initLightSlotPos;
    private readonly float mGradeMoveDistance = 55.0f;

    private ESkillGrade mCurrentGrade;
    private void Awake()
    {
        mLevelUpUI = GetComponent<LevelUpUI>();

        initLightSlotPos = mLightSlotImage.rectTransform.anchoredPosition;
        mLightSlotImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void OnEnable()
    {
        mLevelUpUI.onSelectSkill += HandleLevelUpStart;
    }
    private void OnDisable()
    {
        mLevelUpUI.onSelectSkill -= HandleLevelUpStart;
    }

    public void ShowSkill(Sprite finalIcon, string name)
    {
        Utils.Log("ShowSkill 시작");
        // 1. UI 초기화
        ResetUI();

        //테스트용
        mNameText.text = name;

        // 2. 슬롯 머신 시작
        mSlotEffect.PlaySpin(finalIcon, () =>
        {
            ExpandUI();
        });

        // 3. 일정 시간 뒤에 멈춤 명령(나중에 확률 계산으로 변경 -> 에픽등장 연출추가)
        DOVirtual.DelayedCall(mSpinDuration, () =>
        {
            Utils.Log("DelayedCall!!!");
            mSlotEffect.StopSpin(); // 이제 그만 돌고 결과 내려보내!
        }).SetUpdate(true);
    }

    //다음 등급으로 진입 실패 시, slot 순서대로 StopSpin(); 호출
    //각각의 ExpandUI()가 끝나면 -> 모든 slot LastSequence 실행

    private void ExpandUI()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        // Grade image 위로 이동
        seq.Append(mGradeImage.rectTransform.DOAnchorPosY(initGradePos.y + mGradeMoveDistance, 0.5f).SetEase(Ease.OutBack));

        // 이름 페이드 인
        seq.Append(mNameText.DOFade(1f, 0.8f));

        //테스트용, 끝나자 마자 DownLightSlots 실행
        seq.OnComplete(DownLightSlots);
    }

    //한방에 lightSlot들 내려오고(OutBounce) -> FadeOut과 동시에 ShowPannel을 키고 FadeIn과 동시에 NameText Color Change
    private void DownLightSlots()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        //LightSlot들 내려옴
        seq.Append(mLightSlotImage.rectTransform.DOAnchorPosY(0.0f, 0.8f).SetEase(Ease.OutBounce));
        seq.Join(mLightSlotImage.DOFade(1.0f, 1.0f));
        seq.OnComplete(ChangeColorSeq);

    }

    // 이 색상 변화 Seq
    private void ChangeColorSeq()
    {
        mShowPannelGroup.gameObject.SetActive(true);
        mShowPannelGroup.alpha = 0.0f;

        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        seq.Append(mNameText.DOColor(mNameTextColor, 1.0f));
        seq.Join(mLightSlotImage.DOFade(0.0f, 1.0f));
        seq.Join(mIconFrame.DOFade(0.0f, 1.0f));
        seq.Join(mShowPannelGroup.DOFade(1.0f, 1.0f));
        seq.Join(mLightBase.DOFade(0.0f, 0.8f));
        seq.Join(mLightHighlight.DOFade(0.0f, 0.8f));
    }
    private void ResetUI()
    {
        mSlotEffect.gameObject.SetActive(true);
        mShowPannelGroup.gameObject.SetActive(false);

        mGradeImage.rectTransform.anchoredPosition = initGradePos;
        mLightSlotImage.rectTransform.anchoredPosition = initLightSlotPos;

        mLightBase.alpha = mAlpahBase;
        mLightHighlight.alpha = mAlpahHight;
        mIconFrame.alpha = 1.0f;
        mNameText.color = Color.white;
        mNameText.alpha = 0.0f;
    }

    private void HandleLevelUpStart(List<SkillDataSO> selectedSkills)
    {
        mCurrentGrade = selectedSkills[0].skillGrade;
        ChangeGradeImage(mCurrentGrade);
        //테스트
        ShowSkill(selectedSkills[0].icon, selectedSkills[0].skillName);
    }
    private void ChangeGradeImage(ESkillGrade grade)
    {
        switch (grade)
        {
            case ESkillGrade.None:
            case ESkillGrade.Normal:
                CalGradeImageRect(mGradeNormal, mGradeOffsetNormal);
                CalGradeText(mGradeTextOffsetYNormal, "일반");
                break;
            case ESkillGrade.Expert:
                CalGradeImageRect(mGradeExpert, mGradeOffsetExpert);
                CalGradeText(mGradeTextOffsetYExpert, "고급");
                break;
            case ESkillGrade.Epic:
                CalGradeImageRect(mGradeEpic, mGradeOffsetEpic);
                CalGradeText(mGradeTextOffsetYEpic, "에픽");
                break;
            case ESkillGrade.Legend:
                CalGradeImageRect(mGradeLegend, mGradeOffsetLegend);
                CalGradeText(mGradeTextOffsetYLegend, "전설");
                break;
            default:
                CalGradeImageRect(mGradeNormal, mGradeOffsetNormal);
                CalGradeText(mGradeTextOffsetYNormal, "일반");
                break;
        }
    }
    private void CalGradeImageRect(Sprite grade, Vector3 offset)
    {
        mGradeImage.sprite = grade;
        mGradeImage.rectTransform.sizeDelta = offset;
        Vector2 pos = mGradeImage.rectTransform.anchoredPosition;
        pos.y = offset.z;
        mGradeImage.rectTransform.anchoredPosition = pos;
        initGradePos = pos;
    }
    private void CalGradeText(float y, string grade)
    {
        mGradeText.text = grade;
        Vector2 pos = mGradeText.rectTransform.anchoredPosition;
        pos.y = y;
        mGradeText.rectTransform.anchoredPosition = pos;
    }
}