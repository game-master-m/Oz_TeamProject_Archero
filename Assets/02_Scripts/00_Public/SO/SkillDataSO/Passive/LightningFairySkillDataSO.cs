using UnityEngine;

[CreateAssetMenu(fileName = "Skill_LightningFairy", menuName = "Archero/SkillData/Passive/LightningFairySkillDataSO")]
public class LightningFairySkillDataSO : SkillDataSO
{
    public LightningFairy LightningFairyPrefab;
    public int ElementNumber = 1;
    public int SeatNumber = 3;
    public int MaxChainCount = 8;
    public float DamageDuplicater = PublicDamageConstans.LightningDamageDuplicater;
    public float ChainRange = 10.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new LightningFairyStrategy(this);
    }
}
