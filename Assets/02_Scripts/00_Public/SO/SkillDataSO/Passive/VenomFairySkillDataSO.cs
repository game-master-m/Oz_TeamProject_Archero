using UnityEngine;

[CreateAssetMenu(fileName = "Skill_VenomFairy", menuName = "Archero/SkillData/Passive/VenomFairySkillDataSO")]
public class VenomFairySkillDataSO : SkillDataSO
{
    public VenomFairy mVenomFairyPrefab;
    public int mElementNumber = 2;
    public int mSeatNumber = 2;
    public float mEffectTime = 9999f;
    public float mDamageTick = 1.0f;
    public float mDamageDuplicater = 0.5f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new VenomFairyStrategy(this);
    }
}
