using System.Collections;
using UnityEngine;

public class MeleeMoveState : EnemyState
{
    public MeleeMoveState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
    }

    private const float updateDelay = 0.1f;
    private float nextUpdateTime = 0f;

    public override void Enter()
    {
        base.Enter();

        if (mEnemy.Agent != null && mEnemy.Agent.isOnNavMesh)
        {
            mEnemy.Agent.isStopped = false;
            mEnemy.Agent.SetDestination(mEnemy.Target.position);
        }
    }

    public override void Update()
    {
        base.Update();

        if (mEnemy.Target == null)
            return;

        // 0.1초마다 경로 업데이트 → 부하 줄이기
        if (Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateDelay;

            if (mEnemy.Agent != null && mEnemy.Agent.isOnNavMesh)
            {
                mEnemy.Agent.SetDestination(mEnemy.Target.position);
            }
        }

        // 이동 방향을 향해 자연스럽게 회전
        Vector3 moveDir = mEnemy.Agent.velocity;
        moveDir.y = 0;

        // velocity가 0이면 회전하지 않도록 체크
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(moveDir.normalized);
            //회전값 보정(곱하는 순서가 중요, Forward까지의 회전값 * 보정 회전값)
            Quaternion targetRot = lookRot * mEnemy.CorrectionQtrn;
            mEnemy.transform.rotation = Quaternion.Slerp(
                mEnemy.transform.rotation,
                targetRot,
                mEnemy.RotateSpeed * Time.deltaTime
            );
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
