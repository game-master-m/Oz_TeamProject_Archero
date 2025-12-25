using UnityEngine;

public class RangeCombatState : EnemyState
{
    private Node mChaseAndShot;
    private float mWaitAfterShotAnimation = 1.0f;
    private float mProjectileSpeed = 15.0f;

    public RangeCombatState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
        mChaseAndShot = new RepeaterNode(
                BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SmallWaterBall))
            );
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        mChaseAndShot.Evaluate();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        mChaseAndShot.Abort();
    }
}
