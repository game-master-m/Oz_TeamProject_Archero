using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class MeteorBase : MonoBehaviour
{
    private Collider[] mCollidersInRange = new Collider[30];
    protected BlazeMeteorPotionSkillDataSO mSkillData;
    protected PlayerAttack mPlayer;

    protected float mRange;
    protected float mMeteorDamage;

    private void OnTriggerEnter(Collider other)
    {
        OnHitGround();
    }

    //세팅
    public void SetUp(BlazeMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
    {
        mRange = skillDataSO.DamageRange;
        mMeteorDamage = attack.Stat.AttackDamage + PublicDamageConstans.MeteorDamageDuplicater;
        SetElement(skillDataSO, attack);
    }

    //주변 적한테 데미지
    private void OnHitGround() 
    {
        Utils.Log("땅에부딪힘");

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

        ReturnPool();
    }

    public abstract void SetElement(BlazeMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack);
    public abstract void Applyelement(EnemyBase enemy);
    public abstract void ReturnPool();
}
