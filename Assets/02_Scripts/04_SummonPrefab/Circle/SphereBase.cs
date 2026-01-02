using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SphereBase : MonoBehaviour
{
    protected static int mTotalCount = 0;

    protected Vector3 mPositionOffset;
    protected float mRotateSpeed;
    [SerializeField] protected PlayerAttack mPlayer;
    protected Sphere[] mChildShpheres;

    private float mAttackDamage;
    private float mAttackSpeed;

    public void SetOwner(PlayerAttack attack)
    {
        mTotalCount++;

        mPlayer = attack;

        Utils.Log($"토탈카운트: {mTotalCount}");
        this.gameObject.transform.position = attack.gameObject.transform.position + mPositionOffset;

        mAttackSpeed = attack.Stat.AttackSpeed;
        mAttackDamage = attack.Stat.AttackDamage;

        mChildShpheres = GetComponentsInChildren<Sphere>();
        foreach (Sphere sphere in mChildShpheres)
        {
            sphere.SetOwner(this);
        }
    }

    //풀에 집어넣기 전에 플레이어한테서 떼어내기
    public void Detach()
    {
        mTotalCount--;
        this.gameObject.transform.SetParent(null, false);
    }


    public void OnHitTarget(EnemyBase target)
    {
        ApplyDamage(target, mAttackDamage);
    }

    public abstract void ApplyDamage(EnemyBase target, float damage);
}
