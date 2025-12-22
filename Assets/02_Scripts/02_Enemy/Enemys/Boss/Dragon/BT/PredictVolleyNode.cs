using UnityEngine;

public class PredictVolleyNode : ActionNode
{
    private BlackBoard mBoard;
    private int mFireCount = 0;
    private float mTimer = 0f;
    private Vector3 mSpawnOffset = Vector3.zero;
    private bool bIsInitialized = false;

    private readonly int mMaxShots;
    private readonly float mFireInterval;
    private readonly float mLeadTime = 1.8f; // 예측 가중치
    private readonly float mMoveSpeed;

    public PredictVolleyNode(EnemyBase owner, BlackBoard board, int maxShots, float moveSpeed, float fireInterval, Vector3 spawnOffset) : base(owner)
    {
        mBoard = board;
        mMaxShots = maxShots;
        mSpawnOffset = spawnOffset;
        mMoveSpeed = moveSpeed;
        mFireInterval = fireInterval;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;

        if (!bIsInitialized)
        {
            mBoard.LastKnownPos = mOwner.Target.position;
            mFireCount = 0;
            mTimer = 0f;
            bIsInitialized = true;
        }

        mTimer += Time.deltaTime;
        if (mTimer >= mFireInterval && mFireCount < mMaxShots)
        {
            FirePredictiveShot();
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

    private void FirePredictiveShot()
    {
        // [예측 로직] 타겟의 현재 위치 + (이동 속도 * 가중치)
        Vector3 playerVelocity = (mBoard.Target.position - mBoard.LastKnownPos) / mFireInterval;
        Vector3 predictPos = mBoard.Target.position + (playerVelocity * mLeadTime * mFireInterval);
        mBoard.LastKnownPos = mBoard.Target.position; // 다음 프레임 속도 계산용

        Vector3 fireDir = (predictPos - (mOwner.transform.position + mSpawnOffset)).normalized;
        fireDir.y = 0f;

        SmallFireBall smallFireBall = Managers.Pool.GetFromPool(mBoard.SmallFireBallPrefab);
        smallFireBall.transform.position = mOwner.transform.position + mOwner.transform.TransformDirection(mSpawnOffset);
        smallFireBall.Setup(mOwner.AttackDamage * 0.5f, mMoveSpeed, fireDir, mOwner);
    }
}