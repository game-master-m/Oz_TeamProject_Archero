using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIdleState : EnemyState
{
    public RangeIdleState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //유니티 에디터에서만 로그찍기
        Utils.Log("RangeIdle Enter");
        mEnemy.Anim.CrossFade(AnimHash.idle, 0.1f);
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
    }
}
