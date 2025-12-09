using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FanShot_Lv", menuName = "Archero/SkillData/Active/MultiShotSkillDataSO")]
public class FanShotSkillDataSO : SkillDataSO
{
    [Header("FanShot ´É·ÂÄ¡")]
    [SerializeField] int mShotCount = 5;
    [SerializeField] float mDamageMultiplier = 0.5f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return new FanShotStrategy(mShotCount, mDamageMultiplier);
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
