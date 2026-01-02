using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetStrategy : IProjectileStrategy, ISkillStackable<IProjectileStrategy>
{
    private int mMaxBounceCount;
    private float mBounceRange; //리코쳇 범위
    private float mDamageMultiplier;

    private bool bIsFirstHit = true;
    public RicochetStrategy(int maxBounceCount, float bounceRange, float damageMultiplier)
    {
        mMaxBounceCount = maxBounceCount;
        mBounceRange = bounceRange;
        mDamageMultiplier = damageMultiplier;
    }

    //스킬선택 시, 이미 보유하고 있으면 PlayerAttack.cs의 AddOrStack에서 걸러내고 기존 전략인스턴스의 TryStack만 호출
    public bool TryStack(IProjectileStrategy newRicochet)
    {
        if (newRicochet is RicochetStrategy)
        {
            //튕김 + 1
            mMaxBounceCount += 1;
            //데미지 감소 없음
            return true;
        }
        return false;
    }
    public void OnShoot(Projectile projectile)
    {
        projectile.RemainingBounceCount += mMaxBounceCount;
        bIsFirstHit = true;
    }
    public void OnHit(Projectile projectile, IDamageable target)
    {
        //데미지 감소(첫 타격 제외)
        if (!bIsFirstHit) projectile.MultipleDamage(mDamageMultiplier);
        bIsFirstHit = false;

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
