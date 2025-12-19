using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlazeMeteorPotion_Normal", menuName = "Archero/SkillData/Passive/BlazeMeteorPotionSkillDataSO")]
public class BlazeMeteorPotionSkillDataSO : SkillDataSO
{
    public BlazeMeteor MeteorPrefab;
    public BlazeMeteorPotion PotionPrefab;

    public Vector3 MeteorOffset = new Vector3(0, 5.0f, 0);

    public int ElementNumber = 0;
    public float EffectTime = PublicDamageConstans.FireEffectTime;
    public float DamageTick = PublicDamageConstans.FireDamageTick;
    public float DamageDuplicater = PublicDamageConstans.FireDamageDuplicater;
    public float DamageRange = 20;
    public float MeteorSpeed = 20;
    public float PotionSpawnDelay = 3.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new BlazeMeteorPotionStrategy(this);
    }
}
