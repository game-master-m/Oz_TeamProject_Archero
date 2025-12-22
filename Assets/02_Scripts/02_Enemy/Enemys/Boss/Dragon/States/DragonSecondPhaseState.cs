using System.Collections.Generic;
using UnityEngine;

public class DragonSecondPhaseState : DragonState
{
    private Node mPhase2BT;

    //예측 연발 샷
    private readonly int mMaxShot = 25;
    private readonly float mMoveSpeed = 12.0f;
    private readonly float mFireInterval = 0.15f;
    private readonly Vector3 mSpawnOffset = new Vector3(0, 1.0f, 0.5f);


    public DragonSecondPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent)
    {
        mPhase2BT = new RepeaterNode(new SequenceNode(new List<Node>
        {
            new WaitNode(mDragon,mDragon.Board),
            new RotateToTargetNode(mDragon,mDragon.Board,10.0f),
            new PredictVolleyNode(mDragon, mDragon.Board, mMaxShot, mMoveSpeed,mFireInterval ,mSpawnOffset),
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
