using UnityEngine;

public class DragonSecondPhaseState : DragonState
{
    public DragonSecondPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent) { }

    public override void Enter()
    {
        Utils.Log("Dragon Second Phase State ¡¯¿‘!!");
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
