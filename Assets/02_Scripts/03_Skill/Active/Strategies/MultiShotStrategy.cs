using UnityEngine;

public class MultiShotStrategy : IProjectileStrategy, ISkillStackable<IProjectileStrategy>
{
    private int mAddtionalNum = 1;
    public void OnHit(Projectile projectile, IDamageable target) { }

    public void OnShoot(Projectile projectile)
    {

    }

    public bool TryStack(IProjectileStrategy strategy)
    {
        if (strategy is MultiShotStrategy multiShotStrategy)
        {
            //복사할 개체 수 증가
            mAddtionalNum++;
            return true;
        }
        return false;
    }

}
