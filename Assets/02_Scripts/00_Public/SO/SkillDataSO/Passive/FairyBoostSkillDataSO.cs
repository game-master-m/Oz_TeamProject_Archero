using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FairyBoost", menuName = "Archero/SkillData/Passive/FairyBoostSkillDataSO")]
public class FairyBoostSkillDataSO : SkillDataSO
{
    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new FairyBoostStarategy(this);
    }
}
