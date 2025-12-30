
public class SkeletonCombatState : SkeletonState
{
    public SkeletonCombatState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent)
    {
    }

    public override void Enter() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
