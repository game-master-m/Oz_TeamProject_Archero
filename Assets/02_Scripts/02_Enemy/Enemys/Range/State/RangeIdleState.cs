using UnityEngine;

public class RangeIdleState : EnemyState
{
    public RangeIdleState(EnemyBase enemy, IState parent = null)
        : base(enemy, parent)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Utils.Log("RangeIdle Enter");

        // 이동 완전 정지
        mEnemy.Agent.isStopped = true;
        mEnemy.Agent.velocity = Vector3.zero;

        // Idle 애니메이션
        mEnemy.Anim.CrossFade(AnimHash.idle, 0.1f);
    }

    public override void Update()
    {
        base.Update();

        // 선택 사항: Idle 상태에서도 플레이어를 바라보게
        if (mEnemy.Target == null) return;

        Vector3 dir = mEnemy.Target.position - mEnemy.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot =
            Quaternion.LookRotation(dir) * mEnemy.CorrectionQtrn;

        mEnemy.transform.rotation = Quaternion.RotateTowards(
            mEnemy.transform.rotation,
            targetRot,
            mEnemy.RotateSpeed * Time.deltaTime
        );
    }

    public override void Exit()
    {
        base.Exit();
    }
}
