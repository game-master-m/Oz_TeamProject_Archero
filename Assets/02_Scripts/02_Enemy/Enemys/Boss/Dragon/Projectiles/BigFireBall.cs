using UnityEngine;

public class BigFireBall : EnemyProjectileBase
{
    [SerializeField] private float mSplitDelay = 2.0f; // 2초 후 자동 분열
    [SerializeField] private int mSplitCount = 8;
    private bool bHasSplit = false;

    public override void Setup(float damage, float speed, Vector3 direction, EnemyBase owner)
    {
        base.Setup(damage, speed, direction, owner);
        bHasSplit = false;
    }

    protected override void Update()
    {
        base.Update();
        if (!bIsActive) return;

        // 시간이 지났을 때 자동으로 분열 실행
        if (!bHasSplit && mLifeTimer >= mSplitDelay)
        {
            Split();
        }
    }

    private void Split()
    {
        bHasSplit = true;
        float angleStep = 360f / mSplitCount;

        for (int i = 0; i < mSplitCount; i++)
        {
            // 작은 불덩이(BossSmallFireball) 소환
            // Managers.Pool.Get<BossSmallFireball>() 형태 권장

            float angle = i * angleStep;
            Vector3 splitDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            // 작은 불덩이 세팅 (데미지 절반 등)
            // smallBall.Setup(mCurrentDamage * 0.5f, mMoveSpeed, splitDir);
        }

        ReturnPool(); // 분열 후 본체는 제거
    }
}