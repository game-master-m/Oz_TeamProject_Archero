using System;
using UnityEngine;

public class NormalShotNode : ActionNode
{
    private BlackBoard mBoard;
    private float mTimer = 0f;
    private Vector3 mSpawnOffset = Vector3.zero;
    private bool bIsInitialized = false;
    private float mDamageMultiplier;
    private Func<EnemyProjectileBase> mProjectileFactory;

    private readonly float mBeforeDelay;
    private readonly float mMoveSpeed;

    public NormalShotNode(EnemyBase owner, BlackBoard board, float moveSpeed, float beforeDelay, float damageMultiplier, Vector3 offset, Func<EnemyProjectileBase> factory) : base(owner)
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
            mTimer = 0f;
            bIsInitialized = true;
            mBoard.LastKnownPos = mOwner.Target.position;
        }

        mTimer += Time.deltaTime;
        if (mTimer >= mBeforeDelay)
        {
            NormalShot();
            mTimer = 0f;
            bIsInitialized = false;
            return ENodeState.Success;
        }

        return ENodeState.Running;
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
