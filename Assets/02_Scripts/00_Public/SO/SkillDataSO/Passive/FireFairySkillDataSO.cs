using UnityEngine;

[CreateAssetMenu(fileName = "Skill_FireFairy", menuName = "Archero/SkillData/Passive/FireFairySkillDataSO")]
public class FireFairySkillDataSO : SkillDataSO
{
    public FireFairy FireFairyPrefab;
    public int ElementNumber = 0;
    public int SeatNumber = 1;
    public float EffectTime = 3f;
    public float DamageTick = 0.2f;
    public float DamageDuplicater = 0.2f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new FireFairyStrategy(this);
    }
}
