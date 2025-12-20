using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDizzyState : DragonState
{
    public DragonDizzyState(DragonController dragon, IState parent = null) : base(dragon, parent) { }

    public override void Enter()
    {
        Utils.Log("Dragon Dizzy State ÁøÀÔ!!");
    }
    public override void Update() { }
    public override void FixedUpdate()
    {
        if (!mDragon.IsDizzy) return;

        mElapsedTimeBase += Time.fixedDeltaTime;
        if (mElapsedTimeBase >= mDragon.DizzyDuration)
        {
            mElapsedTimeBase = 0;
            mDragon.IsDizzy = false;
        }
    }
    public override void Exit()
    {
        mDragon.DizzyCount = 0;
    }
}
