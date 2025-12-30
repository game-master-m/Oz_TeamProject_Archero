using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonController : EnemyBase
{
    [Header("사용스킬 및 이펙트")]
    [SerializeField] private EffectBase mDizzyEffectPrefab;

    [Header("공격용 컬라이더")]

    #region 상태들 선언
    //크게 스폰, 아이들 , 컴뱃, 기절, 죽음
    SkeletonSpawnState mSpawnState;
    SkeletonIdleState mIdleState;
    SkeletonDizzyState mDizzyState;
    SkeletonDeathState mDeathState;
    SkeletonCombatState mCombatState;
    //컴뱃의 자식들로 Phase1, Phase2 -> 내부에 행동트리 구현
    SkeletonFirstPhaseState mFirstPhaseState;
    SkeletonSecondPhaseState mSecondPhaseState;
    #endregion

    private readonly int mMaxDizzyCount = 20;
    private readonly float mDizzyDuration = 2.5f;
    private readonly float mMinDizzyDmgRate = 0.02f;  //총 체력의 2%
    private readonly float mHPRecoverDuration = 1.0f;
    private readonly float mHPRecoverInterval = 0.05f;
    private WaitForSeconds mWaitRecoverInterval;
    #region 프로퍼티
    public bool IsDizzy { get; set; } = false;
    public int DizzyCount { get; set; }
    public float DizzyDuration => mDizzyDuration;
    public int ResurrectionCount { get; set; } = 1;
    public bool IsHPEnd { get; set; } = false;
    #endregion



    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();

        //스탯 초기화
        InitStats(mStatData);

        //이펙트 프리팹
        Board.DizzyEffectPrefab = mDizzyEffectPrefab;

        //정지거리를 넉넉하게 잡음
        mAgent.stoppingDistance = 1.0f;

        //상태 생성
        mSpawnState = new SkeletonSpawnState(this);
        mIdleState = new SkeletonIdleState(this);     //내부 행동트리로 패트롤까지 수행
        mDizzyState = new SkeletonDizzyState(this);
        mDeathState = new SkeletonDeathState(this);
        mCombatState = new SkeletonCombatState(this);
        //컴뱃의 자식들로 Phase1, Phase2, Phase3 -> 내부에 행동트리 구현
        mFirstPhaseState = new SkeletonFirstPhaseState(this, mCombatState);
        mSecondPhaseState = new SkeletonSecondPhaseState(this, mCombatState);

        //상태 전이조건
        InitTransitions();

        mWaitRecoverInterval = new WaitForSeconds(mHPRecoverInterval);
    }
    private void Start()
    {

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

        // 초기 상태 설정
        mStateMachine.ChangeState(mSpawnState);
        ResurrectionCount = 1;

        //이벤트 구독
        onHPChanged += HandleHPChange;  //본인의 LivingEntity
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        ResurrectionCount = 1;

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
        Utils.Log("Skeleton SetTarget!");
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
        mStateMachine.AddAnyTransition(mDeathState, () => IsHPEnd && !mStateMachine.IsCurrentState(mDeathState));
        mStateMachine.AddAnyTransition(mDizzyState, () => IsDizzy && !mStateMachine.IsCurrentState(mDizzyState));

        //Dizzy State
        mStateMachine.AddTransition(mDizzyState, mIdleState, () => !IsDizzy);

        //Spawn State
        mStateMachine.AddTransition(mSpawnState, mIdleState, () => mSpawnState.IsSpawned);
        mStateMachine.AddTransition(mDeathState, mSpawnState, () => mDeathState.IsDeathEnd);


        //Idle State
        mStateMachine.AddTransition(mIdleState, mCombatState, () => mTarget != null && CheckInDistance(mTarget, mDetectRange));

        //Combat State
        mStateMachine.AddTransition(mCombatState, mIdleState, () => mTarget != null && !CheckInDistance(mTarget, mDetectRange));
        mStateMachine.AddTransition(mCombatState, mFirstPhaseState,
            () => !mStateMachine.IsCurrentState(mFirstPhaseState) && ResurrectionCount >= 1);
        mStateMachine.AddTransition(mCombatState, mSecondPhaseState,
            () => !mStateMachine.IsCurrentState(mSecondPhaseState) && ResurrectionCount <= 0);
    }
    #endregion

    #region 헬퍼함수
    private void AddHp(float amount)
    {
        mCurrentHP += amount;
        UpdateHPRequest(mCurrentHP / mMaxHP);
    }
    #endregion

    public override void Die()
    {
        if (ResurrectionCount > 0)
        {
            Utils.Log("살아나기 카운트 0 보다 큼");
            IsHPEnd = true;
        }
        else
        {
            base.Die();
        }

    }

    #region 코루틴
    public IEnumerator RecoverHPCO()
    {
        while (mCurrentHP >= mMaxHP)
        {
            AddHp(mMaxHP / (mHPRecoverDuration / mHPRecoverInterval));
            yield return mWaitRecoverInterval;
        }
    }
    #endregion
}
