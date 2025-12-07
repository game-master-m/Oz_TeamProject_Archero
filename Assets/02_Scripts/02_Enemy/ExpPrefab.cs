using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ExpPrefab : MonoBehaviour
{
    [SerializeField] private float mMoveSpeed = 15f;
    [SerializeField] private int mExpAmount = 50;

    [Header("이벤트 발송")]
    [SerializeField] private IntEventChannelSO mOnGetExpRequest;    //LevelUpController가 구독

    [Header("이벤트 구독")]
    [SerializeField] private VoidEventChannelSO mOnRoomClear; // StageManager가 발송

    private Transform mTarget;
    private Rigidbody mRigidbody;
    private BoxCollider mBoxCollider;

    private Vector3 mMoveDirection;

    private bool bIsRoomClear = false;

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mBoxCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        mOnRoomClear.onEvent += HandleRoomClear;
        mBoxCollider.enabled = false;
        bIsRoomClear = false;

        //이전에 날아다니던 물리적 힘(속도, 회전력)을 반드시 제거해야 함
        if (mRigidbody != null)
        {
            mRigidbody.velocity = Vector3.zero;        // 이동 속도 초기화
            mRigidbody.angularVelocity = Vector3.zero; // 회전 속도 초기화
        }
    }
    private void OnDisable()
    {
        mOnRoomClear.onEvent -= HandleRoomClear;
        bIsRoomClear = false;
        mBoxCollider.enabled = false;

    }
    private void HandleRoomClear()
    {
        bIsRoomClear = true;
        mBoxCollider.enabled = true;
    }
    public void SetTarget(Transform target)
    {
        mTarget = target;
        Utils.Log($"ExpPrefab Set Target: {mTarget.name}");
    }
    private void FixedUpdate()
    {
        if (!bIsRoomClear) return;
        mMoveDirection = (mTarget.position - transform.position).normalized;
        mRigidbody.velocity = mMoveDirection * mMoveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //프로젝트 셋팅의 레이어 충돌 매트릭스에서 Player와만 충돌하도록 설정했으므로 태그 체크 불필요
        //if (!other.CompareTag(Define.Tag_Player)) return;

        mOnGetExpRequest.Raised(mExpAmount);
        Managers.Pool.ReturnToPool(this);
    }

}
