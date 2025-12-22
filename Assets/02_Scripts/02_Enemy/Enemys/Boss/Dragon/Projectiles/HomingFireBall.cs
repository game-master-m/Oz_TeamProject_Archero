using UnityEngine;

public class HomingFireBall : EnemyProjectileBase
{
    [SerializeField] private float mHomingTurnSpeed = 2.5f;
    private Transform mTarget;

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
            Vector3 targetDir = (mTarget.position + Vector3.up * 0.5f - transform.position).normalized;

            // 부드럽게 타겟 쪽으로 회전 (Slerp)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(targetDir),
                mHomingTurnSpeed * Time.fixedDeltaTime
            );
        }

        // 부모의 기본적인 velocity 설정 실행
        mRigid.velocity = transform.forward * mMoveSpeed;
    }
}