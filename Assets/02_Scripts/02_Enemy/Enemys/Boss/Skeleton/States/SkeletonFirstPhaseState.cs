using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFirstPhaseState : SkeletonState
{
    private Node mPhase1BT;
    private readonly float mDashCoolTime = 10.0f;
    private readonly float mSummonCoolTime = 15.0f;

    private float mSummCoolTimer = 0;
    private bool bIsDashCoolEnd = true;
    private bool bIsSummonCoolEnd = true;

    public SkeletonFirstPhaseState(SkeletonController skeleton, IState parent = null) : base(skeleton, parent)
    {
        mPhase1BT = BuildBT();

    }
    public override void Enter()
    {
        bIsDashCoolEnd = true;
        bIsSummonCoolEnd = true;

        mElapsedTimeBase = 0;
        mSummCoolTimer = 0;
    }
    public override void Update()
    {
        mPhase1BT.Evaluate();
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

        //셔먼 쿨타임
        if (!bIsSummonCoolEnd)
        {
            mSummCoolTimer += Time.fixedDeltaTime;
            if (mSummCoolTimer > mSummonCoolTime)
            {
                mSummCoolTimer = 0.0f;
                bIsSummonCoolEnd = true;
            }
        }
    }
    public override void Exit()
    {
        if (mSkeleton.Agent.enabled)
        {
            mSkeleton.Agent.velocity = Vector3.zero;
        }
    }

    private Node BuildBT()
    {
        //쿨타임 도입? 
        //1. 셔먼 - 앞쪽에 슬라임 세마리 소환
        SequenceNode summon = new SequenceNode(new List<Node>
            {
                new ConditionNode(() => bIsSummonCoolEnd),
                new ConditionNode(() => { bIsSummonCoolEnd = false; return true; }),
                new CWaitNode(mSkeleton,0.1f,false),
                new RotateToTargetNode(mSkeleton,mSkeleton.Board,20.0f),
                new SummonNode(mSkeleton,10.0f,mSkeleton.AttackSpeed/2.0f),
                new CWaitNode(mSkeleton,1.0f),
            });


        //2. 대쉬(쿨타임 5초) -> 빠른 공격 + 사거리내에 있으면 천천히 한번 더 공격
        SequenceNode dash = new SequenceNode(new List<Node>
                {
                    new ConditionNode(() => bIsDashCoolEnd),
                    new ConditionNode(() => { bIsDashCoolEnd = false; return true; }),
                    new CDashNode(mSkeleton, mSkeleton.Board, 1.8f, 1.4f, 30f, 0.3f, 1.5f),
                    new CWaitNode(mSkeleton,0.1f,false),
                    new RotateToTargetNode(mSkeleton,mSkeleton.Board,20.0f),
                    new CAttackNode(mSkeleton,mSkeleton.AttackSpeed,3.0f,3.0f,2.0f),
                    new CWaitNode(mSkeleton,0.1f,false),
                    BT_Builder.GetNormalAttackBT(mSkeleton,mSkeleton.Board,20.0f,mSkeleton.AttackSpeed,3.0f,3.0f,0.1f),
                    new CWaitNode(mSkeleton,1.0f),
                });

        //3. 커먼체이스앤 어택

        //기본 체이스 앤 어택
        //공격을 플레이어 좌우로 예측해서 공격
        //슬라임 소환 -> 몸빵용 HP 높은 대신에 3초 후 사라짐, 꼬불꼬불 투사체 던지기
        //예측중에 멀어지면 대쉬공격
        return new RepeaterNode
            (
                new SequenceNode(new List<Node>
                {
                    summon,
                    BT_Builder.GetPatrolBT(mSkeleton, mSkeleton.Board, 10.0f, 0.5f, 1.0f),
                })
            );
    }
}
