using System.Collections;

public class FanShotStrategy : IProjectileStrategy, ISkillStackable<IProjectileStrategy>
{
    private int mShotCount;
    private float mShotAngle = 10.0f;
    private float mDamageMultiplier;

    public FanShotStrategy(int shotCount, float damageMultiplier)
    {
        mShotCount = shotCount;
        mDamageMultiplier = damageMultiplier;
    }

    public void OnHit(Projectile projectile, IDamageable target) { }

    //스킬선택 시, 이미 보유하고 있으면 PlayerAttack.cs의 AddOrStack에서 걸러내고 기존 전략인스턴스의 TryStack만 호출
    public bool TryStack(IProjectileStrategy newFanShot)
    {
        if (newFanShot is FanShotStrategy)
        {
            mShotCount += 2;
            mDamageMultiplier *= mDamageMultiplier;
            return true;
        }
        return false;
    }
    public void OnShoot(Projectile projectile)
    {
        projectile.MultipleDamage(mDamageMultiplier);

        if (mShotCount % 2 == 0)
        {
            for (int i = -(mShotCount) / 2; i <= (mShotCount) / 2; i++)
            {
                if (i == 0) continue;
                SpawnSubArrow(projectile, i);
            }
        }
        else
        {
            for (int i = -(mShotCount - 1) / 2; i <= (mShotCount - 1) / 2; i++)
            {
                if (i == 0) continue;
                SpawnSubArrow(projectile, i);
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
