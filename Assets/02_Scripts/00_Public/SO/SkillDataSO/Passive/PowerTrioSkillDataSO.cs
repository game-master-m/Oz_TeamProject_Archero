using UnityEngine;

[CreateAssetMenu(fileName = "PowerTrio_", menuName = "Archero/SkillData/Passive/PowerTrio")]
public class PowerTrioSkillDataSO : SkillDataSO
{
    [Header("삼위일체 능력치")]
    [SerializeField] private float mMaxHPMultiplier = 1.20f;
    [SerializeField] private float mAttackSpeedMultiplier = 1.2f;
    [SerializeField] private float mAttackDamageMultiplier = 1.15f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new PowerTrioStrategy(mMaxHPMultiplier, mAttackSpeedMultiplier, mAttackDamageMultiplier);
    }
}
