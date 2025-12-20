using UnityEngine;

public class DragonIdleState : DragonState
{
    public DragonIdleState(DragonController dragon, IState parent = null) : base(dragon, parent) { }

    public override void Enter()
    {
        Utils.Log("Dragon Idle State ¡¯¿‘!!");
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
