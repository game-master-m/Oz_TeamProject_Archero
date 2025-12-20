using UnityEngine;

public class DragonThirdPhaseState : DragonState
{
    public DragonThirdPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent) { }

    public override void Enter()
    {
        Utils.Log("Dragon Third Phase State ¡¯¿‘!!");
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
