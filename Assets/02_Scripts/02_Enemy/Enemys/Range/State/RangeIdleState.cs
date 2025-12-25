using System.Collections;
using UnityEngine;

public class RangeIdleState : EnemyState
{
    private Node mPatrolBT;
    public RangeIdleState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
        mPatrolBT = new RepeaterNode(BT_Builder.GetPatrolBT(enemy, enemy.Board, 10.0f, 1.5f, 2.5f));
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        mPatrolBT.Evaluate();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        mPatrolBT?.Abort();
    }

}
