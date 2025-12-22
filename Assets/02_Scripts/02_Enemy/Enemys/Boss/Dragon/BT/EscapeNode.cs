using UnityEngine;
public class EscapeNode : ActionNode
{
    private float mEscapeRange = 5.0f;
    public EscapeNode(EnemyBase owner) : base(owner) { }

    public override ENodeState Evaluate()
    {
        float dist = Vector3.Distance(mOwner.transform.position, mOwner.Target.position);
        if (dist > mEscapeRange) return ENodeState.Failure;

        // [텔레포트] 플레이어 반대 방향으로 NavMesh 위 안전한 좌표 찾기
        Vector3 escapeDir = (mOwner.transform.position - mOwner.Target.position).normalized;
        Vector3 targetPos = mOwner.transform.position + escapeDir * 15.0f;

        if (UnityEngine.AI.NavMesh.SamplePosition(targetPos, out var hit, 5.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
            mOwner.transform.position = hit.position;
            mOwner.Agent.Warp(hit.position); // Agent 위치 동기화 필수
            Utils.Log("보스 긴급 탈출(텔레포트)!");
            return ENodeState.Success;
        }
        return ENodeState.Failure;
    }
}