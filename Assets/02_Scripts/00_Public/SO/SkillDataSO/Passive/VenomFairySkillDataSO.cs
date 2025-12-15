using UnityEngine;

[CreateAssetMenu(fileName = "Skill_VenomFairy", menuName = "Archero/SkillData/Passive/VenomFairySkillDataSO")]
public class VenomFairySkillDataSO : SkillDataSO
{
    public VenomFairy VenomFairyPrefab;
    public int ElementNumber = 2;
    public int SeatNumber = 2;
    public float EffectTime = 9999f;
    public float DamageTick = 1.0f;
    public float DamageDuplicater = 0.5f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new VenomFairyStrategy(this);
    }
}
