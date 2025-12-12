using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SphereBase : MonoBehaviour
{
    [SerializeField] protected Vector3 mPositionOffset = new Vector3(0, 1.0f, 0);
    [SerializeField] protected float mRotateSpeed = 100.0f;

    protected PlayerAttack mPlayer;
    private float mAttackDamage;
    private float mAttackSpeed;

    public void SetUp(PlayerAttack attack) 
    {
        this.gameObject.transform.position = attack.gameObject.transform.position + mPositionOffset;

        mAttackSpeed = attack.gameObject.GetComponent<PlayerStat>().AttackSpeed;
        mAttackDamage = attack.gameObject.GetComponent<PlayerStat>().AttackDamage * 0.4f;

        Sphere[] childShpheres = GetComponentsInChildren<Sphere>();
        foreach (Sphere sphere in childShpheres) 
        {
            sphere.SetOwner(this);
        }
    }

    public void OnHitTarget(EnemyBase target) 
    {
        ApplyDamage(target, mAttackDamage);
    }

    public abstract void ApplyDamage(EnemyBase target, float damage);
}
