using UnityEngine;

public class RotateToTargetNode : ActionNode
{
    private BlackBoard mBoard;
    private float mAngleThreshold;
    private float mRotateSpeed;

    private bool bIsFirstFrame = false;
    public RotateToTargetNode(EnemyBase owner, BlackBoard board, float rotateSpeed, float threshold = 10.0f) : base(owner)
    {
        mBoard = board;
        mAngleThreshold = threshold;
        mRotateSpeed = rotateSpeed;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;

        if (!bIsFirstFrame)
        {
            mOwner.Agent.velocity = Vector3.zero;
            mOwner.Agent.isStopped = true;
        }

        // 타겟 방향 계산 (Y축 무시)
        Vector3 targetDir = (mBoard.Target.position - mOwner.transform.position).normalized;
        targetDir.y = 0;

        if (targetDir == Vector3.zero) return ENodeState.Success;

        mOwner.LookAtDiretion(targetDir, mRotateSpeed);

        // 정면과의 각도 차이가 임계값 이내면 성공
        float angle = Vector3.Angle(mOwner.transform.forward, targetDir);
        if (angle <= mAngleThreshold)
        {
            bIsFirstFrame = false;
            mOwner.Agent.isStopped = false;
            return ENodeState.Success;
        }

        return ENodeState.Running;
    }
    public override void Abort()
    {
        base.Abort();
        mOwner.Agent.isStopped = false;
        bIsFirstFrame = false;
    }
}