using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class SkillSlotEffect : MonoBehaviour
{
    [Header("Effect 변수")]
    [SerializeField] private Image mIconCenter;
    [SerializeField] private Image mIconTop;

    [SerializeField] private float mItemHeight = 160f;
    [SerializeField] private float mSpinDuration = 0.1f;

    [Header("Data 테스트용")]
    [SerializeField] private List<Sprite> dummySprites;

    private Queue<Sprite> mDeckQue = new Queue<Sprite>();

    private bool bIsSpinning = false;
    private Sprite mFinalTargetSprite;

    private Image mCurrentCenter;
    private Image mCurrentTop;

    private Sequence mCurrentSeq;
    private void Awake()
    {
        Initialize();
    }

    private void OnDisable()
    {
        KillSequence();
    }
    public void PlaySpin(Sprite resultSprite, Action onComplete = null)
    {
        if (mCurrentCenter == null) Initialize();
        ResetEffect();

        mFinalTargetSprite = resultSprite;
        bIsSpinning = true;
        DoSpinLoop(onComplete);
    }
    public void StopSpin()
    {
        bIsSpinning = false;
    }
    private Sprite GetSpriteFromQueue()
    {
        if (dummySprites == null || dummySprites.Count == 0) return null;

        if (mDeckQue.Count == 0)
        {
            RefillQueue();
        }

        return mDeckQue.Dequeue();
    }
    private void RefillQueue()
    {
        if (dummySprites == null || dummySprites.Count == 0) return;

        int count = dummySprites.Count;
        int startIndex = UnityEngine.Random.Range(0, count);

        if (mCurrentTop != null && mCurrentTop.sprite != null)
        {
            Sprite startSprite = dummySprites[startIndex];
            if (startSprite == mCurrentTop.sprite)
            {
                startIndex = (startIndex + 1) % count;
            }
        }

        mDeckQue.Clear();

        for (int i = 0; i < count; i++)
        {
            int currentIndex = (startIndex + i) % count;
            Sprite s = dummySprites[currentIndex];
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
    private void DoSpinLoop(Action onComplete)
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
                DoSpinLoop(onComplete);
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