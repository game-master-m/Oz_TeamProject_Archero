using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSecondPhaseState : SkeletonState
{
    public SkeletonSecondPhaseState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent)
    {
    }

    public override void Enter() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
