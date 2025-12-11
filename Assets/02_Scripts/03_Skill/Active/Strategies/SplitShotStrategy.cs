using System.Collections;
using UnityEngine;

public class SplitShotStrategy : IProjectileStrategy, ISkillStackable<IProjectileStrategy>
{
    private int mSplitCount;
    private float mDamageMultiplier;
    private float mSplitAngle = 10.0f;

    public SplitShotStrategy(int splitCount, float damageMultiplier)
    {
        mSplitCount = splitCount;
        mDamageMultiplier = damageMultiplier;
    }

    //스킬선택 시, 이미 보유하고 있으면 PlayerAttack.cs의 AddOrStack에서 걸러내고 기존 전략인스턴스의 TryStack만 호출
    public bool TryStack(IProjectileStrategy newSplitShot)
    {
        if (newSplitShot is SplitShotStrategy)
        {
            //갈라짐 + 1
            mSplitCount += 1;
            //데미지 감소 없음
            return true;
        }
        return false;
    }
    public void OnShoot(Projectile projectile) { }

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
