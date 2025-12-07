using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Multi_Lv", menuName = "Archero/SkillData/Active/MultiShotSkillDataSO")]
public class MultiShotSkillDataSO : SkillDataSO
{
    [SerializeField] int mShotCount = 5;
    [SerializeField] float mDamageMultiplier = 0.5f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return new MultiShotStrategy(mShotCount, mDamageMultiplier);
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
