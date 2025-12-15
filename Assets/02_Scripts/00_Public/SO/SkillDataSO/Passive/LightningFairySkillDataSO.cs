using UnityEngine;

[CreateAssetMenu(fileName = "Skill_LightningFairy", menuName = "Archero/SkillData/Passive/LightningFairySkillDataSO")]
public class LightningFairySkillDataSO : SkillDataSO
{
    public LightningFairy LightningFairyPrefab;
    public int ElementNumber = 1;
    public int SeatNumber = 3;
    public int MaxChainCount = 8;
    public float EffectTime = 0f;
    public float DamageTick = 0f;
    public float DamageDuplicater = 0.3f;
    public float ChainRange = 6.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new LightningFairyStrategy(this);
    }
}
