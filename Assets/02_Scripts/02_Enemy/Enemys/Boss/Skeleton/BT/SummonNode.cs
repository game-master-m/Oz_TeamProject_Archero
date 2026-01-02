using UnityEngine;
using UnityEngine.AI;

public class SummonNode : ActionNode
{
    private EnemyBase mSummonPrefab;
    private float mSummonPosForward;

    private float mSummonTiming;

    private readonly int mSummonCount = 3;
    private readonly float mSpawnAngle = 40.0f;

    private bool bIsFirstFrame = false;
    private bool bIsSummonStart = false;
    public SummonNode(EnemyBase owner, float summonPosForward, float summonTiming) : base(owner)
    {
        mOwner = owner;
        mSummonPrefab = owner.Board.SummonPrefab;
        mSummonPosForward = summonPosForward;

        mSummonTiming = summonTiming;
    }

    public override ENodeState Evaluate()
    {
        if (!bIsFirstFrame)
        {
            bIsFirstFrame = true;
            mOwner.Anim.CrossFade(AnimHash.attack, 0.1f);
            return ENodeState.Running;
        }

        var stateInfo = mOwner.Anim.GetCurrentAnimatorStateInfo(0);
        bool isAttackState = stateInfo.shortNameHash == AnimHash.attack;
        if (!bIsSummonStart && stateInfo.shortNameHash == AnimHash.attack && stateInfo.normalizedTime >= mSummonTiming)
        {
            bIsSummonStart = true;
            //소환하고 끝이 아니고 조금만 기다렸다가 -> WaitNode로 넘어가야 함
            for (int i = 0; i < mSummonCount; i++)
            {
                EnemyBase enemy = Managers.Pool.GetFromPool(mSummonPrefab);
                enemy.Anim.Rebind();
                enemy.Anim.Update(0.0f);
                Physics.SyncTransforms(); // 물리 갱신

                //스폰포스 정하고(내 앞 셔먼포서포워드 기준 방사형 40도 3마리)
                Vector3 spawnOrigin = mOwner.transform.position;
                Vector3 spawnPos = spawnOrigin + (Quaternion.Euler(0.0f, -mSpawnAngle + (mSpawnAngle * i), 0.0f) * mOwner.transform.forward * mSummonPosForward);

                enemy.transform.position = spawnPos;

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                agent.enabled = true;
                agent.Warp(spawnPos);
                agent.isStopped = false;

                enemy.SetTarget(mOwner.Target);
            }
            return ENodeState.Running;
        }

        if (bIsSummonStart)
        {
            if (stateInfo.normalizedTime >= mSummonTiming * 2.0f)
            {
                bIsFirstFrame = false;
                bIsSummonStart = false;
                Reset();
                return ENodeState.Success;
            }
        }

        return ENodeState.Running;
    }

    public override void Abort()
    {
        base.Abort();
        Reset();
    }

    private void Reset()
    {
        mOwner.Anim.Play(AnimHash.idle);
        bIsFirstFrame = false;
        bIsSummonStart = false;
    }
}
