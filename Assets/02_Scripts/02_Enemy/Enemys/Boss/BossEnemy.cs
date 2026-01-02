using UnityEngine;

public class BossEnemy : EnemyBase
{

    /*
      FSM 
       ㅏMove
       ㅏAttack
            ㄴ행동트리
       ㅏetc...
            
     슬라임
    이동방식
    1.특정위치에서 좌우로 이동
    2.이동했으면 잠시 멈추고 다시 이동


    
    공격방식
    1.플레이를 향해서 미사일1개 발사
    2.발사된 미사일이 4개로 분열
    3.분열된게 벽에 부딪히면 4개로 분열
    4.n초 지속후 사라짐
    

    1.4~5개 정도를 흩뿌려서 발사
    2.n초 지속후 사라짐


    큰 슬라임이 죽으면 중간슬라임n개로 분열
    직선 발사 공격만 쓰기


    중간슬라임죽으면 n개로 분열
    무작위로 상하좌우 움직이기

     */
    BossAttackState mAttackState;
    BossMoveState mMoveState;

    protected override void Awake()
    {
        base.Awake();
        mMoveState=new BossMoveState(this);
        mAttackState=new BossAttackState(this);

        InitStats(mStatData);
        InitTransitions();
    }
    private void Start()
    {
        mStateMachine.ChangeState(mMoveState);
    }
    protected override void Update()
    {
        base.Update();

        //좌우로 무작위 이동
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        //n개로 분열


    }
    public override void InitStats(EnemyStatDataSO data)
    {
        base.InitStats(data);
    }
    protected void InitTransitions()
    {

        // 상태 전환 로직을 여기에 작성하세요.
        //mStateMachine.AddTransition(mIdleState, mMoveState, () => true); // 예시
        

        //좌or우로 움직이다 멈추고 공격상태로 전환
        mStateMachine.AddTransition(mMoveState, mAttackState, () => true);

        


        //플레이어를 향해 공격
        //분열쏘고 n초후 흩뿌리기쏘고>이건 어택에서

        //공격하면 다시 이동으로
        mStateMachine.AddTransition(mAttackState, mMoveState, () => true);


        //공격끝나면 다시 좌우로 움직이기
    }
}
