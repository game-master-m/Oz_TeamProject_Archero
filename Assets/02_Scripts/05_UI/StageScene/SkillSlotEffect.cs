using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotEffect : MonoBehaviour
{
    [Header("Effect 변수")]
    [SerializeField] private Image mIconCenter;
    [SerializeField] private Image mIconTop;

    [SerializeField] private float mItemHeight = 160f;
    [SerializeField] private float mSpinDuration = 0.1f;

    private List<Sprite> mReelSkillIconList = new List<Sprite>();

    private Queue<Sprite> mDeckQue = new Queue<Sprite>();

    private bool bIsSpinning = false;
    private Sprite mFinalTargetSprite;

    private Image mCurrentCenter;
    private Image mCurrentTop;

    private Sequence mCurrentSeq;
    private Action onComplete;
    private void Awake()
    {
        Initialize();
    }
    private void OnDisable()
    {
        KillSequence();
    }
    public void PlaySpinInitial(List<SkillDataSO> reelList)
    {
        ResetEffect();
        UpdateReelSprites(reelList);

        bIsSpinning = true;
        DoSpinLoop();
    }
    public void UpdateReelSprites(List<SkillDataSO> reelList)
    {
        mReelSkillIconList.Clear();

        foreach (var item in reelList)
        {
            mReelSkillIconList.Add(item.icon);
        }
        mDeckQue.Clear();
    }
    public void StopSpin(Sprite finalSprite, Action onComplete)
    {
        mFinalTargetSprite = finalSprite;
        bIsSpinning = false;
        this.onComplete = onComplete;
    }

    private Sprite GetSpriteFromQueue()
    {
        if (mReelSkillIconList == null || mReelSkillIconList.Count == 0) return null;

        if (mDeckQue.Count == 0)
        {
            RefillQueue();
        }

        return mDeckQue.Dequeue();
    }
    private void RefillQueue()
    {
        if (mReelSkillIconList == null || mReelSkillIconList.Count == 0) return;

        int count = mReelSkillIconList.Count;
        int startIndex = UnityEngine.Random.Range(0, count);

        if (mCurrentTop != null && mCurrentTop.sprite != null)
        {
            Sprite startSprite = mReelSkillIconList[startIndex];
            if (startSprite == mCurrentTop.sprite)
            {
                startIndex = (startIndex + 1) % count;
            }
        }

        mDeckQue.Clear();

        for (int i = 0; i < count; i++)
        {
            int currentIndex = (startIndex + i) % count;
            Sprite s = mReelSkillIconList[currentIndex];
            if (s != null)
            {
                mDeckQue.Enqueue(s);
            }
        }
    }
    private void ResetEffect()
    {
        Initialize();

        mIconCenter.rectTransform.DOKill();
        mIconTop.rectTransform.DOKill();

        mCurrentCenter.transform.localScale = Vector3.one;
        mCurrentTop.transform.localScale = Vector3.one;

        //Time.timeScale = 0 일 때, 위치 이동 씹힘 방지
        //Canvas.ForceUpdateCanvases();
    }
    private void DoSpinLoop()
    {
        mCurrentSeq = DOTween.Sequence();
        mCurrentSeq.SetUpdate(true);
        mCurrentSeq.Join(mCurrentCenter.rectTransform.DOAnchorPosY(-mItemHeight, mSpinDuration).SetEase(Ease.Linear));
        mCurrentSeq.Join(mCurrentTop.rectTransform.DOAnchorPosY(0, mSpinDuration).SetEase(Ease.Linear));

        mCurrentSeq.OnComplete(() =>
        {
            // 1. 위치 리셋
            mCurrentCenter.rectTransform.anchoredPosition = new Vector2(0, mItemHeight);

            // 2. 역할 교대 
            var temp = mCurrentCenter;
            mCurrentCenter = mCurrentTop;
            mCurrentTop = temp;

            if (bIsSpinning)
            {
                mCurrentTop.sprite = GetSpriteFromQueue();
                //재귀함수 : bIsSpinning이 false일 때까지 반복 실행.
                DoSpinLoop();
            }
            else
            {
                //bIsSpinning이 true면 정해진 sprite를 위로 보내서 마지막 애니메이션 실행
                mCurrentTop.sprite = mFinalTargetSprite;
                DoFinalLand(onComplete);
            }
        });
    }


    private void DoFinalLand(Action onComplete)
    {
        KillSequence();
        mCurrentSeq = DOTween.Sequence();
        mCurrentSeq.SetUpdate(true);

        mCurrentSeq.Join(mCurrentCenter.rectTransform.DOAnchorPosY(-mItemHeight, mSpinDuration * 1.5f).SetEase(Ease.OutSine));
        mCurrentSeq.Join(mCurrentTop.rectTransform.DOAnchorPosY(0, mSpinDuration * 6.0f).SetEase(Ease.OutBack));

        mCurrentSeq.OnComplete(() =>
        {
            mCurrentTop.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f, 10, 2).SetUpdate(true);

            //다 하면 PlaySpin() 의 인자로 넘겨받은 onComplete 실행
            onComplete?.Invoke();
        });
    }

    private void Initialize()
    {
        KillSequence();

        mIconCenter.rectTransform.anchoredPosition = Vector2.zero;
        mIconTop.rectTransform.anchoredPosition = new Vector2(0, mItemHeight);

        mCurrentCenter = mIconCenter;
        mCurrentTop = mIconTop;
    }
    private void KillSequence()
    {
        if (mCurrentSeq != null && mCurrentSeq.IsActive())
        {
            mCurrentSeq.Kill();
            mCurrentSeq = null;
        }
    }


}