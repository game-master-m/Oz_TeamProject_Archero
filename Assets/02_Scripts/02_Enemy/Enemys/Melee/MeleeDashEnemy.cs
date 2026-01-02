using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDashEnemy : MeleeEnemy
{
    // 근접공격 에너미의 기능을 여기에 추가하세요.

    //EnemyBase를 상속받아 필요한 기능을 구현합니다.
    //EnemyBase에는 NavMeshAgent, Animator, CapsuleCollider,StateMachine, EnemyStat 등이 이미 포함되어 있습니다.

    #region States
    // 여기에 근접공격 에너미의 상태들을 정의하세요.
    MeleeIdleState mIdleState;
    MeleeCombatState mCombatState;
    MeleeDeathState mDeathState;
    MeleeDashState mDashState;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        // 근접공격 에너미의 초기화 로직을 여기에 작성하세요.
        InitStats(mStatData);

        // 상태들 생성
        mIdleState = new MeleeIdleState(this);
        mCombatState = new MeleeCombatState(this);
        mDeathState = new MeleeDeathState(this);
        mDashState = new MeleeDashState(this);
        //전환조건 설정
        InitTransitions();
    }
    private void Start()
    {

    }
    protected override void Update()
    {
        base.Update();
        //테스트용 타겟 추척
        if (mTarget != null)
        {
            //mAgent.SetDestination(mTarget.position);
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // 추가적인 고정 업데이트 로직이 필요하면 여기에 작성하세요.
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        // 추가적인 활성화 로직이 필요하면 여기에 작성하세요.

        // 초기 상태 설정
        mStateMachine.ChangeState(mIdleState);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        // 추가적인 비활성화 로직이 필요하면 여기에 작성하세요.
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        // 추가적인 파괴 로직이 필요하면 여기에 작성하세요.
    }
    public override void InitStats(EnemyStatDataSO data)
    {
        base.InitStats(data);
        // 근접공격 에너미의 스탯 초기화 로직을 여기에 작성하세요.
    }

    private void InitTransitions()
    {
        //LivingEntity의 IsDead(mCurrentHP = 0 ) true일 때, 항상 데쓰스테이트로 감 -> 다른 state exit()에서 BT 종료
        mStateMachine.AddAnyTransition(mDeathState, () => IsDead && !mStateMachine.IsCurrentState(mDeathState));

        mStateMachine.AddTransition(mIdleState, mCombatState,
            () => mTarget != null && CheckInDistance(mTarget, mDetectRange));

        mStateMachine.AddTransition(mCombatState, mIdleState,
            () => mTarget == null || !CheckInDistance(mTarget, mDetectRange));
        mStateMachine.AddTransition(mCombatState, mDashState,
            () => !mStateMachine.IsCurrentState(mDashState) && CheckInDistance(mTarget, mDetectRange));
    }

    //TakeDamage(float amount) 필요 시 오버라이드

    //Die() 필요 시 오버라이드

}
