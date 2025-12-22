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

    //스프레드 샷
    private readonly int mMaxSpreadShot = 10;
    public DragonSecondPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent)
    {
        mPhase2BT = new RepeaterNode(new SequenceNode(new List<Node>
        {
            new WaitNode(mDragon,mDragon.Board),
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new FanShotNode(mDragon,mDragon.Board,6,14.0f,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBallPrefab)),
            new WaitNode(mDragon,mDragon.Board),
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new SpreadVollyNode(mDragon,mDragon.Board,mMaxSpreadShot, mMoveSpeed,mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBallPrefab)),
            new PredictVolleyNode(mDragon, mDragon.Board, mMaxShot, mMoveSpeed,mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBallPrefab)),
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new SpreadVollyNode(mDragon, mDragon.Board, mMaxSpreadShot, mMoveSpeed, mFireInterval ,mSpawnOffset,() => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBallPrefab)),
        }));
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
    public override void Exit() { }
}
