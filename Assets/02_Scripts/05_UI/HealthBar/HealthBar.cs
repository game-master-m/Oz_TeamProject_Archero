using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Canvas mCanvas;
    [SerializeField] private Image mFillImage;
    [SerializeField] private Image mBgImage;

    private LivingEntity mTargetEntity;
    private Camera mCam;
    private float mTargetFill = 1f;
    private float mSmoothSpeed = 10f; // 체력바가 부드럽게 깎이는 속도

    private void Awake()
    {
        mTargetEntity = GetComponentInParent<LivingEntity>();

        // 성능 최적화: RaycastTarget 해제
        if (mFillImage != null) mFillImage.raycastTarget = false;
        if (mBgImage != null) mBgImage.raycastTarget = false;

        if (mCanvas != null) mCanvas.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(InitAfterOneFrame());
    }
    private void OnEnable()
    {
        StartCoroutine(InitAfterOneFrame());
    }

    private void OnDisable()
    {
        if (mTargetEntity != null)
        {
            mTargetEntity.onHPChanged -= UpdateHealthBar;
        }
    }
    private void LateUpdate()
    {
        if (mCam == null) mCam = Camera.main;

        // 1. 빌보드 (카메라 정면 보기)
        if (mCanvas.renderMode == RenderMode.WorldSpace && mCam != null)
        {
            transform.rotation = mCam.transform.rotation;
        }

        // 2. 부드러운 게이지 감소 효과
        if (mFillImage != null && Mathf.Abs(mFillImage.fillAmount - mTargetFill) > 0.005f)
        {
            mFillImage.fillAmount = Mathf.Lerp(mFillImage.fillAmount, mTargetFill, Time.deltaTime * mSmoothSpeed);
        }
    }
    private void Initialize()
    {
        mCam = Camera.main;

        if (mTargetEntity != null)
        {
            mTargetEntity.onHPChanged += UpdateHealthBar;
            // 켜질 때 체력바 즉시 갱신
            if (mTargetEntity.MaxHP > 0)
            {
                UpdateHealthBar(mTargetEntity.CurrentHP / mTargetEntity.MaxHP);
                if (mFillImage != null) mFillImage.fillAmount = mTargetFill;
                mCanvas.enabled = true;
            }
            else
            {
                if (mCanvas != null) mCanvas.enabled = false;
            }

        }
    }
    private void UpdateHealthBar(float ratio)
    {
        if (float.IsNaN(ratio) || float.IsInfinity(ratio)) return;

        mTargetFill = ratio;

        // 꽉 찼거나 죽었을 때 숨기기
        if (mCanvas != null)
        {
            if (ratio >= 0.99f || ratio <= 0f)
            {
                //mCanvas.enabled = false;
            }
            else
            {
                mCanvas.enabled = true;
            }
        }
    }

    private IEnumerator InitAfterOneFrame()
    {
        yield return null;
        Initialize();
    }
}