using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyBase : LivingEntity
{
    [SerializeField] protected EnemyStatDataSO mStatData;
    [SerializeField] private ExpPrefab mExpPrefab;

    protected Animator mAnimator;
    protected NavMeshAgent mAgent;
    protected CapsuleCollider mCapsuleCollider;

    protected StateMachine mStateMachine;

    protected float mAttackDamage;
    protected float mAttackRange;
    protected float mAttackSpeed;
    protected float mRotateSpeed;

    public float AttackRange => mAttackRange;
    public float RotateSpeed => mRotateSpeed;
    public float AttackSpeed => mAttackSpeed;
    public float AttackDamage => mAttackDamage;

    public NavMeshAgent Agent => mAgent;

    //플레이어 추적용 타겟
    protected Transform mTarget;

    public event Action<EnemyBase> onEnemyDie;
    public Animator Anim => mAnimator;
    public Transform Target => mTarget;

    protected virtual void Awake()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mCapsuleCollider = GetComponent<CapsuleCollider>();

        // NavMeshAgent 세팅 (속도, 회전 등)
        mAgent.updateRotation = false;

        mStateMachine = new StateMachine();

        Managers.Pool.CreatePool(mExpPrefab, 60, Managers.Pool.transform);
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // 부모의 체력 초기화 실행

        //컬라이더 끄고, StageManager에서 켬
        if (mCapsuleCollider != null) mCapsuleCollider.enabled = true;

        // 적이 다시 살아날 때(풀링) 필요한 초기화
        if (mAgent != null)
        {
            mAgent.enabled = false; // NavMeshAgent 비활성화, StageManager에서 활성화 처리
        }
    }
    protected virtual void OnDisable()
    {
        ResetAgentSetting();
        mCapsuleCollider.enabled = false;
    }
    protected virtual void OnDestroy()
    {
        ResetAgentSetting();
    }
    protected virtual void Update()
    {
        mStateMachine?.Update();
    }
    protected virtual void FixedUpdate()
    {
        mStateMachine?.FixedUpdate();
    }

    private void ResetAgentSetting()
    {
        mTarget = null;
        if (mAgent != null)
        {
            if (mAgent.isOnNavMesh)
            {
                mAgent.velocity = Vector3.zero;
                mAgent.isStopped = true;
                mAgent.enabled = false;
            }
        }
    }
    public virtual void InitStats(EnemyStatDataSO data)
    {
        base.Init(data.MaxHP);
        mAgent.speed = data.MoveSpeed;
        mAttackDamage = data.AttackDamage;
        mAttackRange = data.AttackRange;
        mAttackSpeed = data.AttackSpeed;
        mRotateSpeed = data.RotateSpeed;
    }
    public void SetTarget(Transform target)
    {
        mTarget = target;
    }

    public override void Die()
    {
        base.Die();
        // 0. 죽음 방송~
        onEnemyDie?.Invoke(this);

        // 0. 경험치 드랍
        ExpPrefab exp = Managers.Pool.GetFromPool(mExpPrefab);
        exp.transform.position = transform.position + Vector3.up * 0.5f;
        exp.SetTarget(mTarget);

        // 1. 움직임 멈춤
        if (mAgent.isOnNavMesh)
        {
            mAgent.isStopped = true;
        }
        mAgent.enabled = false;

        // 2. 콜라이더 끄기 (시체에 공격 안 막히게)
        mCapsuleCollider.enabled = false;

        // 3. StageManager에게 알리기 (필요시 이벤트나 매니저 호출)


        // 4. 애니메이션 재생 후 풀로 반환 
        Managers.Pool.ReturnToPool(this);
    }
}
