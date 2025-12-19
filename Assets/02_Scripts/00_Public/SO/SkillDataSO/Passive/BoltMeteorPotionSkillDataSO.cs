using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoltMeteorPotion_Normal", menuName = "Archero/SkillData/Passive/BoltMeteorPotionSkillDataSO")]
public class BoltMeteorPotionSkillDataSO : SkillDataSO
{
    public BoltMeteor MeteorPrefab;
    public BoltMeteorPotion PotionPrefab;

    public Vector3 MeteorOffset = new Vector3(0, 5.0f, 0);

    public int ElementNumber = 0;
    public int MaxChainCount = 8;
    public float ChainRange = 10;
    public float DamageDuplicater = PublicDamageConstans.LightningDamageDuplicater;
    public float DamageRange = 20;
    public float MeteorSpeed = 20;
    public float PotionSpawnDelay = 3.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new BoltMeteorPotionStrategy(this);
    }
}
