using UnityEngine;

public class StoneRainning : EffectBase
{
    [SerializeField] private float mHitCircleRadius = 1.0f;
    [SerializeField] private float mHitCoolTime = 0.5f;

    private Collider[] mHitBuffer = new Collider[1];
    private float mHitCoolTimer = 0.0f;
    private float mDamage;
    private Vector3 mSpawnPos;

    public override void Setup(Vector3 spawnPos, Quaternion rotation, float damage)
    {
        base.Setup(spawnPos, rotation);
        mSpawnPos = spawnPos;
        mHitCoolTimer = 0.0f;
        mDamage = damage;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        mHitCoolTimer += Time.fixedDeltaTime;

        if (mHitCoolTimer >= mHitCoolTime)
        {
            PerformHit();
            mHitCoolTimer = 0f;
        }

    }
    private void PerformHit()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(mSpawnPos, mHitCircleRadius, mHitBuffer, Layers.GetLayerMask(ELayerName.Player));

        if (numColliders > 0)
        {
            if (mHitBuffer[0].TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(mDamage);
            }
            mHitBuffer[0] = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mSpawnPos, mHitCircleRadius);
    }
}
