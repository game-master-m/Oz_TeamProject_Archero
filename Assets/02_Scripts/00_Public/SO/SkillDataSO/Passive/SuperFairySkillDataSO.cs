using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuperFairy", menuName = "Archero/SkillData/Passive/SuperFairySkillDataSO")]
public class SuperFairySkillDataSO : SkillDataSO
{
    public SuperFairy SuperFairyPrefab;
    public int ElementNumber = 4;
    public int SeatNumber = 4;
    public float LaserDuration = PublicDamageConstans.SuperLaserDuration;
    public float LaserDamageTick = PublicDamageConstans.SuperLaserDamageTick;
    public float LaserRange = PublicDamageConstans.SuperLaserRange;
    public float LaserDamageDuplicater = PublicDamageConstans.SuperLaserDamageDuplicater;
    public float LaserChainCount = PublicDamageConstans.SuperLaserChainCount;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new SuperFairyStrategy(this);
    }
}
