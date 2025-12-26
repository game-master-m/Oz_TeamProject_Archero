using UnityEngine;

public class SlimeAttackState : SlimeState
{
    public SlimeAttackState(SlimeController slime, IState parent = null) : base(slime, parent)
    {
        BuildBT();
    }

    public override void Enter()
    {
        base.Enter();
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
    private void BuildBT()
    {

    }
}
