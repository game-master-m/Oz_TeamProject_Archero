using UnityEngine;
using UnityEngine.AI;

public class CMoveToNextPosNode : ActionNode
{
    private NavMeshAgent mAgent;
    private BlackBoard board;
    private float mTimer = 0.0f;
    private bool bIsFirstFrame = true;
    public CMoveToNextPosNode(EnemyBase owner, BlackBoard board) : base(owner)
    {
        this.board = board;
        mAgent = owner.Agent;
    }

    public override ENodeState Evaluate()
    {
        var agent = mOwner.Agent;
        // 에이전트가 활성화되어 있고 NavMesh 위에 있는지 반드시 체크
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            return ENodeState.Running;
        }

        if (bIsFirstFrame)
        {
            bIsFirstFrame = false;
            mOwner.Anim.CrossFade(AnimHash.move, 0.1f);
        }

        //경로를 계산중이면 대기
        if (agent.pathPending) return ENodeState.Running;

        // 보드에 저장된 목적지로 이동 설정
        if (Vector3.SqrMagnitude(agent.destination - board.LastKnownPos) > 0.01f)
        {
            agent.isStopped = false;
            agent.SetDestination(board.LastKnownPos);

            return ENodeState.Running;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            ResetTimeAndStop();
            return ENodeState.Success;
        }

        //무한 러닝 방지
        mTimer += Time.deltaTime;
        if (mTimer > 5.0f)
        {
            ResetTimeAndStop();
            return ENodeState.Success;
        }

        mOwner.LookAtDiretion(mOwner.Agent.velocity);
        return ENodeState.Running;
    }
    public override void Abort()
    {
        base.Abort();
        mTimer = 0.0f;
    }
    private void ResetTimeAndStop()
    {
        mTimer = 0.0f;
        bIsFirstFrame = true;
        mAgent.isStopped = true;
    }
}