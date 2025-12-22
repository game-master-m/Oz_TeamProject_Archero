using System;
using UnityEngine;

public class SpreadVollyNode : ActionNode
{
    private BlackBoard mBoard;
    private int mFireCount = 0;
    private float mTimer = 0f;
    private Vector3 mSpawnOffset = Vector3.zero;
    private bool bIsInitialized = false;
    private bool bIsLeft;
    private Func<EnemyProjectileBase> mProjectileFactory;

    private readonly int mMaxShots;
    private readonly float mFireInterval;
    private readonly float mMoveSpeed;

    public SpreadVollyNode(EnemyBase owner, BlackBoard board, int maxShots, float moveSpeed, float fireInterval, Vector3 spawnOffset, Func<EnemyProjectileBase> factory) : base(owner)
    {
        mBoard = board;
        mMaxShots = maxShots;
        mSpawnOffset = spawnOffset;
        mMoveSpeed = moveSpeed;
        mFireInterval = fireInterval;
        mProjectileFactory = factory;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;


        if (!bIsInitialized)
        {
            int leftOfRight = Mathf.FloorToInt(UnityEngine.Random.Range(0, 2));
            bIsLeft = leftOfRight == 0 ? true : false;

            mFireCount = 0;
            mTimer = 0f;
            bIsInitialized = true;
            mBoard.LastKnownPos = mOwner.Target.position;
        }

        mTimer += Time.deltaTime;
        if (mTimer >= mFireInterval && mFireCount < mMaxShots)
        {
            FireSpreadShot();
            mFireCount++;
            mTimer = 0f;
        }

        if (mFireCount >= mMaxShots)
        {
            bIsInitialized = false;
            return ENodeState.Success;
        }

        return ENodeState.Running;
    }

    private void FireSpreadShot()
    {
        float deg = bIsLeft ? -mFireCount * 8.0f : mFireCount * 8.0f;
        Vector3 fireDir = Quaternion.Euler(0, deg, 0) * (mBoard.LastKnownPos - mOwner.transform.position);
        fireDir.y = 0.0f;

        EnemyProjectileBase smallFireBall = mProjectileFactory?.Invoke();
        smallFireBall.transform.position = mOwner.transform.position + mOwner.transform.TransformDirection(mSpawnOffset);
        smallFireBall.Setup(mOwner.AttackDamage * 0.5f, mMoveSpeed, fireDir, mOwner);

    }
}
