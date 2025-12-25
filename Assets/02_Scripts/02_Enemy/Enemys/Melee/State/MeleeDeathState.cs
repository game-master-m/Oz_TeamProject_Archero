using System.Collections;
using UnityEngine;

public class MeleeDeathState : EnemyState
{
    public MeleeDeathState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Utils.Log("Enemy DeatState Enter");
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
