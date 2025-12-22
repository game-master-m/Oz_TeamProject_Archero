using UnityEngine;
using System;

public class FanShotNode : ActionNode
{
    private BlackBoard mBoard;
    private Vector3 mSpawnOffset = Vector3.zero;
    private bool bIsInitialized = false;
    private float mTimer = 0f;
    private float mShotDuration = 1.0f;
    private Func<EnemyProjectileBase> mProjectileFactory;

    private readonly int mMaxShots;
    private readonly float mMoveSpeed;

    public FanShotNode(EnemyBase owner, BlackBoard board, int maxShots, float moveSpeed, Vector3 spawnOffset, Func<EnemyProjectileBase> factory) : base(owner)
    {
        mBoard = board;
        mMaxShots = maxShots;
        mSpawnOffset = spawnOffset;
        mMoveSpeed = moveSpeed;
        mProjectileFactory = factory;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;

        if (!bIsInitialized)
        {
            mTimer = 0f;
            bIsInitialized = true;
        }

        mTimer += Time.deltaTime;
        if (mTimer >= mShotDuration)
        {
            FireFanShot();
            mTimer = 0f;
            bIsInitialized = false;
            return ENodeState.Success;
        }

        return ENodeState.Running;
    }

    private void FireFanShot()
    {
        float totalDeg = (mMaxShots - 1) * 20.0f;
        float startDeg = -totalDeg / 2;
        Vector3 targetDir = (mBoard.Target.position - mOwner.transform.position).normalized;
        targetDir.y = 0.0f;

        Vector3 fireDir;
        for (int i = 0; i < mMaxShots; i++)
        {
            float rotationDeg = startDeg + (i * 20.0f);
            fireDir = Quaternion.Euler(0, rotationDeg, 0) * targetDir;

            EnemyProjectileBase projectilePrefab = mProjectileFactory?.Invoke();
            projectilePrefab.transform.position = mOwner.transform.position + mOwner.transform.TransformDirection(mSpawnOffset);
            projectilePrefab.Setup(mOwner.AttackDamage, mMoveSpeed, fireDir, mOwner);
        }
    }
}
