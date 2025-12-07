using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetStrategy : IProjectileStrategy
{
    private int mMaxBounceCount;
    private float mBounceRange; //리코쳇 범위
    private float mDamageMultiplier;

    public RicochetStrategy(int maxBounceCount, float bounceRange, float damageMultiplier)
    {
        mMaxBounceCount = maxBounceCount;
        mBounceRange = bounceRange;
        mDamageMultiplier = damageMultiplier;
    }
    public void OnShoot(Projectile projectile)
    {
        projectile.RemainingBounceCount += mMaxBounceCount;
    }
    public void OnHit(Projectile projectile, IDamageable target)
    {
        //데미지 처리?
        float finalDamage = projectile.CurrentDamage;
        projectile.MultipleDamage(mDamageMultiplier);

        //리코쳇 가능 여부 판단
        if (projectile.RemainingBounceCount <= 0)
        {
            projectile.bShouldReturnPool = true;
            return;
        }
        else
        {
            projectile.bShouldReturnPool = false;
            projectile.RemainingBounceCount--;
        }


        //가장 가까운 타겟 찾기
        projectile.LookTarget(mBounceRange);
    }
}
