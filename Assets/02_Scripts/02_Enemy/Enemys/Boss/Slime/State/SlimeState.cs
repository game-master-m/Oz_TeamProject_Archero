using UnityEngine;

public class SlimeState : IState
{
    protected readonly SlimeController mSlime;
    protected float mElapsedTimeBase = 0f;
    public IState Parent { get; }

    public SlimeState(SlimeController slime, IState parent = null)
    {
        this.mSlime = slime;
        Parent = parent;
    }


    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }

}
