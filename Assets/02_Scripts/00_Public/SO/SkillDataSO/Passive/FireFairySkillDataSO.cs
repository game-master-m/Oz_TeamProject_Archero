using UnityEngine;

[CreateAssetMenu(fileName = "Skill_FireFairy", menuName = "Archero/SkillData/Passive/FireFairySkillDataSO")]
public class FireFairySkillDataSO : SkillDataSO
{
    public FireFairy mFireFairyPrefab;
    public int mElementNumber = 0;
    public int mSeatNumber = 1;
    public float mEffectTime = 3f;
    public float mDamageTick = 0.2f;
    public float mDamageDuplicater = 0.2f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new FireFairyStrategy(this);
    }
}
