using System.Collections;
using UnityEngine;

public class SummonedEnemy : EnemyBase
{
    [Header("Projectile")]
    [SerializeField] private EnemyProjectileBase mProjectilePrefab;
    [SerializeField] private Vector3 mSpawnOffset = new Vector3(0.0f, 0.5f, 2.0f);

    [Header("Summoned Enemy ¼³Á¤")]
    [SerializeField] private float mLifeTime = 5.0f;

    RangeIdleState mIdleState;
    RangeCombatState mCombatState;
    RangeDeathState mDeathState;

    private bool bIsLifeTimeEnd = false;

    public Vector3 SpawnOffset => mSpawnOffset;
    protected override void Awake()
    {
        base.Awake();

        mAnim = GetComponentInChildren<Animator>();

        Board.SpawnOffset = mSpawnOffset;
        Board.SnakeBall = mProjectilePrefab;

        mIdleState = new RangeIdleState(this);
        mCombatState = new RangeCombatState(this, EProjectileName.SnakeBall);
        mDeathState = new RangeDeathState(this);

        InitTransitions();

        if (mProjectilePrefab != null)
        {
            Managers.Pool.CreatePool(mProjectilePrefab, 20, Managers.Pool.transform);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        InitStats(mStatData);

        mStateMachine.ChangeState(mIdleState);

        bIsLifeTimeEnd = false;

        StartCoroutine(LifeTimerCO());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    private void InitTransitions()
    {
        mStateMachine.AddAnyTransition(mDeathState, () => bIsLifeTimeEnd || (IsDead && !mStateMachine.IsCurrentState(mDeathState)));

        mStateMachine.AddTransition(mIdleState, mCombatState,
            () => mTarget != null && CheckInDistance(mTarget, DetectRange));

        mStateMachine.AddTransition(mCombatState, mIdleState,
            () => mTarget == null || !CheckInDistance(mTarget, DetectRange));
    }

    private IEnumerator LifeTimerCO()
    {
        yield return new WaitForSeconds(mLifeTime);
        bIsLifeTimeEnd = true;
    }
}
