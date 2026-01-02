using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FairyPower", menuName = "Archero/SkillData/Passive/FairyPowerSkillDataSO")]
public class FairyPowerSkillDataSO : SkillDataSO
{
    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new FairyPowerStrategy(this);
    }
}
