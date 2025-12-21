using UnityEngine;

public class RangeMoveState : EnemyState
{
    public RangeMoveState(EnemyBase enemy, IState parent = null)
        : base(enemy, parent)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (mEnemy.Target == null)
            return;

        // 이동 재개
        mEnemy.Agent.isStopped = false;

        // 공격 범위 근처에서 멈추게 (자연스러운 감속)
        mEnemy.Agent.stoppingDistance = mEnemy.AttackRange * 0.9f;

        mEnemy.Agent.SetDestination(mEnemy.Target.position);

        // 이동 애니메이션
        mEnemy.Anim.CrossFade(AnimHash.move, 0.1f);
    }

    public override void Update()
    {
        base.Update();

        if (mEnemy.Target == null)
            return;

        // 플레이어 실시간 추적
        mEnemy.Agent.SetDestination(mEnemy.Target.position);
    }

    public override void Exit()
    {
        base.Exit();
        // 필요 시 여기서 정지 가능
        // mEnemy.Agent.isStopped = true;
    }
}
