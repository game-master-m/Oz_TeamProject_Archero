using UnityEngine;
using UnityEngine.AI;

public class SummonStoneRainNode : ActionNode
{
    private enum ESpawnPhase { Startup, Spawning, Recovery }
    private ESpawnPhase mCurrentPhase;

    private BlackBoard mBoard;
    private float mTimer = 0f;
    private int mCurrentSpawnCount = 0;
    private bool bIsInitialized = false;

    // 설정 변수
    private float mSpawnInterval;
    private float mSpawnRadiuse;
    private int mTotalStoneCount;
    private EffectBase mStonePrefab;

    public SummonStoneRainNode(EnemyBase owner, BlackBoard board, EffectBase stonePrefab,
        float spawnInterval, float spawnRadius, int totalCount) : base(owner)
    {
        mOwner = owner;
        mBoard = board;
        mStonePrefab = stonePrefab;
        mSpawnInterval = spawnInterval;
        mSpawnRadiuse = spawnRadius;
        mTotalStoneCount = totalCount;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;

        // 1. 초기화: 애니메이션 시작
        if (!bIsInitialized)
        {
            PrepareAction();
        }

        mTimer += Time.deltaTime;

        switch (mCurrentPhase)
        {
            case ESpawnPhase.Startup:
                if (mTimer >= mOwner.AttackSpeed * 0.6f)
                {
                    mOwner.Anim.speed = 0f;
                    mCurrentPhase = ESpawnPhase.Spawning;
                    mTimer = mSpawnInterval;
                }
                break;

            case ESpawnPhase.Spawning:
                if (mCurrentSpawnCount < mTotalStoneCount)
                {
                    if (mTimer >= mSpawnInterval)
                    {
                        SpawnSingleStone();
                        mCurrentSpawnCount++;
                        mTimer = 0f;
                    }
                }
                else
                {
                    mOwner.Anim.speed = 1f;
                    mCurrentPhase = ESpawnPhase.Recovery;
                    mTimer = 0f;
                }
                break;

            case ESpawnPhase.Recovery:
                if (mTimer >= mOwner.AttackSpeed * 0.25f)
                {
                    bIsInitialized = false;
                    if (mOwner.Agent.enabled) mOwner.Agent.isStopped = false;
                    mOwner.Anim.Play(AnimHash.idle);
                    return ENodeState.Success;
                }
                break;
        }

        return ENodeState.Running;
    }

    private void PrepareAction()
    {
        mTimer = 0f;
        mCurrentSpawnCount = 0;
        mCurrentPhase = ESpawnPhase.Startup;
        bIsInitialized = true;

        if (mOwner.Agent.enabled)
        {
            mOwner.Agent.velocity = Vector3.zero;
            mOwner.Agent.isStopped = true;
        }

        // 애니메이션 재생
        mOwner.Anim.CrossFade(AnimHash.attack, 0.1f);
        mOwner.Anim.speed = 1f;
    }

    private void SpawnSingleStone()
    {
        Vector3 randomPos = GetRandomNavMeshPoint(mOwner.transform.position, mSpawnRadiuse);
        randomPos.y = 1.0f;
        if (randomPos != Vector3.zero)
        {
            EffectBase effect = Managers.Pool.GetFromPool(mStonePrefab);
            effect?.Setup(randomPos, Quaternion.identity, mOwner.AttackDamage);
        }
    }

    private Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomPos = center + new Vector3(randomCircle.x, 0, randomCircle.y);
        NavMeshHit hit;
        return NavMesh.SamplePosition(randomPos, out hit, 5.0f, NavMesh.AllAreas) ? hit.position : Vector3.zero;
    }

    public override void Abort()
    {
        base.Abort();
        mOwner.Anim.speed = 1f;
        if (mOwner.Agent.enabled) mOwner.Agent.isStopped = false;
        mOwner.Anim.Play(AnimHash.idle);
        bIsInitialized = false;
    }
}