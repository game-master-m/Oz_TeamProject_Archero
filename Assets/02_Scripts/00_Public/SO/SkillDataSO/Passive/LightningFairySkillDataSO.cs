using UnityEngine;

[CreateAssetMenu(fileName = "Skill_LightningFairy", menuName = "Archero/SkillData/Passive/LightningFairySkillDataSO")]
public class LightningFairySkillDataSO : SkillDataSO
{
    public LightningFairy mLightningFairyPrefab;
    public int mElementNumber = 1;
    public int mSeatNumber = 3;
    public int mMaxChainCount = 8;
    public float mEffectTime = 0f;
    public float mDamageTick = 0f;
    public float mDamageDuplicater = 0.3f;
    public float mChainRange = 6.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new LightningFairyStrategy(this);
    }
}
