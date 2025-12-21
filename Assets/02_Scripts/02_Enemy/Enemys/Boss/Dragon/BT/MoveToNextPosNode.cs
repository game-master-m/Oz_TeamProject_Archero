using UnityEngine;

public class MoveToNextPosNode : ActionNode
{
    private BlackBoard board;

    public MoveToNextPosNode(EnemyBase owner, BlackBoard board) : base(owner)
    {
        this.board = board;
    }

    public override ENodeState Evaluate()
    {
        var agent = mOwner.Agent;
        // 에이전트가 활성화되어 있고 NavMesh 위에 있는지 반드시 체크
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            return ENodeState.Running;
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
            agent.isStopped = true;
            return ENodeState.Success;
        }

        mOwner.LookAtDiretion(mOwner.Agent.velocity);
        return ENodeState.Running;
    }
}