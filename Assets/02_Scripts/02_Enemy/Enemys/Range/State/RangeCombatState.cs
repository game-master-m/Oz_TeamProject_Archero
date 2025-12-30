using UnityEngine;

public class RangeCombatState : EnemyState
{
    private Node mChaseAndShot;
    private float mWaitAfterShotAnimation = 1.0f;
    private float mProjectileSpeed = 15.0f;

    public RangeCombatState(EnemyBase enemy, EProjectileName ball, IState parent = null) : base(enemy, parent)
    {
        mWaitAfterShotAnimation = mEnemy.ShotAndWaitTime;
        mProjectileSpeed = mEnemy.ProjectileSpeed;

        SelectorNode node;
        switch (ball)
        {
            case EProjectileName.SmallWindBall:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SmallWindBall));
                break;
            case EProjectileName.SmallFireBall:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SmallFireBall));
                break;
            case EProjectileName.SmallWaterBall:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SmallWaterBall));
                break;
            case EProjectileName.SmallMagicBall:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SmallMagicBall));
                break;
            case EProjectileName.SnakeBall:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SnakeBall));
                break;
            case EProjectileName.SplitBall:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SplitBall));
                break;
            case EProjectileName.HomingFireBall:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.HomingFireBall));
                break;
            default:
                node = BT_Builder.GetChaseAndShotBT(mEnemy, mEnemy.Board, 20.0f, mEnemy.AttackSpeed, mWaitAfterShotAnimation, mProjectileSpeed, mEnemy.Board.SpawnOffset, () => Managers.Pool.GetFromPool(mEnemy.Board.SmallFireBall));
                break;
        }
        mChaseAndShot = new RepeaterNode(
                node
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
