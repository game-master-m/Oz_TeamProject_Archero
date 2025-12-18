using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserFairy_Normal", menuName = "Archero/SkillData/Passive/LaserFairySkillDataSO")]
public class LaserFairySkillDataSO : SkillDataSO
{
    public LaserFairy LaserFairyPrefab;
    public int ElementNumber = 4;
    public int SeatNumber = 3;
    public float LaserDuration = PublicDamageConstans.LaserDuration;
    public float LaserDamageTick = PublicDamageConstans.LaserDamageTick;
    public float LaserRange = PublicDamageConstans.LaserRange;
    public float LaserDamageDuplicater = PublicDamageConstans.LaserDamageDuplicater;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new LaserFairyStrategy(this);
    }
}
