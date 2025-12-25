using UnityEngine;

public class BigFireBall : EnemyProjectileBase
{
    [SerializeField] private float mMinSplitDelay = 1.5f; // 2초 후 자동 분열
    [SerializeField] private int mSplitCount = 12;
    [SerializeField] private SmallFireBall mSmallFireBall;
    [SerializeField] private FireExplosionAir mExplosionAir;

    private bool bHasSplit = false;
    private float mSplitDelay;
    protected override void Awake()
    {
        base.Awake();
        Managers.Pool.CreatePool(mExplosionAir, 10, Managers.Pool.transform);
    }
    public override void Setup(float damage, float speed, Vector3 direction, EnemyBase owner)
    {
        base.Setup(damage, speed, direction, owner);
        bHasSplit = false;
        mSplitDelay = mMinSplitDelay + Random.Range(0, 1.0f);
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
        FireExplosionAir effect = Managers.Pool.GetFromPool(mExplosionAir);
        effect.Setup(transform.position, transform.rotation);

        bHasSplit = true;
        float angleStep = 360f / mSplitCount;
        float randomAngle = Random.Range(0, angleStep);
        for (int i = 0; i < mSplitCount; i++)
        {
            // 작은 불덩이(SmallFireBall) 소환
            SmallFireBall smallFireBallPrefab = Managers.Pool.GetFromPool(mSmallFireBall);

            float angle = i * angleStep + randomAngle;
            Vector3 splitDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            smallFireBallPrefab.transform.position = transform.position;
            // 작은 불덩이 세팅 (데미지 절반 등)
            smallFireBallPrefab.Setup(mCurrentDamage * 0.5f, mMoveSpeed * 2.0f, splitDir, mOwner);
        }

        ReturnPool(); // 분열 후 본체는 제거
    }
}