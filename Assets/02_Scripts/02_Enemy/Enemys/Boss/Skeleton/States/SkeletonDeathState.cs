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
        Utils.Log("스켈레톤 데쓰 스테이트 엔터");
        mSkeleton.Anim.CrossFade(AnimHash.death, 0.1f);
        if (mSkeleton.ResurrectionCount > 0)
        {
            Utils.Log("스켈레톤 데쓰 스테이트 엔터 - 살아나기");
            mSkeleton.ResurrectionCount--;
            IsDeathEnd = false;
        }
        else
        {
            Utils.Log("스켈레톤 데쓰 스테이트 엔터 - 그냥죽기");
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
                mSkeleton.IsHPEnd = false;
                IsDeathEnd = true;
                mElapsedTimeBase = 0.0f;
                mSkeleton.StartCoroutine(mSkeleton.RecoverHPCO());
            }
        }
    }
    public override void Exit()
    {
        IsDeathEnd = false;
    }


}
