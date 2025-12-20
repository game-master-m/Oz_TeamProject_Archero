using UnityEngine;

public class DragonCombatState : EnemyState
{
    public DragonCombatState(EnemyBase enemy, IState parent = null) : base(enemy, parent) { }

    public override void Enter() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
