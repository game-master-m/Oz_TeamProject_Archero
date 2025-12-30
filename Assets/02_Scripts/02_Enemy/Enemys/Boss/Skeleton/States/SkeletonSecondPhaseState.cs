using System.Collections.Generic;
using UnityEngine;

public class SkeletonSecondPhaseState : SkeletonState
{
    private Node mPhase2BT;
    private readonly float mDashCoolTime = 15.0f;
    private readonly float mSummonCoolTime = 10.0f;
    private readonly float mStoneRainningCoolTime = 20.0f;

    private float mSummonCoolTimer = 0;
    private float mStoneRainningCoolTimer = 0;
    private bool bIsDashCoolEnd = true;
    private bool bIsSummonCoolEnd = true;
    private bool bIsStoneRainningCoolEnd = true;

    public SkeletonSecondPhaseState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent)
    {
        mPhase2BT = BuildBT();
    }

    public override void Enter()
    {
        bIsDashCoolEnd = true;
        bIsSummonCoolEnd = true;
        bIsStoneRainningCoolEnd = true;

        mElapsedTimeBase = 0;
        mSummonCoolTimer = 0;
        mStoneRainningCoolTimer = 0;
    }
    public override void Update()
    {
        mPhase2BT.Evaluate();
    }
    public override void FixedUpdate()
    {
        //대쉬 쿨타임
        if (!bIsDashCoolEnd)
        {
            mElapsedTimeBase += Time.fixedDeltaTime;
            if (mElapsedTimeBase > mDashCoolTime)
            {
                mElapsedTimeBase = 0.0f;
                bIsDashCoolEnd = true;
            }
        }

        //셔먼 슬라임 쿨타임
        if (!bIsSummonCoolEnd)
        {
            mSummonCoolTimer += Time.fixedDeltaTime;
            if (mSummonCoolTimer > mSummonCoolTime)
            {
                mSummonCoolTimer = 0.0f;
                bIsSummonCoolEnd = true;
            }
        }

        //스톤 레이닝 쿨타임
        if (!bIsStoneRainningCoolEnd)
        {
            mStoneRainningCoolTimer += Time.fixedDeltaTime;
            if (mStoneRainningCoolTimer > mStoneRainningCoolTime)
            {
                mStoneRainningCoolTimer = 0.0f;
                bIsStoneRainningCoolEnd = true;
            }
        }
    }
    public override void Exit()
    {
        if (mSkeleton.Agent.enabled)
        {
            mSkeleton.Agent.velocity = Vector3.zero;
        }
        mPhase2BT.Abort();
    }

    private Node BuildBT()
    {
        //1. 셔먼(쿨타임 15초) - 앞쪽에 슬라임 세마리 소환하고 튀기
        SequenceNode summonSlime = new SequenceNode(new List<Node>
            {
                new ConditionNode(() => bIsSummonCoolEnd),
                new ConditionNode(() => { bIsSummonCoolEnd = false; return true; }),
                new CWaitNode(mSkeleton,0.1f,false),
                new RotateToTargetNode(mSkeleton,mSkeleton.Board,20.0f),
                new SummonNode(mSkeleton, 8.0f, mSkeleton.AttackSpeed/2.0f),
                new CWaitNode(mSkeleton,1.0f),
                BT_Builder.GetPatrolBT(mSkeleton, mSkeleton.Board, 8.0f, 0.5f, 1.0f),
            });


        //2. 대쉬(쿨타임 10초) -> 빠른 공격 + 사거리내에 있으면 천천히 한번 더 공격
        SequenceNode dash = new SequenceNode(new List<Node>
                {
                    new ConditionNode(() => bIsDashCoolEnd),
                    new ConditionNode(() => { bIsDashCoolEnd = false; return true; }),
                    new CDashNode(mSkeleton, mSkeleton.Board, 1.8f, 1.4f, 30f, 0.3f, 1.5f),
                    new CWaitNode(mSkeleton,0.1f,false),
                    new RotateToTargetNode(mSkeleton,mSkeleton.Board,20.0f),
                    new CAttackNode(mSkeleton,mSkeleton.AttackSpeed,3.0f,3.0f,3.0f),
                    new CWaitNode(mSkeleton,0.1f,false),
                    new SelectorNode(new List<Node>
                    {
                        BT_Builder.GetNormalAttackBT(mSkeleton,mSkeleton.Board,20.0f,mSkeleton.AttackSpeed,3.0f,3.0f,0.1f),
                        new ConditionNode(() => true),
                    }),
                    new CWaitNode(mSkeleton,1.0f),
                });

        //3. 스톤레이닝 장판 소환
        SequenceNode stoneRainning = new SequenceNode(new List<Node>
                {
                    new ConditionNode(() => bIsStoneRainningCoolEnd),
                    new ConditionNode(() => { bIsStoneRainningCoolEnd = false; return true; }),
                    new CWaitNode(mSkeleton,0.1f,false),
                    new RotateToTargetNode(mSkeleton,mSkeleton.Board,20.0f),
                    new SummonStoneRainNode(mSkeleton,mSkeleton.Board,mSkeleton.Board.CurrentEffect,0.5f,20.0f,10),
                    new CWaitNode(mSkeleton,1.0f),
                });

        return new RepeaterNode
            (
                new SelectorNode(new List<Node>
                {
                    stoneRainning,
                    summonSlime,
                    dash,
                    BT_Builder.GetChaseAndAttackBT(mSkeleton, mSkeleton.Board, 20.0f, mSkeleton.AttackSpeed,3.0f,3.0f ,1.0f),
                })
            );
    }
}
