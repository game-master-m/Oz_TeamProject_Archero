using System.Collections.Generic;
using UnityEngine;

public class DragonIdleState : DragonState
{
    private Node mPatrolBT;
    private float mPatrolRange = 10.0f;
    private float mMinWaitTime = 2.0f;
    private float mMaxWaitTime = 4.0f;
    public DragonIdleState(DragonController dragon, IState parent = null) : base(dragon, parent)
    {
        BuildPatrolBT();
    }

    public override void Enter()
    {
        Utils.Log("Dragon Idle State 진입!!");

        //행동트리 초기화(강제 종료)
        mPatrolBT.Abort();
    }
    public override void Update()
    {
        mPatrolBT.Evaluate();
    }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        //행동트리 초기화(강제 종료)
        mPatrolBT.Abort();
    }

    private void BuildPatrolBT()
    {
        Node setup = new SetRandomPatrolDataNode(mDragon, mDragon.Board, mPatrolRange, mMinWaitTime, mMaxWaitTime);

        // 2. 이동 (보드에 설정된 LastKnownPos 사용)
        Node move = new MoveToNextPosNode(mDragon, mDragon.Board);

        // 3. 대기 (보드에 설정된 CurrentWaitTime 사용)
        Node wait = new WaitNode(mDragon, mDragon.Board);

        // 4. 순차 실행 (메모리 기능을 켜서 이동 중 중단되어도 이어서 진행)
        SequenceNode seq = new SequenceNode(new List<Node> { setup, move, wait }, true);

        // 5. 무한 반복
        mPatrolBT = new RepeaterNode(seq);
    }
}
