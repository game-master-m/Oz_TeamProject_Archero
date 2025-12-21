using UnityEngine;

public class DashAttackNode : ActionNode
{
    private enum EDashState { Charging, Dashing }
    private EDashState mCurrentState;

    private BlackBoard mBoard;
    private Vector3 mTargetPos;
    private Vector3 mLastPosition;
    private float mTimer;
    private float mOriginalSpeed;
    private bool bPosRecorded;

    public DashAttackNode(EnemyBase owner, BlackBoard board) : base(owner)
    {
        mBoard = board;
    }

    public override ENodeState Evaluate()
    {
        if (mBoard.Target == null) return ENodeState.Failure;

        switch (mCurrentState)
        {
            case EDashState.Charging:
                return UpdateCharging();
            case EDashState.Dashing:
                return UpdateDashing();
            default:
                return ENodeState.Failure;
        }
    }

    private ENodeState UpdateCharging()
    {
        mTimer += Time.deltaTime;

        // 0.6초 시점에 플레이어 위치 기록
        if (mTimer >= 0.6f && !bPosRecorded)
        {
            mTargetPos = mBoard.Target.position;
            bPosRecorded = true;
            Utils.Log("대쉬 대상 위치 기록 완료");
        }

        // 1.5초 기모으기 완료 후 대쉬 전환
        if (mTimer >= 1.5f)
        {
            PrepareDash();
            mCurrentState = EDashState.Dashing;
        }

        return ENodeState.Running;
    }

    private void PrepareDash()
    {
        mOriginalSpeed = mOwner.Agent.speed;
        mOwner.Agent.speed = mOriginalSpeed * 4.0f; // 속도 4배
        mOwner.Agent.isStopped = false;
        mOwner.Agent.SetDestination(mTargetPos);

        mOwner.Anim.speed = 0.3f; // 재생속도 0.3
        mOwner.Anim.SetTrigger("AttackDown");

        mLastPosition = mOwner.transform.position;
    }

    private ENodeState UpdateDashing()
    {
        // 1. 터널링 방지 판정 (이전 위치와 현재 위치 사이 스캔)
        if (CheckTunnelingCollision())
        {
            ResetDashSettings();
            return ENodeState.Success; // 피격 성공
        }

        // 2. 목적지 도달 체크
        if (!mOwner.Agent.pathPending && mOwner.Agent.remainingDistance <= mOwner.Agent.stoppingDistance + 0.1f)
        {
            ResetDashSettings();
            return ENodeState.Failure; // 피격 실패
        }

        return ENodeState.Running;
    }

    private bool CheckTunnelingCollision()
    {
        Vector3 currentPos = mOwner.transform.position;
        Vector3 direction = currentPos - mLastPosition;
        float distance = direction.magnitude;

        if (distance > 0.01f)
        {
            // SphereCast로 궤적 추적
            int layerMask = 1 << LayerMask.NameToLayer("Player"); // 혹은 Layers 헬퍼 사용
            if (Physics.SphereCast(mLastPosition, 1.5f, direction.normalized, out RaycastHit hit, distance, layerMask))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(mOwner.AttackDamage);
                    Utils.Log("대쉬 공격 적중!");
                    return true;
                }
            }
        }
        mLastPosition = currentPos;
        return false;
    }

    private void ResetDashSettings()
    {
        mOwner.Agent.speed = mOriginalSpeed;
        mOwner.Anim.speed = 1.0f;
        mTimer = 0f;
        bPosRecorded = false;
        mCurrentState = EDashState.Charging;
    }

    public override void Abort()
    {
        ResetDashSettings();
        base.Abort();
    }
}