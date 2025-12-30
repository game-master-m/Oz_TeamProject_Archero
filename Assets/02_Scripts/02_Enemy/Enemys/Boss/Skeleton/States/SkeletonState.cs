using UnityEngine;

public class SkeletonState : IState
{
    protected readonly SkeletonController mSkeleton;
    protected float mElapsedTimeBase = 0f;
    public IState Parent { get; }

    public SkeletonState(SkeletonController skeleton, IState parent = null)
    {
        mSkeleton = skeleton;
        Parent = parent;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
