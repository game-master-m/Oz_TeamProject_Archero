using UnityEngine;

public class SpinAttackNode : ActionNode
{
    private EnemyAttackCol mAttackCol;

    private bool bIsAttacking = false;
    private bool bHitProcessed = false;
    private float mAttackEndTime = 0f;

    private float mHitStartTime = 0.2f;

    public SpinAttackNode(EnemyBase owner, EnemyAttackCol attackCol) : base(owner)
    {
        mAttackCol = attackCol;
    }

    public override ENodeState Evaluate()
    {
        if (!bIsAttacking)
        {
            bIsAttacking = true;
            //애니메이션 실행
            mOwner.Anim.CrossFade(AnimHash.attackSpin, 0.1f);
            return ENodeState.Running;
        }

        var stateInfo = mOwner.Anim.GetCurrentAnimatorStateInfo(0);

        if (!bHitProcessed && stateInfo.shortNameHash == AnimHash.attackSpin && stateInfo.normalizedTime >= mHitStartTime)
        {
            mAttackCol.StartAttack();
            bHitProcessed = true;

            float remainingTime = (stateInfo.length * (1f - stateInfo.normalizedTime)) / stateInfo.speed;
            mAttackEndTime = Time.time + remainingTime;
        }


        if (bHitProcessed && Time.time >= mAttackEndTime)
        {
            ResetAttack();
            return ENodeState.Success;
        }

        return ENodeState.Running;


    }
    private void ResetAttack()
    {
        bIsAttacking = false;
        bHitProcessed = false;
        mOwner.Anim.speed = 1.0f;
        mOwner.Anim.Play(AnimHash.idle);

        mAttackCol.EndAttack();
    }
    public override void Abort()
    {
        base.Abort();
        mAttackCol.EndAttack();
    }

}
