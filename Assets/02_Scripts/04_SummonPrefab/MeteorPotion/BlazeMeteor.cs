using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeMeteor : MeteorBase
{
    private float mFireDamage;
    private float mEffectTime;
    private float mDamageTick;

    //속성 관련 세팅
    public override void SetElement(BlazeMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack) 
    {
        mFireDamage = attack.Stat.AttackDamage * skillDataSO.DamageDuplicater;
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        Utils.Log("메테오 셋업 완료");
    }

    //속성 부여
    public override void Applyelement(EnemyBase enemy)
    {
        enemy.TakeDotDamage(mFireDamage, mEffectTime, mDamageTick);
    }

    public override void ReturnPool()
    {
        Managers.Pool.ReturnToPool(this);
    }
}
