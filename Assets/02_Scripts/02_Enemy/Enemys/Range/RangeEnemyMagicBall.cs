using UnityEngine;

public class RangeEnemyMagicBall : EnemyBase
{
    [Header("Projectile")]
    [SerializeField] private EnemyProjectileBase mProjectilePrefab;
    [SerializeField] private Vector3 mSpawnOffset = new Vector3(0.0f, 0.5f, 2.0f);

    RangeIdleState mIdleState;
    RangeCombatState mCombatState;
    RangeDeathState mDeathState;

    public Vector3 SpawnOffset => mSpawnOffset;
    protected override void Awake()
    {
        base.Awake();

        mIdleState = new RangeIdleState(this);
        mCombatState = new RangeCombatState(this);
        mDeathState = new RangeDeathState(this);

        InitTransitions();

        if (mProjectilePrefab != null)
        {
            Managers.Pool.CreatePool(mProjectilePrefab, 20, Managers.Pool.transform);
        }
    }
    private void Start()
    {
        Board.SpawnOffset = mSpawnOffset;
        Board.SmallMagicBall = mProjectilePrefab;
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
        mStateMachine.AddAnyTransition(mDeathState, () => IsDead && !mStateMachine.IsCurrentState(mDeathState));

        mStateMachine.AddTransition(mIdleState, mCombatState,
            () => mTarget != null && CheckInDistance(mTarget, DetectRange));

        mStateMachine.AddTransition(mCombatState, mIdleState,
            () => mTarget == null || !CheckInDistance(mTarget, DetectRange));
    }

}
