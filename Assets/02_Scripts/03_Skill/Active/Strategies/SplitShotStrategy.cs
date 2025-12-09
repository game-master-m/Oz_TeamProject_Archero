using System.Collections;
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
                if (i == 0) continue;
                SpawnSubArrow(projectile, i, targetID);
            }
        }
        else
        {
            for (int i = -(mSplitCount - 1) / 2; i <= (mSplitCount - 1) / 2; i++)
            {
                SpawnSubArrow(projectile, i, targetID);
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

        //한번 분리 후 Split전략 제거, 안 해도 되지만 몬스터가 많으면 무한증식 가능,
        //추후 이펙트 입히고 뺄지 말지 다시 고려
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
