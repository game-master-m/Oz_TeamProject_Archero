using UnityEngine;

public class MoveToTargetNode : ActionNode
{
    private BlackBoard mBoard;

    public MoveToTargetNode(EnemyBase owner, BlackBoard board) : base(owner)
    {
        mBoard = board;
    }

    public override ENodeState Evaluate()
    {
        // 1. 타겟 존재 여부 확인
        if (mBoard.Target == null) return ENodeState.Failure;

        float dist = Vector3.Distance(mOwner.transform.position, mBoard.Target.position);

        // 2. 공격 사거리 내에 들어왔는지 체크
        if (dist <= mOwner.AttackRange)
        {
            mOwner.Agent.isStopped = true;
            return ENodeState.Success;
        }

        // 3. NavMeshAgent를 통한 이동 설정
        if (mOwner.Agent.isOnNavMesh)
        {
            mOwner.Agent.isStopped = false;
            mOwner.Agent.SetDestination(mBoard.Target.position);
        }

        // 4. 이동 방향을 바라보도록 회전
        mOwner.LookAtDiretion(mOwner.Agent.velocity);

        return ENodeState.Running;
    }
}