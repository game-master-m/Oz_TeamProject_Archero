using System.Collections.Generic;
using UnityEngine;

public class DragonSecondPhaseState : DragonState
{
    private Node mPhase2BT;

    //예측 연발 샷
    private readonly int mMaxShot = 15;
    private readonly float mMoveSpeed = 10.0f;
    private readonly float mFireInterval = 0.15f;
    private readonly Vector3 mSpawnOffset = new Vector3(0, 1.0f, 0.5f);
    private readonly Vector3 mSpawnOffset2 = new Vector3(0.0f, 3.2f, 2.5f);

    //스프레드 샷
    private readonly int mMaxSpreadShot = 10;



    public DragonSecondPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent)
    {
        BuildBT();
    }

    public override void Enter()
    {
        Utils.Log("Dragon Second Phase State 진입!!");
    }
    public override void Update()
    {
        mPhase2BT.Evaluate();
    }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        mDragon.Board.CurrentEffect?.ExecuteEffect();
        mPhase2BT.Abort();
    }

    private void BuildBT()
    {
        Node pressure = new SequenceNode(new List<Node>
        {
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new WaitNode(mDragon,0.1f),
            new SummonFireTrailNode(mDragon,mDragon.Board,2.0f,mSpawnOffset2),
            new FanShotNode(mDragon,mDragon.Board,6,14.0f,1.0f,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBall)),
            new SpreadVollyNode(mDragon,mDragon.Board,5, mMoveSpeed,mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
            new SpreadVollyNode(mDragon,mDragon.Board,5, mMoveSpeed,mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
            new NormalShotNode(mDragon,mDragon.Board,8.0f,1.5f,1.0f,mSpawnOffset,()=>Managers.Pool.GetFromPool(mDragon.Board.BigFireBall)),
            new ConditionNode( () => {mDragon.Board.CurrentEffect.ExecuteEffect(); return true; }),
            new WaitNode(mDragon,1.0f)
        });

        Node harass = new SequenceNode(new List<Node>
        {
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new WaitNode(mDragon,0.1f),
            new SummonFireTrailNode(mDragon,mDragon.Board,3.0f,mSpawnOffset2),
            new SpreadVollyNode(mDragon,mDragon.Board,mMaxSpreadShot, mMoveSpeed,mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
            new PredictVolleyNode(mDragon, mDragon.Board, mMaxShot, mMoveSpeed,mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
            new SpreadVollyNode(mDragon, mDragon.Board, mMaxSpreadShot, mMoveSpeed, mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBall)),
            new ConditionNode( () =>
                {
                    if (mDragon.Board.CurrentEffect != null)
                    {
                        mDragon.Board.CurrentEffect.ExecuteEffect();
                    }
                    return true;
                }),
            new WaitNode(mDragon,1.0f)
        });

        Node fakeFan = new SequenceNode(new List<Node>
        {
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new WaitNode(mDragon,0.1f),
            new SummonFireTrailNode(mDragon,mDragon.Board,2.0f,mSpawnOffset2),
            new FanShotNode(mDragon,mDragon.Board,6,14.0f,1.0f,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBall)),
            new NormalShotNode(mDragon,mDragon.Board,8.0f,1.5f,1.0f,mSpawnOffset,()=>Managers.Pool.GetFromPool(mDragon.Board.BigFireBall)),
            new ConditionNode( () =>
                {
                    if (mDragon.Board.CurrentEffect != null)
                    {
                        mDragon.Board.CurrentEffect.ExecuteEffect();
                    }
                    return true;
                }),
            new WaitNode(mDragon,1.0f)
        });

        Node fakeBig = new SequenceNode(new List<Node>
        {
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new WaitNode(mDragon,0.1f),
            new SummonFireTrailNode(mDragon,mDragon.Board,2.0f,mSpawnOffset2),
            new NormalShotNode(mDragon,mDragon.Board,8.0f,1.5f,1.0f,mSpawnOffset,()=>Managers.Pool.GetFromPool(mDragon.Board.BigFireBall)),
            new ConditionNode( () =>
                {
                    if (mDragon.Board.CurrentEffect != null)
                    {
                        mDragon.Board.CurrentEffect.ExecuteEffect();
                    }
                    return true;
                }),
            new FanShotNode(mDragon,mDragon.Board,6,14.0f,1.0f,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBall)),
            new WaitNode(mDragon,1.0f)
        });

        Node randomAttack = new RandomSelectorNode(new List<Node>
        {
            pressure,harass,fakeBig,fakeFan
        });

        Node mainCycle = new SequenceNode(new List<Node>
        {
            randomAttack,
            new WaitNode(mDragon,1.0f),
            randomAttack,
            new WaitNode(mDragon,1.0f),

            new SetRandomPatrolDataNode(mDragon, mDragon.Board, 8.0f, 0.8f, 1.2f),
            new MoveToNextPosNode(mDragon, mDragon.Board),
            new WaitNode(mDragon,mDragon.Board.CurrentWaitTime),
        });

        mPhase2BT = new RepeaterNode(
            new SelectorNode(new List<Node>
            {
                new SequenceNode(new List<Node>
                {
                    new ConditionNode(() => (mDragon.Target.position - mDragon.transform.position).sqrMagnitude > 800.0f),
                    new ConditionNode( () =>
                    {
                        if (mDragon.Board.CurrentEffect != null)
                        {
                            mDragon.Board.CurrentEffect.ExecuteEffect();
                        }
                        return true;
                    }),
                    new DashAttackNode(mDragon, mDragon.Board, 1.3f, 1.0f, 15.0f, 0.27f, 3.5f),
                    new WaitNode(mDragon,1.5f),
                }),

                mainCycle
            })
        );
    }
}
