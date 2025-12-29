using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCombatState : EnemyState
{
    private Node mCombatBT;

    // 근접 공격 파라미터 (보스보다 단순)
    private readonly float mHitTiming;
    private readonly float mHitBoxOffsetForward;
    private readonly float mHitBoxRadius;
    private readonly float mRotateToTargetSpeed = 15.0f;
    private readonly float mWaitTimeFromAnimaionEnd = 0.5f;

    public BatCombatState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
        mHitBoxOffsetForward = enemy.AttackRange;
        mHitBoxRadius = enemy.AttackRange / 2.0f;
        mHitTiming = enemy.AttackSpeed;
        mCombatBT = new RepeaterNode(BT_Builder.GetChaseAndAttackBT
            (
                enemy, enemy.Board, mRotateToTargetSpeed, mHitTiming,
                mHitBoxOffsetForward, mHitBoxRadius, mWaitTimeFromAnimaionEnd
            ));
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (mEnemy.Target == null) return;

        // BT 실행
        mCombatBT.Evaluate();
    }

    public override void Exit()
    {
        base.Exit();
        // BT 강제 중단
        mCombatBT.Abort();
    }
}
