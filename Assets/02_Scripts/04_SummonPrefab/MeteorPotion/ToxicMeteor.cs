using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicMeteor : MeteorBase
{
    private float mToxicDamage;
    private float mEffectTime;
    private float mDamageTick;

    //세팅
    public void SetUp(ToxicMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
    {
        mRange = skillDataSO.DamageRange;
        mMeteorDamage = attack.Stat.AttackDamage + PublicDamageConstans.MeteorDamageDuplicater;
        mToxicDamage = attack.Stat.AttackDamage * skillDataSO.DamageDuplicater;
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mMeteorSpeed = skillDataSO.MeteorSpeed;
        Utils.Log("메테오 셋업 완료");
    }

    //속성 부여
    public override void Applyelement(EnemyBase enemy)
    {
        enemy.TakeDotDamage(mToxicDamage, mEffectTime, mDamageTick, EDmgElement.Poison);
    }

    public override void ReturnPool()
    {
        Managers.Pool.ReturnToPool(this);
    }
}
