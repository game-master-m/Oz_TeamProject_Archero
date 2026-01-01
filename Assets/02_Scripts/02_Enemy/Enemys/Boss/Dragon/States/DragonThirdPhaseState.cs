using System.Collections.Generic;
using UnityEngine;

public class DragonThirdPhaseState : DragonState
{
    private Node mPhase3BT;

    //예측 연발 샷
    private readonly float mMoveSpeed = 14.0f;
    private readonly float mFireInterval = 0.10f;
    private readonly Vector3 mSpawnOffset = new Vector3(0, 1.0f, 0.5f);
    private readonly Vector3 mSpawnOffset2 = new Vector3(0.0f, 3.2f, 2.5f);

    public DragonThirdPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent)
    {
        BuildBT();
    }

    public override void Enter()
    {
        Utils.Log("Dragon Third Phase State 진입!!");
    }
    public override void Update()
    {
        mPhase3BT.Evaluate();
    }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        mDragon.Board.CurrentEffect?.ExecuteEffect();
        mPhase3BT.Abort();
    }

    private void BuildBT()
    {
        Node meleeCombo = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => Vector3.SqrMagnitude(mDragon.transform.position - mDragon.Target.position) <= mDragon.AttackRange*mDragon.AttackRange),
            new ConditionNode( () =>
                {
                    if (mDragon.Board.CurrentEffect != null)
                    {
                        mDragon.Board.CurrentEffect.ExecuteEffect();
                    }
                    return true;
                }),
            new RotateToTargetNode(mDragon, mDragon.Board, 30.0f),
            new BasicAttackNode(mDragon, mDragon.Board, 0.26f, 5.0f, 2.65f), // 1페이즈 근접공격
            new SpreadVollyNode(mDragon, mDragon.Board, 8, mMoveSpeed, mFireInterval, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
            new WaitNode(mDragon, 0.5f)
        }, true);

        Node gapCloserCombo = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => (mDragon.Target.position - mDragon.transform.position).sqrMagnitude > 450.0f),
            new ConditionNode( () =>
                {
                    if (mDragon.Board.CurrentEffect != null)
                    {
                        mDragon.Board.CurrentEffect.ExecuteEffect();
                    }
                    return true;
                }),
            new SelectorNode(new List<Node>
            {
                new DashAttackNode(mDragon, mDragon.Board, 0.8f, 0.6f, 20.0f, 0.32f, 3.5f),
                new ConditionNode(() => true)
            }),
            new SpinAttackNode(mDragon, mDragon.AttackCol),
            new RotateToTargetNode(mDragon, mDragon.Board, 12.0f),
            new FanShotNode(mDragon, mDragon.Board, 10, 15.0f, 0.2f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBall)),
            new WaitNode(mDragon, 0.8f)
        }, true);

        Node hellPattern = new RandomSelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node>{
                new SummonFireTrailNode(mDragon, mDragon.Board, 1.0f, mSpawnOffset2),
                new PredictVolleyNode(mDragon, mDragon.Board, 20, mMoveSpeed, mFireInterval, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
                new ConditionNode( () =>
                {
                    if (mDragon.Board.CurrentEffect != null)
                    {
                        mDragon.Board.CurrentEffect.ExecuteEffect();
                    }
                    return true;
                }),
            }),
            new SequenceNode(new List<Node>{
                new FanShotNode(mDragon, mDragon.Board, 5, 12.0f, 0.5f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBall)),
                new SpreadVollyNode(mDragon, mDragon.Board, 12, mMoveSpeed, 0.05f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
            }),
            new SequenceNode(new List<Node>{
                new NormalShotNode(mDragon, mDragon.Board, 10.0f, 1.0f, 1.0f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.BigFireBall)),
                new NormalShotNode(mDragon, mDragon.Board, 10.0f, 1.0f, 1.0f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.BigFireBall)),
            }),
            new SequenceNode(new List<Node>{
                new SummonFireTrailNode(mDragon, mDragon.Board, 1.0f, mSpawnOffset2),
                new SpreadVollyNode(mDragon, mDragon.Board, 12, mMoveSpeed, 0.05f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBall)),
                new WaitNode(mDragon, 0.5f),
                new SpreadVollyNode(mDragon, mDragon.Board, 12, mMoveSpeed, 0.05f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBall)),
                new ConditionNode( () =>
                {
                    if (mDragon.Board.CurrentEffect != null)
                    {
                        mDragon.Board.CurrentEffect.ExecuteEffect();
                    }
                    return true;
                }),
            }),
        });

        mPhase3BT = new RepeaterNode(
            new SelectorNode(new List<Node>
            {
                gapCloserCombo,

                meleeCombo,

                new SequenceNode(new List<Node>
                {
                    new RotateToTargetNode(mDragon, mDragon.Board, 15.0f),
                    hellPattern,
                    hellPattern,
                    new WaitNode(mDragon, 0.5f),
                    hellPattern,
                    new SetRandomPatrolDataNode(mDragon, mDragon.Board, 8.0f, 0.5f, 0.8f),
                    new MoveToNextPosNode(mDragon, mDragon.Board),
                    new WaitNode(mDragon, 1.0f)
                }, true)
            })
        );
    }
}
