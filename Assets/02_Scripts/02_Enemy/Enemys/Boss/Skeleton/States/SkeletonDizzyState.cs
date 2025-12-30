using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDizzyState : SkeletonState
{
    private readonly Vector3 spawnOffset = new Vector3(0.0f, 5.5f, 0.0f);
    private EffectBase mDizzyEffect;
    public SkeletonDizzyState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent) { }

    public override void Enter()
    {
        if (mSkeleton.Agent.enabled == true)
        {
            mSkeleton.Agent.velocity = Vector3.zero;
            mSkeleton.Agent.isStopped = true;
        }

        Vector3 worldSpawnPos = mSkeleton.transform.TransformPoint(spawnOffset);

        mDizzyEffect = Managers.Pool.GetFromPool(mSkeleton.Board.DizzyEffectPrefab);
        mDizzyEffect.Setup(worldSpawnPos, Quaternion.identity);
    }
    public override void Update() { }
    public override void FixedUpdate()
    {
        if (!mSkeleton.IsDizzy) return;

        mElapsedTimeBase += Time.fixedDeltaTime;
        if (mElapsedTimeBase >= mSkeleton.DizzyDuration)
        {
            mElapsedTimeBase = 0;
            mSkeleton.IsDizzy = false;
        }
    }
    public override void Exit()
    {
        if (mSkeleton.Agent.enabled == true) mSkeleton.Agent.isStopped = false;

        mSkeleton.DizzyCount = 0;
        mSkeleton.IsDizzy = false;

        if (mDizzyEffect != null)
        {
            mDizzyEffect.ExecuteEffect();
        }
    }
}
