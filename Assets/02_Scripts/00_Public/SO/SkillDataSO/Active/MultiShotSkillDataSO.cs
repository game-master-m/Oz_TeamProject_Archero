using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiShot_Legend", menuName = "Archero/SkillData/Active/MultiShotSkillDataSO")]
public class MultiShotSkillDataSO : SkillDataSO
{
    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return new MultiShotStrategy();
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
