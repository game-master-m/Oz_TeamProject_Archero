using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class BasicAttackNode : ActionNode
{
    private BlackBoard mBoard;

    // 판정 설정
    private float mHitTiming;    // 판정 시점 (0~1 사이)
    private float mOffset;       // 정면 거리
    private float mRadius;       // 판정 반경

    private bool bIsAttacking = false;
    private bool bHitProcessed = false;
    private float mAttackEndTime = 0f;

    public BasicAttackNode(EnemyBase owner, BlackBoard board, float hitTiming, float offset, float radius) : base(owner)
    {
        mBoard = board;
        mHitTiming = hitTiming;
        mOffset = offset;
        mRadius = radius;
    }

    public override ENodeState Evaluate()
    {
        if (!bIsAttacking)
        {
            StartAttack();
            return ENodeState.Running;
        }

        var stateInfo = mOwner.Anim.GetCurrentAnimatorStateInfo(0);

        // [핵심 로직] 판정 시점에 도달했고 아직 처리 전일 때
        if (!bHitProcessed && stateInfo.IsName("AttackDown") && stateInfo.normalizedTime >= mHitTiming)
        {
            // 1. 판정 직전에 남은 후딜레이 시간 계산
            // (전체 길이 * 남은 비율(1 - 현재진행도)) / 재생 속도
            float remainingTime = (stateInfo.length * (1f - stateInfo.normalizedTime)) / stateInfo.speed;
            mAttackEndTime = Time.time + remainingTime;

            // 2. 판정 수행
            PerformHitCheck();

            bHitProcessed = true;
            Utils.Log($"판정 발생! 남은 {remainingTime:F2}초 동안 후딜레이에 진입합니다.");
        }

        // 3. 판정 이후 설정된 종료 시간에 도달하면 Success
        if (bHitProcessed && Time.time >= mAttackEndTime)
        {
            ResetAttack();
            return ENodeState.Success;
        }

        return ENodeState.Running;
    }

    private void StartAttack()
    {
        bIsAttacking = true;
        bHitProcessed = false;
        mAttackEndTime = 0f;
        // 마스터 플랜 가이드 준수: AnimHash 사용 권장
        mOwner.Anim.SetTrigger("AttackDown");
    }

    private void ResetAttack()
    {
        bIsAttacking = false;
        bHitProcessed = false;
    }

    private void PerformHitCheck()
    {
        Vector3 hitCenter = mOwner.transform.position + (mOwner.transform.forward * mOffset);
        int layerMask = Layers.GetLayerMask(ELayerName.Player);

        Collider[] hitColliders = Physics.OverlapSphere(hitCenter, mRadius, layerMask);
        foreach (var col in hitColliders)
        {
            if (col.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(mOwner.AttackDamage);
            }
        }
    }

    public override void Abort()
    {
        ResetAttack();
        mOwner.Anim.speed = 1.0f;
        base.Abort();
    }
}