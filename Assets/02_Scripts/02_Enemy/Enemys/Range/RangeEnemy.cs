using UnityEngine;

using UnityEngine;

public class RangeEnemy : EnemyBase
{
    [Header("Projectile")]
    [SerializeField] private EnemyProjectile mProjectilePrefab;
    [SerializeField] private Transform mFirePoint;

    RangeIdleState mIdleState;
    RangeMoveState mMoveState;
    RangeAttackState mAttackState;

    protected override void Awake()
    {
        base.Awake();

        mIdleState = new RangeIdleState(this);
        mMoveState = new RangeMoveState(this);
        mAttackState = new RangeAttackState(this);

        InitTransitions();

        Managers.Pool.CreatePool(mProjectilePrefab, 20, Managers.Pool.transform);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        InitStats(mStatData);
        mAgent.enabled = true;

        mStateMachine.ChangeState(mIdleState);
    }

    private void InitTransitions()
    {
        mStateMachine.AddTransition(mIdleState, mMoveState,
            () => mTarget != null && CheckInDistance(mTarget, DetectRange));

        mStateMachine.AddTransition(mMoveState, mIdleState,
            () => mTarget == null || !CheckInDistance(mTarget, DetectRange));

        mStateMachine.AddTransition(mMoveState, mAttackState,
            () => mTarget != null && CheckInDistance(mTarget, AttackRange));

        mStateMachine.AddTransition(mAttackState, mMoveState,
            () => mTarget == null || !CheckInDistance(mTarget, AttackRange));
    }

    // 🔥 실제 공격 (State에서 호출)
    public void FireProjectile()
    {
        if (mTarget == null) return;

        EnemyProjectile proj = Managers.Pool.GetFromPool(mProjectilePrefab);
        proj.transform.position = mFirePoint.position;
        proj.Fire(mTarget.position, AttackDamage);
    }
}
