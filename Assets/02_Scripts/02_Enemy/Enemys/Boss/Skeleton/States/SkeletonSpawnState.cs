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
            IsSpawned = true;
            mElapsedTimeBase = 0.0f;
        }
    }
    public override void Exit()
    {
        mSkeleton.Anim.speed = 1.0f;
        IsSpawned = false;
    }
}
