using UnityEngine;

public class CAttackNode : ActionNode
{
    // 판정 설정
    private float mHitTiming;    // 판정 시점 (0~1 사이)
    private float mOffset;       // 정면 거리
    private float mRadius;       // 판정 반경

    private bool bIsAttacking = false;
    private bool bHitProcessed = false;
    private float mAttackEndTime = 0f;

    private readonly Collider[] mHitResults = new Collider[1];

    //DrawLine 용
    private Vector3 mDebugHitCenter;
    private float mDebugDisplayTimer = 0f;
    public CAttackNode(EnemyBase owner, float hitTiming, float offset, float radius) : base(owner)
    {
        mHitTiming = hitTiming;
        mOffset = offset;
        mRadius = radius;
    }

    public override ENodeState Evaluate()
    {
        if (!bIsAttacking)
        {
            StartAttack();
            Utils.Log("공격시작");
            return ENodeState.Running;
        }

        var stateInfo = mOwner.Anim.GetCurrentAnimatorStateInfo(0);
        bool isAttackState = stateInfo.shortNameHash == AnimHash.attack;
        // [핵심 로직] 판정 시점에 도달했고 아직 처리 전일 때
        if (!bHitProcessed && stateInfo.shortNameHash == AnimHash.attack && stateInfo.normalizedTime >= mHitTiming)
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

        //히트박스 시각화(디버그용)
        if (mDebugDisplayTimer > 0f)
        {
            DrawDebugWireSphere(mDebugHitCenter, mRadius, Color.red);
            mDebugDisplayTimer -= Time.deltaTime;
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
        mOwner.Anim.Play(AnimHash.attack);
        mOwner.Agent.velocity = Vector3.zero;
        mOwner.Agent.isStopped = true;
    }

    private void ResetAttack()
    {
        bIsAttacking = false;
        bHitProcessed = false;
    }

    private void PerformHitCheck()
    {
        Vector3 hitCenter = mOwner.transform.position + (mOwner.transform.forward * mOffset);

        //DrawLine 용
        mDebugHitCenter = hitCenter;
        mDebugDisplayTimer = 0.5f;

        int layerMask = Layers.GetLayerMask(ELayerName.Player);

        int hitCount = Physics.OverlapSphereNonAlloc(hitCenter, mRadius, mHitResults, layerMask);
        if (hitCount > 0)
        {
            if (mHitResults[0].TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(mOwner.AttackDamage);
            }
            mHitResults[0] = null;
        }
    }

    // [헬퍼] 씬 뷰에서 원형 판정을 그려주는 메서드
    private void DrawDebugWireSphere(Vector3 center, float radius, Color color)
    {
        // 360도를 8등분하여 최소한의 선으로 구체 형태 시각화
        float angleStep = 45f;
        for (float i = 0; i < 360; i += angleStep)
        {
            float r1 = i * Mathf.Deg2Rad;
            float r2 = (i + angleStep) * Mathf.Deg2Rad;

            Vector3 p1 = center + new Vector3(Mathf.Cos(r1) * radius, 0, Mathf.Sin(r1) * radius);
            Vector3 p2 = center + new Vector3(Mathf.Cos(r2) * radius, 0, Mathf.Sin(r2) * radius);
            Debug.DrawLine(p1, p2, color); // XZ 평면

            Vector3 p3 = center + new Vector3(0, Mathf.Cos(r1) * radius, Mathf.Sin(r1) * radius);
            Vector3 p4 = center + new Vector3(0, Mathf.Cos(r2) * radius, Mathf.Sin(r2) * radius);
            Debug.DrawLine(p3, p4, color); // YZ 평면
        }
    }

    public override void Abort()
    {
        ResetAttack();
        base.Abort();
    }
}