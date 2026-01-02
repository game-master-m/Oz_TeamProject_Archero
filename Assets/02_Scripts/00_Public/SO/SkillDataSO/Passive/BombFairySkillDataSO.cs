using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombFairy", menuName = "Archero/SkillData/Passive/BombFairySkillDataSO")]
public class BombFairySkillDataSO : SkillDataSO
{
    public BombFairy BombFairyPrefab;
    public int ElementNumber = 4;
    public int SeatNumber = 1;
    public float BombCount = PublicDamageConstans.BombCount;
    public float BombThrowTick = PublicDamageConstans.BombThrowTick;
    public float DamageDuplicater = PublicDamageConstans.BombDamageDuplicater;
    public float BombRange = PublicDamageConstans.BombRange;
    public float BombHeights = 5.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new BombFairyStrategy(this);
    }
}
