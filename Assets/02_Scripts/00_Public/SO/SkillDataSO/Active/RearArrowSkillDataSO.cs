using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RearArrow_Normal", menuName = "Archero/SkillData/Active/RearArrowSkillDataSO")]
public class RearArrowSkillDataSO : SkillDataSO
{
    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return new RearArrowStrategy();
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
