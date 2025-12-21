using UnityEngine;

public class RangeAttackState : EnemyState
{
    private float mAttackTimer;

    public RangeAttackState(EnemyBase enemy) : base(enemy) { }

    public override void Enter()
    {
        mAttackTimer = 0f;
        mEnemy.Agent.isStopped = true;
    }

    public override void Update()
    {
        if (mEnemy.Target == null) return;

        RotateToTarget();

        mAttackTimer += Time.deltaTime;
        if (mAttackTimer >= mEnemy.AttackSpeed)
        {
            PerformAttack();
            mAttackTimer = 0f;
        }
    }

    private void RotateToTarget()
    {
        Vector3 dir = mEnemy.Target.position - mEnemy.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot =
            Quaternion.LookRotation(dir) * mEnemy.CorrectionQtrn;

        mEnemy.transform.rotation = Quaternion.RotateTowards(
            mEnemy.transform.rotation,
            targetRot,
            mEnemy.RotateSpeed * Time.deltaTime);
    }

    private void PerformAttack()
    {
        mEnemy.Anim.SetTrigger("Attack");

        // 즉시 발사 (또는 Animation Event로 분리 가능)
        ((RangeEnemy)mEnemy).FireProjectile();
    }
}
