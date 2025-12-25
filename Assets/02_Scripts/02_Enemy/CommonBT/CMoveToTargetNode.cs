using UnityEngine;

public class CMoveToTargetNode : ActionNode
{
    private BlackBoard mBoard;
    private bool bIsFirstFrame = true;
    private float mSetDestinationTimer = 0.3f;
    private readonly float mSetDestinationDuration = 0.3f;

    public CMoveToTargetNode(EnemyBase owner, BlackBoard board) : base(owner)
    {
        mBoard = board;
    }

    public override ENodeState Evaluate()
    {
        // 1. 타겟 존재 여부 확인
        if (mBoard.Target == null) return ENodeState.Failure;
        if (bIsFirstFrame)
        {
            mOwner.Anim.Play(AnimHash.move);
            bIsFirstFrame = false;
        }

        float sqrDist = Vector3.SqrMagnitude(mOwner.transform.position - mBoard.Target.position);

        // 2. 공격 사거리 내에 들어왔는지 체크
        if (sqrDist <= mOwner.AttackRange * mOwner.AttackRange * 0.9f)
        {
            mOwner.Agent.isStopped = true;
            mOwner.Agent.velocity = Vector3.zero;
            bIsFirstFrame = true;
            Utils.Log("이동완료");
            return ENodeState.Success;
        }

        // 3. NavMeshAgent를 통한 이동 설정
        mSetDestinationTimer += Time.deltaTime;
        if (mSetDestinationTimer >= mSetDestinationDuration)
        {
            mSetDestinationTimer = 0.0f;
            if (mOwner.Agent.isOnNavMesh)
            {
                mOwner.Agent.isStopped = false;
                mOwner.Agent.SetDestination(mBoard.Target.position);
            }
        }

        // 4. 이동 방향을 바라보도록 회전
        mOwner.LookAtDiretion(mOwner.Agent.velocity);

        return ENodeState.Running;
    }
    public override void Abort()
    {
        base.Abort();
        bIsFirstFrame = true;
    }
}
