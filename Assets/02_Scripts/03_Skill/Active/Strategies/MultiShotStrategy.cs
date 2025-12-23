using UnityEngine;
using System.Collections;

public class MultiShotStrategy : IProjectileStrategy, ISkillStackable<IProjectileStrategy>
{
    private int mAddtionalNum = 1;
    public void OnHit(Projectile projectile, IDamageable target) { }

    public void OnShoot(Projectile projectile)
    {
        for (int i = 0; i < mAddtionalNum; ++i) 
        {
            SpawnSubArrow(projectile);
        }
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

    private void SpawnSubArrow(Projectile projectile)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), 0f, 0f);
        Projectile subArrow = Managers.Pool.GetFromPool(projectile);
        subArrow.gameObject.transform.position = projectile.transform.position - projectile.transform.forward * 1 + randomOffset;
        subArrow.gameObject.transform.rotation = projectile.transform.rotation;
  
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
