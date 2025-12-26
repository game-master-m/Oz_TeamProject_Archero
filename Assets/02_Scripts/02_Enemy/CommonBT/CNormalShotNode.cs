using System;
using UnityEngine;

public class CNormalShotNode : ActionNode
{
    private BlackBoard mBoard;
    private Vector3 mSpawnOffset = Vector3.up;
    private float mDamageMultiplier;
    private Func<EnemyProjectileBase> mProjectileFactory;

    private bool bHitProcessed = false;
    private bool bIsInitialized = false;
    private float mAttackEndTime = 0f;

    private readonly float mBeforeDelay;
    private readonly float mMoveSpeed;

    public CNormalShotNode(EnemyBase owner, BlackBoard board, float moveSpeed, float beforeDelay, float damageMultiplier, Vector3 offset, Func<EnemyProjectileBase> factory) : base(owner)
    {
        mOwner = owner;
        mBoard = board;
        mMoveSpeed = moveSpeed;
        mBeforeDelay = beforeDelay;
        mDamageMultiplier = damageMultiplier;
        mSpawnOffset = offset;
        mProjectileFactory = factory;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;

        if (!bIsInitialized)
        {
            bIsInitialized = true;
            mOwner.Anim.CrossFade(AnimHash.attack, 0.1f);
            mBoard.LastKnownPos = mOwner.Target.position;
        }

        var stateInfo = mOwner.Anim.GetCurrentAnimatorStateInfo(0);
        bool isAttackState = stateInfo.shortNameHash == AnimHash.attack;
        // [핵심 로직] 판정 시점에 도달했고 아직 처리 전일 때
        if (!bHitProcessed && stateInfo.shortNameHash == AnimHash.attack && stateInfo.normalizedTime >= mBeforeDelay)
        {
            // 1. 판정 직전에 남은 후딜레이 시간 계산
            // (전체 길이 * 남은 비율(1 - 현재진행도)) / 재생 속도
            float remainingTime = (stateInfo.length * (1f - stateInfo.normalizedTime)) / stateInfo.speed;
            mAttackEndTime = Time.time + remainingTime;
            bHitProcessed = true;

            NormalShot();
        }
        else
        {
            mOwner.LookAtDiretion(mBoard.Target.position - mOwner.transform.position);
        }

        if (bHitProcessed && Time.time >= mAttackEndTime)
        {
            ResetAttack();
            return ENodeState.Success;
        }

        return ENodeState.Running;
    }
    public override void Abort()
    {
        base.Abort();
        ResetAttack();
    }
    private void ResetAttack()
    {
        bIsInitialized = false;
        bHitProcessed = false;
    }
    private void NormalShot()
    {
        Vector3 fireDir = (mBoard.Target.position - mOwner.transform.position);
        fireDir.y = 0.0f;

        EnemyProjectileBase projectilePrefab = mProjectileFactory?.Invoke();
        projectilePrefab.transform.position = mOwner.transform.position + mOwner.transform.TransformDirection(mSpawnOffset);
        projectilePrefab.Setup(mOwner.AttackDamage * mDamageMultiplier, mMoveSpeed, fireDir, mOwner);
    }

}
