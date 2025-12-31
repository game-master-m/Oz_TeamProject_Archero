using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatRandomDashNode : BatDashNode
{
    private float mRandomRadius;

    public BatRandomDashNode(EnemyBase owner, BlackBoard board, float chargeTime, float targetFixTime, float moveSpeedMultiplier, float animSpeedRate, float colliderRadius, float randomRadius)
        : base(owner, board, chargeTime, targetFixTime, moveSpeedMultiplier, animSpeedRate, colliderRadius) 
    {
        mRandomRadius = randomRadius;
    }

    protected override void PrepareDash() 
    {
        Vector3 randomDir = Random.insideUnitSphere * mRandomRadius;
        randomDir.y = 0;
        randomDir += mOwner.transform.position;

        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, mRandomRadius, NavMesh.AllAreas))
        {
            mTargetPos = hit.position;
        }
        else 
        {
            mTargetPos = mOwner.transform.position;
        }

        mOwner.Agent.isStopped = false;
        mOwner.Agent.SetDestination(mTargetPos);
    }
}
