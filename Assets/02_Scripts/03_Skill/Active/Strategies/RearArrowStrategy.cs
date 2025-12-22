using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearArrowStrategy : IProjectileStrategy
{
    public RearArrowStrategy()
    {

    }

    public void OnHit(Projectile projectile, IDamageable target) { }

    public void OnShoot(Projectile projectile)
    {
        SpawnSubArrow(projectile);
    }

    private void SpawnSubArrow(Projectile projectile)
    {
        Projectile subArrow = Managers.Pool.GetFromPool(projectile);
        subArrow.gameObject.transform.position = projectile.transform.position - projectile.transform.forward * 3f;
        subArrow.gameObject.transform.rotation = projectile.transform.rotation;
        subArrow.gameObject.transform.Rotate(0.0f, 180f, 0.0f);
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
