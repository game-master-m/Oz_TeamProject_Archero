using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatState : IState
{
    protected readonly BatController mBat;
    protected float mElapsedTimeBase = 0f;
    public IState Parent { get; }

    public BatState(BatController bat, IState parent = null)
    {
        this.mBat = bat;
        Parent = parent;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
