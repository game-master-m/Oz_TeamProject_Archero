using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToxicMeteorPotion_Normal", menuName = "Archero/SkillData/Passive/ToxicMeteorPotionSkillDataSO")]
public class ToxicMeteorPotionSkillDataSO : SkillDataSO
{
    public ToxicMeteor MeteorPrefab;
    public ToxicMeteorPotion PotionPrefab;

    public Vector3 MeteorOffset = new Vector3(0, 5.0f, 0);

    public int ElementNumber = 0;
    public float EffectTime = PublicDamageConstans.VenomEffectTime;
    public float DamageTick = PublicDamageConstans.VenomDamageTick;
    public float DamageDuplicater = PublicDamageConstans.VenomDamageDuplicater;
    public float DamageRange = 20;
    public float MeteorSpeed = 20;
    public float PotionSpawnDelay = 3.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new ToxicMeteorPotionStarategy(this);
    }
}
