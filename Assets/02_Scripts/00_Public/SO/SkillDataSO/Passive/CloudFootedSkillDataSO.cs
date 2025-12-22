using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CloudFooted_Expert", menuName = "Archero/SkillData/Passive/CloudFootedSkillDataSO")]
public class CloudFootedSkillDataSO : SkillDataSO
{
    public KnockBack_Effect EffectPrefab;
    public float EffectDuration = 1.5f;

    public int DamageCount = 15;
    public float DamageTick = 0.1f;
    public float DamageDuplicater = 0.1f;
    public float DamageDelay = 3f;

    public float KnockBackRadius = 5f;
    public float KnockBackForce = 10f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new CloudFootedStrategy(this);
    }
}
