using UnityEngine;

public class SplitBall : EnemyProjectileBase
{
    [Header("추가로 생성할 투사체")]
    [SerializeField] private EnemyProjectileBase mBabyProjectilePrefab;
    [SerializeField] private int mProjectileCount;
    [SerializeField] private float mSplitDistance = 8f;

    private Vector3 mLatestPos;
    private float mMoveDist;
    private bool bIsSpawned = false;

    public override void Setup(float damage, float speed, Vector3 direction, EnemyBase owner)
    {
        base.Setup(damage, speed, direction, owner);

        Managers.Pool.CreatePool(mBabyProjectilePrefab, 80, Managers.Pool.transform);
        mLatestPos = transform.position;
        mMoveDist = 0f;
        bIsSpawned = false;
    }

    protected override void Update()
    {
        base.Update();

        float moveDist = Vector3.Distance(transform.position, mLatestPos);
        mMoveDist += moveDist;
        mLatestPos = transform.position;

        if (!bIsSpawned && mMoveDist >= mSplitDistance) 
        {
            SpawnBaby(mProjectileCount);
            bIsSpawned=true;
            ReturnPool();
        }
    }

    private void SpawnBaby(int count = 4) 
    {
        Vector3 center = transform.position;
        float angleStep = 360f/count;

        for (int i = 0; i < count; i++) 
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 1.1f;
            EnemyProjectileBase babyProjectile = Managers.Pool.GetFromPool(mBabyProjectilePrefab);
            babyProjectile.transform.position = center;
            babyProjectile.Setup(mCurrentDamage * count * 0.5f, mMoveSpeed * 0.5f, offset, mOwner);
        }
    }
}
