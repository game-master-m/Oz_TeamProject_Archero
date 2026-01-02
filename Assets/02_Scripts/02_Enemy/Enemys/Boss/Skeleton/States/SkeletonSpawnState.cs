using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawnState : SkeletonState
{
    private readonly float mSpawnTime = 2.9f;
    private readonly float mSpawnSpeedMultiplier = 1.0f;

    public bool IsSpawned { get; private set; } = false;

    public SkeletonSpawnState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent)
    {
    }

    public override void Enter()
    {
        if (mSkeleton.Agent.enabled)
        {
            mSkeleton.Agent.velocity = Vector3.zero;
            mSkeleton.Agent.isStopped = true;
        }
        //公利
        mSkeleton.IsInvinciblitiy = true;

        mSkeleton.Anim.speed = mSpawnSpeedMultiplier;
        mSkeleton.Anim.CrossFade(AnimHash.spawn, 0.1f);
        mElapsedTimeBase = 0.0f;
        IsSpawned = false;
    }
    public override void Update() { }
    public override void FixedUpdate()
    {
        mElapsedTimeBase += Time.fixedDeltaTime;
        if (mElapsedTimeBase > mSpawnTime / mSpawnSpeedMultiplier)
        {
            mElapsedTimeBase = 0.0f;
            IsSpawned = true;
        }
    }
    public override void Exit()
    {
        mSkeleton.Anim.speed = 1.0f;
        IsSpawned = false;

        //公利
        mSkeleton.IsInvinciblitiy = false;

        if (mSkeleton.Agent.enabled)
        {
            mSkeleton.Agent.isStopped = false;
        }
    }
}
