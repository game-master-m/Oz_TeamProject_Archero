using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    // 근접공격 에너미의 기능을 여기에 추가하세요.

    //EnemyBase를 상속받아 필요한 기능을 구현합니다.
    //EnemyBase에는 NavMeshAgent, Animator, CapsuleCollider,StateMachine, EnemyStat 등이 이미 포함되어 있습니다.


    #region States
    // 여기에 근접공격 에너미의 상태들을 정의하세요.
    MeleeIdleState mIdleState;
    MeleeMoveState mMoveState;
    MeleeAttackState mAttackState;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        // 근접공격 에너미의 초기화 로직을 여기에 작성하세요.
        InitStats(mStatData);


        // 상태들 생성
        mIdleState = new MeleeIdleState(this);
        mMoveState = new MeleeMoveState(this);
        mAttackState = new MeleeAttackState(this);

        //전환조건 설정
        InitTransitions();
    }
    private void Start()
    {
        // 초기 상태 설정
        mStateMachine.ChangeState(mIdleState);
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
        // 상태 전환 로직을 여기에 작성하세요.
        //mStateMachine.AddTransition(mIdleState, mMoveState, () => true); // 예시
        mStateMachine.AddTransition(mIdleState, mMoveState, () => mTarget != null && Vector3.Distance(transform.position, mTarget.position) <= 10f);
        //현재상태가 Idle일때 move상태로 전환하는 조건: 타겟이 존재하고, 타겟과의 거리가 이하일때
        mStateMachine.AddTransition(mMoveState, mIdleState, () => mTarget == null || Vector3.Distance(transform.position, mTarget.position) > 100f);
        //현재상태가 Move일때 Idle상태로 전환하는 조건: 타겟이 없거나, 타겟과의 거리가 f를 넘을때
        mStateMachine.AddTransition(mMoveState, mAttackState,() => mTarget != null && Vector3.Distance(transform.position, mTarget.position) <= mAttackRange);
        //움직이다가 플레이어가 공격범위 안에 들어오면 공격상태로 전환
        mStateMachine.AddTransition(mAttackState, mMoveState, () => mTarget != null && Vector3.Distance(transform.position, mTarget.position) > mAttackRange + 0.5f);
        //공격하다가 플레이어가 공격범위를 벗어나면 무브상태로 전환
    }
       //TakeDamage(float amount) 필요 시 오버라이드

       //Die() 필요 시 오버라이드
}


