using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //등급 전환 효과 추가
    [SerializeField] private TMP_Text mNameText;
    [SerializeField] private TMP_Text mGradeText;

    [Header("색상변경 컴포넌트")]
    [SerializeField] private Image[] mLightBaseImages;
    [SerializeField] private Image[] mLightHighlightImages;
    [SerializeField] private Image mFrameLightImage;
    [SerializeField] private Color[] mLightBaseColors;
    [SerializeField] private Color[] mLightHighlightColors;
    [SerializeField] private Color[] mFrameLightColors;
    [SerializeField] private Image[] mLightGradeChangeImages;

    [Header("Settings")]
    [SerializeField] private float mGradeSpinDuration = 1.5f;
    [SerializeField] private float mFlashDuration = 1.0f;

    [Header("Init Setting value")]
    [SerializeField] private float mAlpahBase = 1.0f;
    [SerializeField] private float mAlpahHight = 0.6f;
    [SerializeField] private Color mNameTextColor;

    [Header("Grade Sprite")]
    [SerializeField] Sprite mGradeLegend;   //W:130 H:95 Y:148
    [SerializeField] Sprite mGradeEpic;     //W:135 H80 Y:142.5
    [SerializeField] Sprite mGradeExpert;   //W:85 H:40 Y:141
    [SerializeField] Sprite mGradeNormal;   //w:85 H:38 Y:140

    [Header("본인 이벤트(각 슬랏에서 쏴줌)")]
    [SerializeField] VoidEventChannelSO mOnReelEnd;

    private readonly Vector3 mGradeOffsetLegend = new Vector3(130f, 86f, 149f);
    private readonly Vector3 mGradeOffsetEpic = new Vector3(135f, 80f, 146.5f);
    private readonly Vector3 mGradeOffsetExpert = new Vector3(85f, 40f, 141.5f);
    private readonly Vector3 mGradeOffsetNormal = new Vector3(85f, 38f, 140f);
    private readonly float mGradeTextOffsetYLegend = -9.2f;
    private readonly float mGradeTextOffsetYEpic = -5.6f;
    private readonly float mGradeTextOffsetYExpert = -3.8f;
    private readonly float mGradeTextOffsetYNormal = -3.7f;
    private readonly Vector2 mLightGradeChangeOffsetY = new Vector2(0.0f, 680.0f);
    private readonly float mGradeMoveDistance = 55.0f;
    private readonly float mFinalDelay = 0.5f;

    private Vector2 mInitGradePos;
    private Vector2 mInitLightSlotPos;
    private Vector2 mInitLightGradeChangeSize;


    private ESkillGrade mCurrentGrade;
    private Dictionary<ESkillGrade, List<SkillDataSO>> mSkillDic;

    private float mCurrentGradeSpinDuration;
    private float mCurrentFlashDuration;
    private Vector2 mCurrentLigthGradeChangeOffsetY;

    private SkillDataSO mFinalSkill;
    private int mFinishedReelCount = 0;

    private Sequence mCurrentSeq;
    private Tween mCurrentDelayedCall;
    private void Awake()
    {
        mInitLightSlotPos = mLightSlotImage.rectTransform.anchoredPosition;
        mInitLightGradeChangeSize = mLightGradeChangeImages[0].rectTransform.sizeDelta;
        ChangeGradeImages(ESkillGrade.Normal);
        mLightSlotImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
    private void OnEnable()
    {
        mOnReelEnd.onEvent += HandleReelEnd;
    }
    private void OnDisable()
    {
        mOnReelEnd.onEvent -= HandleReelEnd;
        KillAllTweens();
    }
    private void KillAllTweens()
    {
        mFinishedReelCount = 0;
        StopAllCoroutines();
        mCurrentSeq?.Kill();
        mCurrentDelayedCall?.Kill();
        DOTween.Kill(transform);
        DOTween.Kill(mSlotEffect.transform);
    }
    public void StartSlotAnimation(SkillDataSO finalSkill, Dictionary<ESkillGrade, List<SkillDataSO>> skilDic, int index)
    {
        //skillDic 주입 필요.
        mSkillDic = skilDic;

        ResetUI();
        //파이널 스킬네임들 적용
        mNameText.text = finalSkill.skillName;
        mFinalSkill = finalSkill;

        //초기값 세팅 
        //일반등급으로 시작
        mCurrentGrade = ESkillGrade.Normal;

        //각 슬랏 동시 실행
        mSlotEffect.PlaySpinInitial(new List<SkillDataSO>(mSkillDic[mCurrentGrade]));

        //등급이 높을수록 슬롯머신 효과시간 증가(등급당 +10%)
        mCurrentGradeSpinDuration = mGradeSpinDuration;
        //등급이 높을수록 Flash효과 시간증가(등급당 +15%)
        mCurrentFlashDuration = mFlashDuration;
        //등급이 높을수록 LightGradeChange의 크기가 커짐(초기 25%, 등급당 두배)
        mCurrentLigthGradeChangeOffsetY = mLightGradeChangeOffsetY * 0.25f;

        //각 슬랏들 다 실행
        ProcessGradeSpin(index);
    }

    private void HandleReelEnd()
    {
        mFinishedReelCount++;
        if (mFinishedReelCount > 2)
        {
            DownLightSlots();
        }
    }

    private void ResetUI()
    {
        KillAllTweens();

        mShowPannelGroup.gameObject.SetActive(false);
        mSlotEffect.gameObject.SetActive(true);

        mGradeImage.rectTransform.anchoredPosition = mInitGradePos;

        mLightSlotImage.rectTransform.anchoredPosition = mInitLightSlotPos;
        Color col = mLightSlotImage.color;
        col.a = 0.0f;
        mLightSlotImage.color = col;

        mLightBase.alpha = mAlpahBase;
        mLightHighlight.alpha = mAlpahHight;
        mIconFrame.alpha = 1.0f;
        mNameText.color = Color.white;
        mNameText.alpha = 0.0f;
    }


    private void ProcessGradeSpin(int index)
    {
        ChangeGradeImages(mCurrentGrade);

        if (mCurrentGrade == mFinalSkill.skillGrade)
        {
            float finalDuration = mCurrentGradeSpinDuration + (index * mFinalDelay);
            mCurrentDelayedCall = DOVirtual.DelayedCall(finalDuration, () =>
            {
                mSlotEffect.StopSpin(mFinalSkill.icon, () =>
                {
                    ExpandUI(index);
                });
            }).SetUpdate(true);
        }
        else
        {
            mCurrentDelayedCall = DOVirtual.DelayedCall(mCurrentGradeSpinDuration, () =>
            {
                UpgradeFlashFI(() =>
                {
                    //등급이 올라감
                    mCurrentGrade++;

                    // 슬롯 이펙트의 덱을 다음 등급 리스트로 교체
                    mSlotEffect.UpdateReelSprites(new List<SkillDataSO>(mSkillDic[mCurrentGrade]));

                    UpgradeFlashFO();

                    mCurrentFlashDuration *= 1.15f;
                    mCurrentGradeSpinDuration *= 1.1f;
                    mCurrentLigthGradeChangeOffsetY *= 2.0f;
                    // 다음 단계 진행 (재귀 호출)
                    ProcessGradeSpin(index);
                });
            }).SetUpdate(true);
        }
    }
    private void ExpandUI(int index)
    {
        mCurrentSeq = DOTween.Sequence();
        mCurrentSeq.SetUpdate(true);

        // Grade image 위로 이동
        mCurrentSeq.Append(mGradeImage.rectTransform.DOAnchorPosY(mInitGradePos.y + mGradeMoveDistance, 0.5f).SetEase(Ease.OutBack));

        // 이름 페이드 인
        mCurrentSeq.Append(mNameText.DOFade(1f, 0.8f));

        //끝나고 나머지 애들 기다리고 DownLightSlots 실행

        mCurrentSeq.OnComplete(() => { mOnReelEnd?.Raised(); });
    }
    //한방에 lightSlot들 내려오고(OutBounce) -> FadeOut과 동시에 ShowPannel을 키고 FadeIn과 동시에 NameText Color Change
    private void DownLightSlots()
    {
        mCurrentSeq = DOTween.Sequence();
        mCurrentSeq.SetUpdate(true);
        //LightSlot들 내려옴
        mCurrentSeq.Append(mLightSlotImage.rectTransform.DOAnchorPosY(0.0f, 0.7f).SetEase(Ease.OutBounce));
        mCurrentSeq.Join(mLightSlotImage.DOFade(1.0f, 0.4f));
        mCurrentSeq.OnComplete(ChangeNameColorSeq);
    }

    // 이 색상 변화 Seq
    private void ChangeNameColorSeq()
    {
        mShowPannelGroup.gameObject.SetActive(true);
        mShowPannelGroup.alpha = 0.0f;

        mCurrentSeq = DOTween.Sequence();
        mCurrentSeq.SetUpdate(true);

        mCurrentSeq.AppendInterval(0.15f);
        mCurrentSeq.Append(mNameText.DOColor(mNameTextColor, 0.8f));
        mCurrentSeq.Join(mLightSlotImage.DOFade(0.0f, 0.3f));
        mCurrentSeq.Join(mIconFrame.DOFade(0.0f, 0.3f));
        mCurrentSeq.Join(mShowPannelGroup.DOFade(1.0f, 0.7f));
        mCurrentSeq.Join(mLightBase.DOFade(0.0f, 0.3f));
        mCurrentSeq.Join(mLightHighlight.DOFade(0.0f, 0.3f));
    }

    private void UpgradeFlashFI(System.Action onComplete)
    {
        for (int i = 0; i < mLightGradeChangeImages.Length; i++)
        {
            mLightGradeChangeImages[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        Sequence flashSeq = DOTween.Sequence();
        flashSeq.SetUpdate(true);
        flashSeq.Append(mLightBase.DOFade(0.3f, mCurrentFlashDuration * 0.2f).SetEase(Ease.InCubic));
        flashSeq.Join(mIconFrame.DOFade(0.3f, mCurrentFlashDuration * 0.2f).SetEase(Ease.InCubic));
        for (int i = 0; i < mLightGradeChangeImages.Length; i++)
        {
            flashSeq.Join(mLightGradeChangeImages[i].
                DOFade(1.0f, mCurrentFlashDuration * 0.2f).SetEase(Ease.OutCubic));
            flashSeq.Join(mLightGradeChangeImages[i].rectTransform.
                DOSizeDelta(mInitLightGradeChangeSize + mCurrentLigthGradeChangeOffsetY, mCurrentFlashDuration * 0.2f).
                SetEase(Ease.OutCubic));
        }
        flashSeq.OnComplete(() => onComplete?.Invoke());
    }
    private void UpgradeFlashFO()
    {
        Color col = GetCurrentGradeColor(mCurrentGrade);

        Sequence flashFOSeq = DOTween.Sequence();
        flashFOSeq.SetUpdate(true);

        flashFOSeq.Append(mLightGradeChangeImages[0].DOColor(col, mCurrentFlashDuration * 0.1f).SetEase(Ease.InQuad));
        flashFOSeq.Join(mLightGradeChangeImages[1].DOColor(col, mCurrentFlashDuration * 0.1f).SetEase(Ease.InQuad));
        flashFOSeq.Append(mLightBase.DOFade(1.0f, mCurrentFlashDuration * 0.7f).SetEase(Ease.OutQuad));
        flashFOSeq.Join(mIconFrame.DOFade(1.0f, mCurrentFlashDuration * 0.7f).SetEase(Ease.OutQuad));
        for (int i = 0; i < mLightGradeChangeImages.Length; i++)
        {
            flashFOSeq.Join(mLightGradeChangeImages[i].
                DOFade(0.0f, mCurrentFlashDuration * 0.7f).SetEase(Ease.OutSine));
            flashFOSeq.Join(mLightGradeChangeImages[i].rectTransform.
                DOSizeDelta(mInitLightGradeChangeSize, mCurrentFlashDuration * 0.7f).
                SetEase(Ease.InQuad));
        }
    }

    #region Helper 함수들
    private void ChangeGradeImages(ESkillGrade grade)
    {
        switch (grade)
        {
            case ESkillGrade.None:
            case ESkillGrade.Normal:
                CalGradeImageRect(mGradeNormal, mGradeOffsetNormal);
                CalGradeTextPos(mGradeTextOffsetYNormal, "일반");
                ChangeIconFramesColor(ESkillGrade.Normal);
                break;
            case ESkillGrade.Expert:
                CalGradeImageRect(mGradeExpert, mGradeOffsetExpert);
                CalGradeTextPos(mGradeTextOffsetYExpert, "고급");
                ChangeIconFramesColor(ESkillGrade.Expert);
                break;
            case ESkillGrade.Epic:
                CalGradeImageRect(mGradeEpic, mGradeOffsetEpic);
                CalGradeTextPos(mGradeTextOffsetYEpic, "에픽");
                ChangeIconFramesColor(ESkillGrade.Epic);
                break;
            case ESkillGrade.Legend:
                CalGradeImageRect(mGradeLegend, mGradeOffsetLegend);
                CalGradeTextPos(mGradeTextOffsetYLegend, "전설");
                ChangeIconFramesColor(ESkillGrade.Legend);
                break;
            default:
                CalGradeImageRect(mGradeNormal, mGradeOffsetNormal);
                CalGradeTextPos(mGradeTextOffsetYNormal, "일반");
                ChangeIconFramesColor(ESkillGrade.Normal);
                break;
        }
    }
    private Color GetCurrentGradeColor(ESkillGrade grade)
    {
        Color result = Color.white;
        switch (grade)
        {
            case ESkillGrade.Normal:
                result = mLightBaseColors[0];
                break;
            case ESkillGrade.Expert:
                result = mLightBaseColors[1];
                break;
            case ESkillGrade.Epic:
                result = mLightBaseColors[2];
                break;
            case ESkillGrade.Legend:
                result = mLightBaseColors[3];
                break;
        }
        return result;
    }
    private void ChangeIconFramesColor(ESkillGrade grade)
    {
        for (int i = 0; i < mLightBaseImages.Length; i++)
        {
            switch (grade)
            {
                case ESkillGrade.Normal:
                    mLightBaseImages[i].color = mLightBaseColors[0];
                    mLightHighlightImages[i].color = mLightHighlightColors[0];
                    break;
                case ESkillGrade.Expert:
                    mLightBaseImages[i].color = mLightBaseColors[1];
                    mLightHighlightImages[i].color = mLightHighlightColors[1];
                    break;
                case ESkillGrade.Epic:
                    mLightBaseImages[i].color = mLightBaseColors[2];
                    mLightHighlightImages[i].color = mLightHighlightColors[2];
                    break;
                case ESkillGrade.Legend:
                    mLightBaseImages[i].color = mLightBaseColors[3];
                    mLightHighlightImages[i].color = mLightHighlightColors[3];
                    break;
            }
        }
        switch (grade)
        {
            case ESkillGrade.Normal:
                mFrameLightImage.color = mFrameLightColors[0];
                break;
            case ESkillGrade.Expert:
                mFrameLightImage.color = mFrameLightColors[1];
                break;
            case ESkillGrade.Epic:
                mFrameLightImage.color = mFrameLightColors[2];
                break;
            case ESkillGrade.Legend:
                mFrameLightImage.color = mFrameLightColors[3];
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
        mInitGradePos = pos;
    }
    private void CalGradeTextPos(float y, string grade)
    {
        mGradeText.text = grade;
        Vector2 pos = mGradeText.rectTransform.anchoredPosition;
        pos.y = y;
        mGradeText.rectTransform.anchoredPosition = pos;
    }
    #endregion
}