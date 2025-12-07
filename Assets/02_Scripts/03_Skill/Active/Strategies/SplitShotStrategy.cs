using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitShotStrategy : IProjectileStrategy
{
    private int mSplitCount;
    private float mDamageMultiplier;
    private float mSplitAngle = 10.0f;

    public SplitShotStrategy(int splitCount, float damageMultiplier)
    {
        mSplitCount = splitCount;
        mDamageMultiplier = damageMultiplier;
    }


    public void OnShoot(Projectile projectile)
    {

    }

    public void OnHit(Projectile projectile, IDamageable target)
    {
        int targetID = -1;
        if (target is Component component)
        {
            targetID = component.gameObject.GetInstanceID();
        }

        if (mSplitCount % 2 == 0)
        {
            for (int i = -(mSplitCount) / 2; i <= (mSplitCount) / 2; i++)
            {
                if (i != 0)
                {
                    SpawnSubArrow(projectile, i, targetID);
                }
                continue;
            }
        }
        else
        {
            for (int i = -(mSplitCount - 1) / 2; i <= (mSplitCount - 1) / 2; i++)
            {
                if (i != 0)
                {
                    SpawnSubArrow(projectile, i, targetID);
                }
                continue;
            }
        }
    }

    private void SpawnSubArrow(Projectile projectile, int i, int ignoreTargetID)
    {
        Projectile subArrow = Managers.Pool.GetFromPool(projectile);
        if (subArrow == null) return;
        if (ignoreTargetID != -1)
        {
            subArrow.AddIgnoreTarget(ignoreTargetID);
        }
        subArrow.CopyWithOutOnShoot(projectile);

        //필요 시, Split전략 제거 가능
        subArrow.RemoveStrategy<SplitShotStrategy>();

        subArrow.MultipleDamage(mDamageMultiplier);

        subArrow.StartCoroutine(PositionAfterOneFrame(subArrow, projectile, i));
    }
    private IEnumerator PositionAfterOneFrame(Projectile subArrow, Projectile projectile, int i)
    {
        yield return null;
        subArrow.gameObject.transform.position = projectile.transform.position;
        subArrow.gameObject.transform.rotation = projectile.transform.rotation;
        subArrow.gameObject.transform.Rotate(0.0f, i * mSplitAngle, 0.0f);
    }
}
