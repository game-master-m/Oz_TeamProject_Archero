using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDashState : EnemyState
{
    private Node mPhase1BT;
    private float mPatternTimer;
    private float mNextPatternTime;
    private float mLastDistance;
    private bool bIsDashConditionMet = false;

    //BasicAttack BT
    private readonly float mMinDashDist = 8.0f;
    private readonly float mMinDashCheckTime = 5.0f;
    private readonly float mMaxDashCheckTime = 8.0f;
    private readonly float mHitTiming = 0.26f;
    private readonly float mHitBoxOffsetForward = 5.0f;
    private readonly float mHitBoxRadius = 2.65f;

    //DashAttack BT
    private readonly float mChargeTime = 0.7f;
    private readonly float mTargetFixTime = 0.2f;
    private readonly float mMoveSpeedMultiplier = 15.0f;
    private readonly float mAnimSpeedRate = 0.27f;
    private readonly float mColliderRadius = 2.5f;
    public MeleeDashState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
        BuildBT();
    }

    public override void Enter()
    {
        Utils.Log("보스 1페이즈 진입");

        // 해당 페이즈에 특화된 대기 시간을 블랙보드에 주입
        mEnemy.Board.CurrentWaitTime = 0.5f;

        ResetPatternTimer();
        mLastDistance = Vector3.Distance(mEnemy.transform.position, mEnemy.Board.Target.position);
    }

    public override void Update()
    {
        if (!bIsDashConditionMet)
        UpdateConditions();

        mPhase1BT.Evaluate();
    }

    private void UpdateConditions()
    {
        if (mEnemy.Board.Target == null) return;

        mPatternTimer += Time.deltaTime;

        // 지정된 쿨타임(2~4초)이 지났을 때만 거리 체크
        if (mPatternTimer >= mNextPatternTime)
        {
            float currentDist = Vector3.Distance(mEnemy.transform.position, mEnemy.Board.Target.position);

            // 거리가 멀어졌다면 대쉬 플래그 활성화
            if (currentDist > mLastDistance + 0.5f && currentDist > mMinDashDist)
            {
                bIsDashConditionMet = true;
                Utils.Log("대쉬 공격 조건 충족");

                mEnemy.Agent.isStopped = true;
                mEnemy.Agent.velocity = Vector3.zero;
            }
            mPatternTimer = 0.0f;
            mLastDistance = currentDist;
        }
    }

    private void ResetPatternTimer()
    {
        mPatternTimer = 0f;
        mNextPatternTime = Random.Range(mMinDashCheckTime, mMaxDashCheckTime);
        bIsDashConditionMet = false;
    }

    private void BuildBT()
    {
        SequenceNode dashSequence = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => bIsDashConditionMet),
            new CWaitNode(mEnemy, 1.0f),
            new CDashNode(mEnemy, mEnemy.Board, mChargeTime, mTargetFixTime, mMoveSpeedMultiplier, mAnimSpeedRate, mColliderRadius),
            new ConditionNode(() => { ResetPatternTimer(); return true; })
        });

        // 2. 근접 공격 시퀀스 (사거리 이내일 때)
        SequenceNode meleeSequence = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => Vector3.Distance(mEnemy.transform.position, mEnemy.Board.Target.position) <= mEnemy.AttackRange),
            new RotateToTargetNode(mEnemy,mEnemy.Board,20.0f),
            new BatAttackNode(mEnemy, mHitTiming, mHitBoxOffsetForward, mHitBoxRadius)
        });

        // 3. 최상위 셀렉터: 대쉬(준비됐을 때만) > 근접(사거리 내) > 추적(상시)
        mPhase1BT = new SelectorNode(new List<Node>
        {
            dashSequence,
            meleeSequence,
            new CMoveToTargetNode(mEnemy, mEnemy.Board)
        });
    }

    public override void Exit()
    {
        mPhase1BT.Abort();
    }
}
