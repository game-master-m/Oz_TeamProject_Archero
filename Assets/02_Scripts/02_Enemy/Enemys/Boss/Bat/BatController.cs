using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : EnemyBase
{
    [Header("사용스킬 및 이펙트")]
    //아직 고민중

    #region 상태들 선언
    //크게 아이들 , 컴뱃, 죽음
    BatIdleState mIdleState;
    BatDeathState mDeathState;
    BatCombatState mCombatState;
    //컴뱃의 자식들로 Phase1, Phase2, Phase3 -> 내부에 행동트리 구현
    BatFirstPhaseState mFirstPhaseState;
    #endregion


    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();

        //스탯 초기화
        InitStats(mStatData);

        //정지거리를 넉넉하게 잡음
        mAgent.stoppingDistance = 1.5f;

        //상태 생성
        mIdleState = new BatIdleState(this);     //내부 행동트리로 패트롤까지 수행
        mDeathState = new BatDeathState(this);
        mCombatState = new BatCombatState(this);
        //컴뱃의 자식들로 Phase1, Phase2, Phase3 -> 내부에 행동트리 구현
        mFirstPhaseState = new BatFirstPhaseState(this, mCombatState);

        //상태 전이조건
        InitTransitions();
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
        Board.HPPercent = hpPercent;
    }
    #endregion

    #region 전환조건
    private void InitTransitions()
    {
        //Any
        mStateMachine.AddAnyTransition(mDeathState, () => IsDead && !mStateMachine.IsCurrentState(mDeathState));

        //Idle State
        mStateMachine.AddTransition(mIdleState, mCombatState, () => mTarget != null && CheckInDistance(mTarget, mDetectRange));

        //Combat State
        mStateMachine.AddTransition(mCombatState, mIdleState, () => mTarget != null && !CheckInDistance(mTarget, mDetectRange));
        mStateMachine.AddTransition(mCombatState, mFirstPhaseState,
            () => !mStateMachine.IsCurrentState(mFirstPhaseState) && CheckInDistance(mTarget, mDetectRange));
    }
    #endregion
}
