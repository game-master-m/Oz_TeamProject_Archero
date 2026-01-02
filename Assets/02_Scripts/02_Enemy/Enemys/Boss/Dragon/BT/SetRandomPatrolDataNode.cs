using UnityEngine;
using UnityEngine.AI;

public class SetRandomPatrolDataNode : ActionNode
{
    private BlackBoard mBoard;
    private float mRange;
    private float mMinWait;
    private float mMaxWait;

    //최소 이동 거리를 두어 '제자리 맴돎' 방지
    private const float MIN_MOVE_DIST = 3.0f;
    private const int MAX_RETRY = 10; // 최대 10번 시도

    public SetRandomPatrolDataNode(EnemyBase owner, BlackBoard board, float range, float minWait, float maxWait) : base(owner)
    {
        mBoard = board;
        mRange = range;
        mMinWait = minWait;
        mMaxWait = maxWait;
    }

    public override ENodeState Evaluate()
    {
        for (int i = 0; i < MAX_RETRY; i++)
        {
            // 1. 현재 위치 근처 랜덤 좌표 생성
            Vector3 randomPos = mOwner.transform.position + Random.insideUnitSphere * mRange;

            NavMeshHit hit;
            // 2. NavMesh 위인지 확인 (반경 range 이내에서 가장 가까운 곳 탐색)
            if (NavMesh.SamplePosition(randomPos, out hit, mRange, NavMesh.AllAreas))
            {
                // 3. 현재 위치와 너무 가깝다면 다시 계산 (맵 가장자리에서 제자리 걸음 방지)
                float dist = Vector3.Distance(mOwner.transform.position, hit.position);
                if (dist < MIN_MOVE_DIST) continue;

                // 유효한 좌표 발견!
                mBoard.LastKnownPos = hit.position;
                mBoard.CurrentWaitTime = Random.Range(mMinWait, mMaxWait);
                return ENodeState.Success;
            }
        }

        // 10번 시도했는데도 못 찾았다면 (맵 끝에 끼었을 가능성)
        // 안전하게 현재 위치를 목적지로 반환하거나, 에러를 반환
        return ENodeState.Failure;
    }
}