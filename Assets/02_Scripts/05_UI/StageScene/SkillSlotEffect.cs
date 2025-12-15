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
    [SerializeField] private float mSpinDuration = 0.2f;

    [Header("Data")]
    [SerializeField] private List<Sprite> dummySprites;

    private bool bIsSpinning = false;
    private Sprite mFinalTargetSprite;

    private Image mCurrentCenter;
    private Image mCurrentTop;

    private void Awake()
    {
        mIconCenter.rectTransform.anchoredPosition = Vector2.zero;
        mIconTop.rectTransform.anchoredPosition = new Vector2(0, mItemHeight);

        mCurrentCenter = mIconCenter;
        mCurrentTop = mIconTop;
    }

    public void PlaySpin(Sprite resultSprite, Action onComplete = null)
    {
        mFinalTargetSprite = resultSprite;
        bIsSpinning = true;
        DoSpinLoop(onComplete);
    }

    private void DoSpinLoop(Action onComplete)
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(mCurrentCenter.rectTransform.DOAnchorPosY(-mItemHeight, mSpinDuration).SetEase(Ease.Linear));
        seq.Join(mCurrentTop.rectTransform.DOAnchorPosY(0, mSpinDuration).SetEase(Ease.Linear));

        seq.OnComplete(() =>
        {
            // 1. 위치 리셋
            mCurrentCenter.rectTransform.anchoredPosition = new Vector2(0, mItemHeight);

            // 2. 역할 교대 
            var temp = mCurrentCenter;
            mCurrentCenter = mCurrentTop;
            mCurrentTop = temp;

            if (bIsSpinning)
            {
                mCurrentTop.sprite = dummySprites[UnityEngine.Random.Range(0, dummySprites.Count)];
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

    public void StopSpin()
    {
        bIsSpinning = false;
    }

    private void DoFinalLand(Action onComplete)
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(mCurrentCenter.rectTransform.DOAnchorPosY(-mItemHeight, mSpinDuration * 2f).SetEase(Ease.InQuad));
        seq.Join(mCurrentTop.rectTransform.DOAnchorPosY(0, mSpinDuration * 2f).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            mCurrentTop.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
            //다 하면 PlaySpin() 의 인자로 넘겨받은 onComplete 실행
            onComplete?.Invoke();
        });
    }
}