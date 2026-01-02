using UnityEngine;

public class HomingFireBall : EnemyProjectileBase
{
    [SerializeField] private float mHomingTurnSpeed = 2.0f;
    [SerializeField] private float mHomingRange = 15.0f;
    private Transform mTarget;

    private Vector3 mLastFrameTargetPos;

    public override void Setup(float damage, float speed, Vector3 direction, EnemyBase owner)
    {
        base.Setup(damage, speed, direction, owner);
        mTarget = owner.Target;
    }

    protected override void MoveAndRotate()
    {
        if (mTarget != null)
        {
            // 타겟을 향한 방향 계산
            Vector3 targetPos = (mTarget.position - transform.position);
            targetPos.y = 0.0f;

            if (!((targetPos.sqrMagnitude > mLastFrameTargetPos.sqrMagnitude
                 && mLastFrameTargetPos != Vector3.zero)
                || targetPos.sqrMagnitude < 10f))
            {
                if (targetPos.sqrMagnitude < mHomingRange * mHomingRange)
                {
                    Vector3 targetDir = targetPos.normalized;
                    // 부드럽게 타겟 쪽으로 회전 (Slerp)
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(targetDir),
                        mHomingTurnSpeed * Time.fixedDeltaTime
                    );
                }
            }

            mLastFrameTargetPos = targetPos;
        }

        // 부모의 기본적인 velocity 설정 실행
        mRigid.velocity = transform.forward * mMoveSpeed;

    }
    protected override void OnHit(LivingEntity target)
    {
        target.TakeDamage(mCurrentDamage * 0.6f, EDmgElement.Fire);
        target.TakeDotDamage(mCurrentDamage * 0.06f, 5.0f, 0.5f, EDmgElement.Fire);
        ReturnPool();
    }
}