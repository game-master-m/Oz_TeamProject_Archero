using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDeathState : BatState
{
    public BatDeathState(BatController bat, IState parent = null) : base(bat, parent) { }

    public override void Enter()
    {
        Utils.Log("Bat Death State ¡¯¿‘!!");
        Managers.Pool.ReturnToPool(mBat);
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
