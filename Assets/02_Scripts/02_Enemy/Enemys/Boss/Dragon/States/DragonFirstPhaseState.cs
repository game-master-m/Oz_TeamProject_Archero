using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFirstPhaseState : DragonState
{
    public DragonFirstPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent) { }

    public override void Enter()
    {
        Utils.Log("Dragon FirstPhase State ¡¯¿‘!!");
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
