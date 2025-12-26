using UnityEngine;

public class DragonController : EnemyBase
{
    [Header("사용스킬 및 이펙트")]
    [SerializeField] private EnemyProjectileBase mSmallFireBallPrefab;
    [SerializeField] private EnemyProjectileBase mHomingFireBallPrefab;
    [SerializeField] private EnemyProjectileBase mBigFireBallPrefab;
    [SerializeField] private EffectBase mFireTrailPrefab;

    [Header("꼬리공격용 컬라이더")]
    [SerializeField] private EnemyAttackCol mAttackCol;

    #region 상태들 선언
    //크게 아이들 , 컴뱃, 기절, 죽음
    DragonIdleState mIdleState;
    DragonDizzyState mDizzyState;
    DragonDeathState mDeathState;
    DragonCombatState mCombatState;
    //컴뱃의 자식들로 Phase1, Phase2, Phase3 -> 내부에 행동트리 구현
    DragonFirstPhaseState mFirstPhaseState;
    DragonSecondPhaseState mSecondPhaseState;
    DragonThirdPhaseState mThirdPhaseState;
    #endregion

    #region 프로퍼티
    public bool IsDizzy { get; set; } = false;
    public int DizzyCount { get; set; }
    public float DizzyDuration => mDizzyDuration;
    public EnemyAttackCol AttackCol => mAttackCol;
    #endregion

    private readonly int mMaxDizzyCount = 30;
    private readonly float mDizzyDuration = 1.5f;
    private readonly float mMinDizzyDmgRate = 0.02f;  //총 체력의 2%


    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();

        //스탯 초기화
        InitStats(mStatData);

        Board.SmallFireBall = mSmallFireBallPrefab;
        Board.HomingFireBall = mHomingFireBallPrefab;
        Board.BigFireBall = mBigFireBallPrefab;
        Board.FireTrailPrefab = mFireTrailPrefab;

        mAttackCol.SetUpDmg(mAttackDamage);

        //정지거리를 넉넉하게 잡음
        mAgent.stoppingDistance = 1.5f;

        //상태 생성
        mIdleState = new DragonIdleState(this);     //내부 행동트리로 패트롤까지 수행
        mDizzyState = new DragonDizzyState(this);
        mDeathState = new DragonDeathState(this);
        mCombatState = new DragonCombatState(this);
        //컴뱃의 자식들로 Phase1, Phase2, Phase3 -> 내부에 행동트리 구현
        mFirstPhaseState = new DragonFirstPhaseState(this, mCombatState);
        mSecondPhaseState = new DragonSecondPhaseState(this, mCombatState);
        mThirdPhaseState = new DragonThirdPhaseState(this, mCombatState);

        //상태 전이조건
        InitTransitions();
    }
    private void Start()
    {
        Managers.Pool.CreatePool(mSmallFireBallPrefab, 40, Managers.Pool.transform);
        Managers.Pool.CreatePool(mHomingFireBallPrefab, 20, Managers.Pool.transform);
        Managers.Pool.CreatePool(mBigFireBallPrefab, 10, Managers.Pool.transform);
        Managers.Pool.CreatePool(mFireTrailPrefab, 2, Managers.Pool.transform);
    }
    protected override void Update()
    {
        base.Update();

    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        ResetSetting();

        //이벤트 구독
        onHPChanged += HandleHPChange;  //본인의 LivingEntity

        // 초기 상태 설정
        mStateMachine.ChangeState(mIdleState);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        onHPChanged -= HandleHPChange;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public override void InitStats(EnemyStatDataSO data)
    {
        base.InitStats(data);

    }
    private void ResetSetting()
    {
        Board.HPPercent = 1.0f;
        IsDizzy = false;
        DizzyCount = 0;
    }
    public override void SetTarget(Transform target)
    {
        Utils.Log("Dragon SetTarget!");
        base.SetTarget(target);
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleHPChange(float hpPercent)
    {
        if (Board.HPPercent - hpPercent > mMinDizzyDmgRate && !mStateMachine.IsCurrentState(mDizzyState))
        {
            DizzyCount++;
            if (DizzyCount >= mMaxDizzyCount)
            {
                IsDizzy = true;
            }
        }
        Board.HPPercent = hpPercent;
    }
    #endregion

    #region 전환조건
    private void InitTransitions()
    {
        //Any
        mStateMachine.AddAnyTransition(mDeathState, () => IsDead && !mStateMachine.IsCurrentState(mDeathState));
        mStateMachine.AddAnyTransition(mDizzyState, () => IsDizzy && !mStateMachine.IsCurrentState(mDizzyState));

        //Dizzy State
        mStateMachine.AddTransition(mDizzyState, mIdleState, () => !IsDizzy);

        //Idle State
        mStateMachine.AddTransition(mIdleState, mCombatState, () => mTarget != null && CheckInDistance(mTarget, mDetectRange));

        //Combat State
        mStateMachine.AddTransition(mCombatState, mIdleState, () => mTarget != null && !CheckInDistance(mTarget, mDetectRange));
        mStateMachine.AddTransition(mCombatState, mFirstPhaseState,
            () => !mStateMachine.IsCurrentState(mFirstPhaseState) && Board.HPPercent > 0.7f && Board.HPPercent <= 1.0f);
        mStateMachine.AddTransition(mCombatState, mSecondPhaseState,
            () => !mStateMachine.IsCurrentState(mSecondPhaseState) && Board.HPPercent > 0.3f && Board.HPPercent <= 0.7f);
        mStateMachine.AddTransition(mCombatState, mThirdPhaseState,
            () => !mStateMachine.IsCurrentState(mThirdPhaseState) && Board.HPPercent <= 0.3f);
    }
    #endregion
}
