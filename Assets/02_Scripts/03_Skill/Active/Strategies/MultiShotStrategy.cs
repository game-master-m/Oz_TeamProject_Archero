using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotStrategy : IProjectileStrategy
{
    private int mShotCount;
    private float mShotAngle = 10.0f;
    private float mDamageMultiplier;

    public MultiShotStrategy(int shotCount, float damageMultiplier)
    {
        mShotCount = shotCount;
        mDamageMultiplier = damageMultiplier;
    }

    public void OnHit(Projectile projectile, IDamageable target)
    {
    }

    public void OnShoot(Projectile projectile)
    {
        projectile.MultipleDamage(mDamageMultiplier);
        if (mShotCount % 2 == 0)
        {
            for (int i = -(mShotCount) / 2; i <= (mShotCount) / 2; i++)
            {
                if (i != 0)
                {
                    SpawnSubArrow(projectile, i);
                }
                continue;
            }
        }
        else
        {
            for (int i = -(mShotCount - 1) / 2; i <= (mShotCount - 1) / 2; i++)
            {
                if (i != 0)
                {
                    SpawnSubArrow(projectile, i);
                }
                continue;
            }
        }
    }

    private void SpawnSubArrow(Projectile projectile, int i)
    {
        Projectile subArrow = Managers.Pool.GetFromPool(projectile);
        subArrow.gameObject.transform.position = projectile.transform.position;
        subArrow.gameObject.transform.rotation = projectile.transform.rotation;
        subArrow.gameObject.transform.Rotate(0.0f, i * mShotAngle, 0.0f);
        projectile.StartCoroutine(CopyAfterOneFrame(subArrow, projectile));
    }

    //코루틴으로 한 프레임 대기(원본 OnShoot완료) 후 복사
    private IEnumerator CopyAfterOneFrame(Projectile receiver, Projectile giver)
    {
        //원본 프로젝타일 OnShoot 실행 대기
        yield return null;
        receiver.CopyWithOutOnShoot(giver);
    }



}
