using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class MeteorBase : MonoBehaviour
{
    private Collider[] mCollidersInRange = new Collider[30];
    private Rigidbody mRigidbody;
    protected BlazeMeteorPotionSkillDataSO mSkillData;
    protected PlayerAttack mPlayer;

    protected float mMeteorSpeed;
    protected float mRange;
    protected float mMeteorDamage;
    protected WaitForSeconds mWaitEffect;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mWaitEffect = new WaitForSeconds(0.5f);
    }

    private void FixedUpdate()
    {
        mRigidbody.MovePosition(transform.position + Vector3.down * mMeteorSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHitGround();
    }

    //ÁÖº¯ ÀûÇÑÅ× µ¥¹ÌÁö
    private void OnHitGround() 
    {
        Utils.Log("¶¥¿¡ºÎµúÈû");

        int detectCount = Physics.OverlapSphereNonAlloc(transform.position, mRange, mCollidersInRange, Layers.GetLayerMask(ELayerName.Enemy));
       
        if(detectCount == 0) { return; }

        for (int i = 0; i < detectCount; i++) 
        {
            if (!mCollidersInRange[i].enabled || !mCollidersInRange[i].gameObject.activeInHierarchy) continue;
            if (mCollidersInRange[i].gameObject.TryGetComponent<EnemyBase>(out EnemyBase damageable))
            {
                damageable.TakeDamage(mMeteorDamage);
                Applyelement(damageable);
            }
        }

        SetExplodeEffect();
    }
    public abstract void Applyelement(EnemyBase enemy);
    public abstract void ReturnPool();
    protected abstract void SetExplodeEffect();
}
