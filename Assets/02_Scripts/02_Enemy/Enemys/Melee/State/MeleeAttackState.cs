using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : EnemyState
{
    private Node mMeleeBT;

    // 근접 공격 파라미터 (보스보다 단순)
    private readonly float mHitTiming = 0.25f;
    private readonly float mHitBoxOffsetForward = 1.5f;
    private readonly float mHitBoxRadius = 1.2f;

    public MeleeAttackState(MeleeEnemy enemy, IState parent = null)
        : base(enemy, parent)
    {
        BuildBT();
    }

    public override void Enter()
    {
        base.Enter();

        // 이동 완전 정지
        if (mEnemy.Agent != null && mEnemy.Agent.isOnNavMesh)
        {
            mEnemy.Agent.isStopped = true;
            mEnemy.Agent.velocity = Vector3.zero;
        }

        // 공격 애니메이션
        mEnemy.Anim.CrossFade(AnimHash.attack, 0.1f);
    }

    public override void Update()
    {
        base.Update();

        // 타겟이 없으면 즉시 종료 (FSM 전이 조건에 의해 Move/Idle로 이동)
        if (mEnemy.Target == null)
            return;

        // BT 실행
        mMeleeBT.Evaluate();
    }

    public override void Exit()
    {
        base.Exit();

        // BT 강제 중단
        mMeleeBT.Abort();

        // 이동 재개
        if (mEnemy.Agent != null && mEnemy.Agent.isOnNavMesh)
        {
            mEnemy.Agent.isStopped = false;
        }
    }

    private void BuildBT()
    {
        // 보스 드레곤의 근접공격 시퀀스
        // 근접 공격 시퀀스 
        // [사거리 체크] -> [타겟 회전] → [공격 판정]
        SequenceNode meleeSequence = new SequenceNode(new List<Node>
        {
            new ConditionNode(() =>
                mEnemy.Target != null &&
                Vector3.Distance(mEnemy.transform.position, mEnemy.Target.position) <= mEnemy.AttackRange
            ),

            new RotateToTargetNode(
                mEnemy,
                mEnemy.Board,
                15.0f
            ),

            new BasicAttackNode(
                mEnemy,
                mEnemy.Board,
                mHitTiming,
                mHitBoxOffsetForward,
                mHitBoxRadius
            )
        }, true); 
        // useMemory = true → 공격 도중 상태 유지

        //최상위 셀렉터
        // 1. 근접 공격 시퀀스
        // 2. 실패 시 짧은 대기
        mMeleeBT = new SelectorNode(new List<Node>
        {
            meleeSequence,
            new WaitNode(mEnemy, 0.1f)
        });
    }
}
