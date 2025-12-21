using System.Collections.Generic;
using UnityEngine;

public class DragonFirstPhaseState : DragonState
{
    private Node mPhase1BT;
    private float mPatternTimer;
    private float mNextPatternTime;
    private float mLastDistance;
    private bool bIsDashConditionMet = false;

    public DragonFirstPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent)
    {
        BuildBT();
    }

    public override void Enter()
    {
        Utils.Log("보스 1페이즈 진입");

        // [수정] 해당 페이즈에 특화된 대기 시간을 블랙보드에 주입
        mDragon.Board.CurrentWaitTime = 1.0f;

        ResetPatternTimer();
        mLastDistance = Vector3.Distance(mDragon.transform.position, mDragon.Board.Target.position);
    }

    public override void Update()
    {
        // [수정] 패턴이 이미 준비되었거나 실행 중이면 조건 체크를 스킵하여 리소스 낭비 방지
        if (!bIsDashConditionMet)
        {
            UpdateConditions();
        }

        mPhase1BT.Evaluate();
    }

    private void UpdateConditions()
    {
        if (mDragon.Board.Target == null) return;

        mPatternTimer += Time.deltaTime;

        // 지정된 쿨타임(2~4초)이 지났을 때만 거리 체크
        if (mPatternTimer >= mNextPatternTime)
        {
            float currentDist = Vector3.Distance(mDragon.transform.position, mDragon.Board.Target.position);

            // 거리가 멀어졌다면 대쉬 플래그 활성화
            if (currentDist > mLastDistance + 0.5f)
            {
                bIsDashConditionMet = true;
                Utils.Log("대쉬 공격 조건 충족");
            }
            mLastDistance = currentDist;
        }
    }

    private void ResetPatternTimer()
    {
        mPatternTimer = 0f;
        mNextPatternTime = Random.Range(2.0f, 4.0f);
        bIsDashConditionMet = false;
    }

    private void BuildBT()
    {
        // 1. 대쉬 공격 시퀀스
        // [조건] -> [대쉬 수행] -> [성공 시 1초 대기]
        // DashAttackNode가 '피격 성공' 시 Success를 반환하면 WaitNode가 실행됨
        // '피격 실패' 시 Failure를 반환하면 Sequence가 즉시 종료되어 WaitNode를 건너뜀
        SequenceNode dashSequence = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => bIsDashConditionMet),
            new DashAttackNode(mDragon, mDragon.Board),
            new WaitNode(mDragon, mDragon.Board), // 보드에 CurrentWaitTime = 1.0f 설정 필요
            new ConditionNode(() => { ResetPatternTimer(); return true; }) // 조건 노드로 타이머 리셋
        }, true); // useMemory = true (대기 중 상태 유지 필요)

        // 2. 근접 공격 시퀀스 (사거리 이내일 때)
        SequenceNode meleeSequence = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => Vector3.Distance(mDragon.transform.position, mDragon.Board.Target.position) <= mDragon.AttackRange),
            new BasicAttackNode(mDragon, mDragon.Board, 0.5f, 2.0f, 1.5f)
        });

        // 3. 최상위 셀렉터: 대쉬(준비됐을 때만) > 근접(사거리 내) > 추적(상시)
        mPhase1BT = new SelectorNode(new List<Node>
        {
            dashSequence,
            meleeSequence,
            new MoveToTargetNode(mDragon, mDragon.Board)
        });
    }

    public override void Exit()
    {
        mPhase1BT.Abort();
    }
}