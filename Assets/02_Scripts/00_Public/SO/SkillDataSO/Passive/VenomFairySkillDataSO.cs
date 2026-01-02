using UnityEngine;

[CreateAssetMenu(fileName = "Skill_VenomFairy", menuName = "Archero/SkillData/Passive/VenomFairySkillDataSO")]
public class VenomFairySkillDataSO : SkillDataSO
{
    public VenomFairy VenomFairyPrefab;
    public int ElementNumber = 2;
    public int SeatNumber = 2;
    public float EffectTime = PublicDamageConstans.VenomEffectTime;
    public float DamageTick = PublicDamageConstans.VenomDamageTick;
    public float DamageDuplicater = PublicDamageConstans.VenomDamageDuplicater;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new VenomFairyStrategy(this);
    }
}
