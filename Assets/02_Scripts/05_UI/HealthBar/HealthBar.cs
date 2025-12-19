using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Canvas mCanvas;
    [SerializeField] private Image mFillImage;
    [SerializeField] private Image mBgImage;

    [Header("부모 타입")]
    [SerializeField] private EHealthType mHealthType;

    [Header("크기 및 y축 조절 (크기x,크기y,높이 y)")]
    [SerializeField] private Vector3 mRectOffeset = new Vector3(2.0f, 0.3f, 3.0f);

    private LivingEntity mTargetEntity;
    private Camera mCam;
    private float mTargetFill = 1f;
    private float mSmoothSpeed = 10f; // 체력바가 부드럽게 깎이는 속도

    private RectTransform mRect;
    private void Awake()
    {
        mTargetEntity = GetComponentInParent<LivingEntity>();
        mRect = GetComponent<RectTransform>();

        UpdateVisuals();

        // 성능 최적화: RaycastTarget 해제
        if (mFillImage != null) mFillImage.raycastTarget = false;
        if (mBgImage != null) mBgImage.raycastTarget = false;

        if (mCanvas != null) mCanvas.enabled = false;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        //if (mRect == null) mRect = GetComponentInParent<RectTransform>();
        //UpdateVisuals();
    }
#endif
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

        if (mHealthType != EHealthType.Boss)
        {

            // 1. 빌보드 (카메라 정면 보기)
            if (mCanvas.renderMode == RenderMode.WorldSpace && mCam != null)
            {
                transform.rotation = mCam.transform.rotation;
            }
        }
        else
        {
            //보스는 따라다지않고 위치 고정..Canvas Type Screen Space - Overlay 적용
            //보스프리팹 완료 후 작성
        }

        // 2. 부드러운 게이지 감소 효과
        if (mFillImage != null && Mathf.Abs(mFillImage.fillAmount - mTargetFill) > 0.005f)
        {
            mFillImage.fillAmount = Mathf.Lerp(mFillImage.fillAmount, mTargetFill, Time.deltaTime * mSmoothSpeed);
        }
    }
    private void UpdateVisuals()
    {
        Vector2 rectPos = mRect.anchoredPosition;
        rectPos.y = mRectOffeset.z;
        mRect.anchoredPosition = rectPos;

        mRect.sizeDelta = new Vector2(mRectOffeset.x, mRectOffeset.y);
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
            switch (mHealthType)
            {
                case EHealthType.Enemy:
                    if (ratio >= 0.99f) mCanvas.enabled = false;
                    else mCanvas.enabled = true;
                    break;
                case EHealthType.Player:
                case EHealthType.Boss:
                    mCanvas.enabled = true;
                    break;
                case EHealthType.None:
                default:
                    Utils.Log("헬쓰타입 체크 요망");
                    break;
            }
            if (ratio <= 0) mCanvas.enabled = false;
        }
    }

    private IEnumerator InitAfterOneFrame()
    {
        yield return null;
        Initialize();
    }
}