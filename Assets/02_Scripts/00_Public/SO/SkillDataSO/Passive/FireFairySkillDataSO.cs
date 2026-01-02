using UnityEngine;

[CreateAssetMenu(fileName = "Skill_FireFairy", menuName = "Archero/SkillData/Passive/FireFairySkillDataSO")]
public class FireFairySkillDataSO : SkillDataSO
{
    public FireFairy FireFairyPrefab;
    public int ElementNumber = 0;
    public int SeatNumber = 1;
    public float EffectTime = PublicDamageConstans.FireEffectTime;
    public float DamageTick = PublicDamageConstans.FireDamageTick;
    public float DamageDuplicater = PublicDamageConstans.FireDamageDuplicater;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new FireFairyStrategy(this);
    }
}
