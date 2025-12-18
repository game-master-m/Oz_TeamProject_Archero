using UnityEngine;
using TMPro;
using DG.Tweening;

public class DmgText : MonoBehaviour
{
    [SerializeField] private TextMeshPro mText;

    [Header("Motion Settings")]
    [SerializeField] private float mLifetime = 1.0f;            // 텍스트 수명
    [SerializeField] private float mShakeDuration = 0.3f;      // 텍스트 수명 * 0.2, 초반에 빠르게 커짐
    [SerializeField] private float mMoveDuration = 0.7f;       // 작아지면서 움직이며 중간부터(0.5f) Fade out
    [SerializeField] private float targetScaleNormal = 1.5f;
    [SerializeField] private float targetScaleCritical = 2.0f;
    [SerializeField] private float shakePowerNormal = 0.3f;
    [SerializeField] private float shakePowerCritical = 0.6f;


    [Header("속성별 색상")]
    [SerializeField] private Color mNormalColor;
    [SerializeField] private Color mFireColor;
    [SerializeField] private Color mLightningColor;
    [SerializeField] private Color mPoisonColor;

    private Vector3 mStartScale;
    private Sequence mCurrentSeq;

    private void Awake()
    {
        mStartScale = transform.localScale;
    }
    private void OnEnable()
    {
        mCurrentSeq?.Kill();
    }
    private void OnDisable()
    {
        mCurrentSeq?.Kill();
    }
    public void Setup(float damage, EDmgElement element, bool isCritical = false)
    {
        mCurrentSeq?.Kill();
        mCurrentSeq = DOTween.Sequence();

        mText.alpha = 1f;

        int dmg = Mathf.RoundToInt(damage); //소수점 반올림
        float targetScale = 1.0f;
        float shakePower = 1.0f;

        // 1. 텍스트 설정
        if (isCritical)
        {
            targetScale = targetScaleCritical;
            shakePower = shakePowerCritical;
            mText.SetText(Utils.ClearAndAppend(Define.Critical, dmg));
            // 크리티컬이면 텍스트를 가장 앞으로(sorting order가 높을수록 앞에 표시 됨)
            mText.sortingOrder = 10;
        }
        else
        {
            targetScale = targetScaleNormal;
            shakePower = shakePowerNormal;
            mText.SetText(Utils.ClearAndAppend(dmg));
            mText.sortingOrder = 5;
        }

        // 2. 속성별 색상 적용
        SetTextColor(element);

        // 3. 시퀀스 1 - 생성 즉시 커지면서 흔들림
        mCurrentSeq.Append(transform.DOShakePosition(mLifetime * mShakeDuration, shakePower, 15));
        mCurrentSeq.Join(transform.DOScale(mStartScale * targetScale, mLifetime * mShakeDuration * 0.2f).SetEase(Ease.OutBack));
        mCurrentSeq.Insert(mLifetime * mShakeDuration * 0.2f, transform.
            DOPunchScale(Vector3.one * 0.5f, mLifetime * mShakeDuration * 0.8f, 10, 1));

        // 4. 시퀀스 2 - 우하단으로 작아지면서 움직임
        Vector3 moveDir = (Vector3.right + Vector3.down) * 0.8f;
        mCurrentSeq.Append(transform.DOMove(transform.position + moveDir, mLifetime * mMoveDuration).SetEase(Ease.OutQuad));
        mCurrentSeq.Join(transform.DOScale(mStartScale, mLifetime * mMoveDuration * 0.5f));

        // 5. 시퀀스 3 - 작아지면서 움직이는 도중에 페이드 아웃
        mCurrentSeq.Insert(mLifetime * mMoveDuration * 0.5f, mText.DOFade(0.0f, mLifetime * mMoveDuration));

        // 6. 종료 풀 반환
        mCurrentSeq.OnComplete(() => Managers.Pool.ReturnToPool(this));

    }
    private void SetTextColor(EDmgElement element)
    {
        Color targetColor = mNormalColor;
        switch (element)
        {
            case EDmgElement.Fire: targetColor = mFireColor; break;
            case EDmgElement.Lightning: targetColor = mLightningColor; break;
            case EDmgElement.Poison: targetColor = mPoisonColor; break;
            default: targetColor = mNormalColor; break;
        }
        mText.color = targetColor;
    }
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}