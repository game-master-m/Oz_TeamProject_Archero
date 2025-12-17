using UnityEngine;
using TMPro;

public class DmgText : MonoBehaviour
{
    [SerializeField] private TextMeshPro mText;

    [Header("Motion Settings")]
    [SerializeField] private float mMoveSpeed = 2.5f;     // 위로 올라가는 속도
    [SerializeField] private float mLifetime = 1.0f;      // 텍스트 수명
    [SerializeField] private Vector3 mCriticalScale = new Vector3(1.5f, 1.5f, 1.5f); // 크리티컬 시 크기 배율

    [Header("속성별 색상")]
    [SerializeField] private Color mNormalColor;
    [SerializeField] private Color mFireColor;
    [SerializeField] private Color mLightningColor;
    [SerializeField] private Color mPoisonColor;

    private float mElapsed;
    private Vector3 mStartScale;

    private void Awake()
    {
        mStartScale = transform.localScale;
    }

    public void Setup(float damage, EDmgElement element, bool isCritical = false)
    {
        mElapsed = 0f;
        mText.alpha = 1f;
        //크리티컬 시, 데미지텍스트 크기 단순 1.5배 테스트
        transform.localScale = isCritical ? mStartScale * 1.5f : mStartScale;

        int dmg = Mathf.RoundToInt(damage); //소수점 반올림

        // 1. 텍스트 설정
        if (isCritical)
        {
            mText.SetText(Utils.ClearAndAppend(Define.Critical, dmg));
            // 크리티컬이면 텍스트를 가장 앞으로(sorting order가 높을수록 앞에 표시 됨)
            mText.sortingOrder = 10;
        }
        else
        {
            mText.SetText(Utils.ClearAndAppend(dmg));
            mText.sortingOrder = 5;
        }

        // 2. 속성별 색상 적용
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

    private void Update()
    {
        // 1. Y축 방향으로 지속 이동 (겹침 해소의 핵심)
        transform.position += Vector3.up * mMoveSpeed * Time.deltaTime;

        // 2. 카메라 바라보기
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        // 3. 페이드 아웃 및 반환
        mElapsed += Time.deltaTime;
        if (mElapsed < mLifetime)
        {
            // 수명 절반 지난 시점부터 투명해지기 시작
            if (mElapsed > mLifetime * 0.5f)
            {
                float alpha = Mathf.Lerp(1f, 0f, (mElapsed - (mLifetime * 0.5f)) / (mLifetime * 0.5f));
                mText.alpha = alpha;
            }
        }
        else
        {
            Managers.Pool.ReturnToPool(this);
        }
    }
}