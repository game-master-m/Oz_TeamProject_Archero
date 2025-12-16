using UnityEngine;

public class BossAttackState : EnemyState
{
    public BossAttackState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
