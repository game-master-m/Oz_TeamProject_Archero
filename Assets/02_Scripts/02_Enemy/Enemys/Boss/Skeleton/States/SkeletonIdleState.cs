using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonState
{
    private Node mPatrolBT;
    public SkeletonIdleState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent)
    {
        mPatrolBT = new RepeaterNode
            (
                BT_Builder.GetPatrolBT(mSkeleton, mSkeleton.Board, 10.0f, 0.5f, 1.0f)
            );
    }

    public override void Enter()
    {
        mSkeleton.Anim.CrossFade(AnimHash.idle, 0.1f);
    }
    public override void Update()
    {
        mPatrolBT.Evaluate();
    }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        mPatrolBT.Abort();
    }
}
