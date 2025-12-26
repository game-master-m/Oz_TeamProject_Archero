using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class EnemyProjectileBase : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float mMoveSpeed = 12.0f;
    [SerializeField] protected float mLifeTime = 5.0f;
    [SerializeField] protected Vector3 mHitBoxOffset = Vector3.zero;
    [SerializeField] protected float mHigBoxRadius = 1.0f;

    protected Rigidbody mRigid;
    protected EnemyBase mOwner;
    protected float mCurrentDamage;
    protected float mLifeTimer;
    protected bool bIsActive = false;

    private readonly Collider[] mHitResults = new Collider[1];

    protected virtual void Awake()
    {
        mRigid = GetComponent<Rigidbody>();
        mRigid.useGravity = false;
        mRigid.isKinematic = false; // 물리 기반 이동 준수
    }

    public virtual void Setup(float damage, float speed, Vector3 direction, EnemyBase owner)
    {
        mCurrentDamage = damage;
        mMoveSpeed = speed;
        mOwner = owner;

        direction.y = 0;

        // [회전 로직] 발사 시 방향을 바라보게 설정
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        mLifeTimer = 0f;
        bIsActive = true;
    }

    protected virtual void Update()
    {
        if (!bIsActive) return;

        mLifeTimer += Time.deltaTime;
        if (mLifeTimer >= mLifeTime) ReturnPool();
    }

    protected virtual void FixedUpdate()
    {
        if (!bIsActive) return;

        // 투사체 이동 및 진행 방향 바라보기
        MoveAndRotate();
        CheckCollision();
    }

    protected virtual void MoveAndRotate()
    {
        // 1. 앞으로 전진
        mRigid.velocity = transform.forward * mMoveSpeed;

        // 2. [회전 로직] 속도 벡터가 있다면 그 방향을 바라봄
        if (mRigid.velocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(mRigid.velocity);
        }
    }

    private void CheckCollision()
    {
        int mask = Layers.GetLayerMask(ELayerName.Player);
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position + mHitBoxOffset, mHigBoxRadius, mHitResults, mask);

        if (hitCount > 0)
        {
            if (mHitResults[0].TryGetComponent<IDamageable>(out var target))
            {
                OnHit(target);
            }
        }
    }

    protected virtual void OnHit(IDamageable target)
    {
        target.TakeDamage(mCurrentDamage);
        ReturnPool();
    }

    public virtual void ReturnPool()
    {
        bIsActive = false;
        mRigid.velocity = Vector3.zero;
        // 실제 프로젝트의 풀 매니저 방식에 맞춰 호출
        gameObject.SetActive(false);
    }
}