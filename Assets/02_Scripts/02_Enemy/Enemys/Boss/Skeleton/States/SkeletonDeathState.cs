using System.Collections;
using UnityEngine;

public class SkeletonDeathState : SkeletonState
{
    private readonly float mDeathTime = 0.367f;
    public bool IsDeathEnd { get; private set; } = false;
    public SkeletonDeathState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent)
    {
    }

    public override void Enter()
    {
        //公利
        mSkeleton.IsInvinciblitiy = true;

        mSkeleton.Anim.CrossFade(AnimHash.death, 0.1f);
        if (mSkeleton.ResurrectionCount > 0)
        {
            IsDeathEnd = false;
        }
        else
        {
            mSkeleton.StopAllCoroutines();
            Managers.Pool.ReturnToPool(mSkeleton);
        }
    }
    public override void Update() { }
    public override void FixedUpdate()
    {
        if (mSkeleton.ResurrectionCount > 0)
        {
            mElapsedTimeBase += Time.fixedDeltaTime;
            if (mElapsedTimeBase > mDeathTime)
            {

                mSkeleton.ResurrectionCount--;
                mSkeleton.IsHPEnd = false;
                IsDeathEnd = true;
                mElapsedTimeBase = 0.0f;
                mSkeleton.StartCoroutine(mSkeleton.RecoverHPCO());
            }
        }
    }
    public override void Exit()
    {
        //公利
        //mSkeleton.IsInvinciblitiy = false;

        IsDeathEnd = false;
    }


}
