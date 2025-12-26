using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDizzyState : DragonState
{
    private readonly Vector3 spawnOffset = new Vector3(0.0f, 4.0f, 1.2f);
    public DragonDizzyState(DragonController dragon, IState parent = null) : base(dragon, parent) { }

    public override void Enter()
    {
        Utils.Log("Dragon Dizzy State ÁøÀÔ!!");

        mDragon.Agent.velocity = Vector3.zero;
        mDragon.Agent.isStopped = true;

        Vector3 worldSpawnPos = mDragon.transform.TransformPoint(spawnOffset);

        EffectBase dizzy = Managers.Pool.GetFromPool(mDragon.Board.DizzyEffectPrefab);
        dizzy.Setup(worldSpawnPos, Quaternion.identity);
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
        mDragon.Agent.isStopped = false;
        mDragon.DizzyCount = 0;
    }
}
