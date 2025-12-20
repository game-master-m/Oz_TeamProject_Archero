
public class DragonState : IState
{
    protected readonly DragonController mDragon;
    protected float mElapsedTimeBase = 0f;
    public IState Parent { get; }

    public DragonState(DragonController dragon, IState parent = null)
    {
        this.mDragon = dragon;
        Parent = parent;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
