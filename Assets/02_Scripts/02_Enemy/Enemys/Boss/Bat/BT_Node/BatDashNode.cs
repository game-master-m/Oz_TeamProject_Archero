using UnityEngine;

public class BatDashNode : ActionNode
{
    private enum EDashState { Charging, Dashing }
    private EDashState mCurrentState;

    private BlackBoard mBoard;
    protected Vector3 mTargetPos;
    private Vector3 mLastPosition;
    private float mTimer;
    private float mOriginalSpeed;
    private bool bPosRecorded;
    private bool bIsHitProcessed;
    private bool bIsAnimStart = false;

    private float mOriginAcceleration;
    private float mOriginStoppingDist;

    private readonly float mChargeTime;
    private readonly float mTargetFixTime;
    private readonly float mMoveSpeedMultiplier;
    private readonly float mAnimSpeedRate;
    private readonly float mColliderRadius;

    private readonly RaycastHit[] mHitResults = new RaycastHit[1];

    public BatDashNode(EnemyBase owner, BlackBoard board, float chargeTime, float targetFixTime, float moveSpeedMultiplier, float animSpeedRate, float colliderRadius) : base(owner)
    {
        mBoard = board;
        mChargeTime = chargeTime;
        mTargetFixTime = targetFixTime;
        mMoveSpeedMultiplier = moveSpeedMultiplier;
        mAnimSpeedRate = animSpeedRate;
        mColliderRadius = colliderRadius;
        mCurrentState = EDashState.Charging;
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
        if (!bIsAnimStart)
        {
            mOwner.Agent.velocity = Vector3.zero;
            mOwner.Agent.isStopped = true;

            mOwner.Anim.speed = 1.0f; // 재생속도 늦춤
            mOwner.Anim.CrossFade(AnimHash.idle, 0.1f);
            bIsAnimStart = true;
        }
        mTimer += Time.deltaTime;

        // targetFixedTime 시점에 플레이어 위치 기록
        if (mTimer >= mTargetFixTime && !bPosRecorded)
        {
            mTargetPos = mBoard.Target.position;
            bPosRecorded = true;
        }
   
        mOwner.LookAtDiretion(mBoard.Target.position - mOwner.transform.position);

        // 기모으기 완료 후 대쉬 전환
        if (mTimer >= mChargeTime)
        {
            PrepareDash();
            mCurrentState = EDashState.Dashing;
        }

        return ENodeState.Running;
    }

    protected virtual void PrepareDash()
    {
        mOriginalSpeed = mOwner.Agent.speed;
        mOriginAcceleration = mOwner.Agent.acceleration;
        mOriginStoppingDist = mOwner.Agent.stoppingDistance;

        mOwner.Agent.acceleration = 500f;   //가속도 확 늘림
        mOwner.Agent.speed = mOriginalSpeed * mMoveSpeedMultiplier; // 속도 확 빠르게

        mOwner.Agent.stoppingDistance = 0.1f;
        mOwner.Agent.isStopped = false;
        mOwner.Agent.SetDestination(mTargetPos);

        mLastPosition = mOwner.transform.position;
        bIsHitProcessed = false;
    }

    private ENodeState UpdateDashing()
    {
        //1. 경계 충돌 체크
        if (mOwner.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial) 
        {
            mOwner.Agent.velocity = Vector3.zero;
            mOwner.Agent.acceleration = 0f;
            mOwner.Agent.isStopped = true;

            ResetDashSettings();
            return ENodeState.Failure;
        }

        // 2. 터널링 방지 판정 (이전 위치와 현재 위치 사이 스캔)
        if (!bIsHitProcessed)
        {
            if (CheckTunnelingCollision())
            {
                bIsHitProcessed = true;
                ResetDashSettings();
                return ENodeState.Success;
            }
        }

        // 3. 목적지 도달 체크
        if (!mOwner.Agent.pathPending && mOwner.Agent.remainingDistance <= mOwner.Agent.stoppingDistance + 0.1f)
        {
            mOwner.Agent.velocity = Vector3.zero;
            mOwner.Agent.acceleration = 0f;
            mOwner.Agent.isStopped = true;

            ResetDashSettings();
            return ENodeState.Failure;
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
            int layerMask = Layers.GetLayerMask(ELayerName.Player);
            int hitCount = Physics.SphereCastNonAlloc(mLastPosition, mColliderRadius, direction.normalized, mHitResults, distance, layerMask);
            if (hitCount > 0)
            {
                if (mHitResults[0].collider.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(mOwner.AttackDamage);
                    return true;
                }
            }
        }
        mLastPosition = currentPos;
        return false;
    }

    private void ResetDashSettings()
    {
        bIsAnimStart = false;
        mOwner.Agent.acceleration = mOriginAcceleration;
        mOwner.Agent.stoppingDistance = mOriginStoppingDist;
        mOwner.Agent.speed = mOriginalSpeed;
        mOwner.Anim.speed = 1.0f;
        mOwner.Anim.Play(AnimHash.idle);
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
