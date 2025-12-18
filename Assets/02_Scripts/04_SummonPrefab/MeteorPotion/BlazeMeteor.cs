using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeMeteor : MeteorBase
{
    private float mFireDamage;
    private float mEffectTime;
    private float mDamageTick;

    //세팅
    public void SetUp(BlazeMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
    {
        mRange = skillDataSO.DamageRange;
        mMeteorDamage = attack.Stat.AttackDamage + PublicDamageConstans.MeteorDamageDuplicater;
        mFireDamage = attack.Stat.AttackDamage * skillDataSO.DamageDuplicater;
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        Utils.Log("메테오 셋업 완료");
    }

    //속성 부여
    public override void Applyelement(EnemyBase enemy)
    {
        enemy.TakeDotDamage(mFireDamage, mEffectTime, mDamageTick, EDmgElement.Fire);
    }

    public override void ReturnPool()
    {
        Managers.Pool.ReturnToPool(this);
    }
}
