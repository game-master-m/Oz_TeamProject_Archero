using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Split_Lv", menuName = "Archero/SkillData/Active/SplitShotSkillDataSO")]
public class SplitShotSkillDataSO : SkillDataSO
{
    [SerializeField] int mSplitCount = 3;
    [SerializeField] float mDamageMultiplier = 0.4f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return new SplitShotStrategy(mSplitCount, mDamageMultiplier);
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
