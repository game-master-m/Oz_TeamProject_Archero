using UnityEngine;

public class SummonFireTrailNode : ActionNode
{
    private BlackBoard mBoard;
    private float mTimer = 0f;
    private Vector3 mSpawnOffset = Vector3.zero;
    private bool bIsInitialized = false;

    private float mSpawnDelay;
    public SummonFireTrailNode(EnemyBase owner, BlackBoard board, float spawnDelay, Vector3 spawnOffset) : base(owner)
    {
        mBoard = board;
        mSpawnOffset = spawnOffset;
        mSpawnDelay = spawnDelay;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;

        if (!bIsInitialized)
        {
            mTimer = 0f;
            bIsInitialized = true;
            mBoard.LastKnownPos = mOwner.Target.position;
            SummonFireTrail();
        }

        mTimer += Time.deltaTime;
        if (mTimer >= mSpawnDelay)
        {
            mTimer = 0f;
            bIsInitialized = false;
            return ENodeState.Success;
        }

        return ENodeState.Running;
    }
    private void SummonFireTrail()
    {
        EffectBase prefab = Managers.Pool.GetFromPool(mBoard.FireTrailPrefab);
        mBoard.CurrentEffect = prefab;
        Vector3 spawnPos = mOwner.transform.position + mOwner.transform.TransformDirection(mSpawnOffset);
        prefab.Setup(spawnPos, Quaternion.identity);
    }

    public override void Abort()
    {
        base.Abort();
        mBoard.CurrentEffect.ExecuteEffect();
    }
}
