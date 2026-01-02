using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDeathState : DragonState
{
    public DragonDeathState(DragonController dragon, IState parent = null) : base(dragon, parent) { }

    public override void Enter()
    {
        Utils.Log("Dragon Death State ¡¯¿‘!!");
        Managers.Pool.ReturnToPool(mDragon);
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
