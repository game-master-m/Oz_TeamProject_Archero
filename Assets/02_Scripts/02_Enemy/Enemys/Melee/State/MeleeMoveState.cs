using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeMoveState : EnemyState
{
    public MeleeMoveState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
    }

    public override void Enter()
    {//무브스테이트로 가면 움직여라
        base.Enter();
        mEnemy.Agent.SetDestination(mEnemy.Target.position);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        mEnemy.Agent.SetDestination(mEnemy.transform.position);
    }
}
