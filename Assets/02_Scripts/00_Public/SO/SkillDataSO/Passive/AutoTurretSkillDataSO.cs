using UnityEngine;

[CreateAssetMenu(fileName = "AutoTurret", menuName = "Archero/SkillData/Passive/AutoTurret")]
public class AutoTurretSkillDataSO : SkillDataSO
{
    [Header("오토터렛 능력치")]
    [SerializeField] private float mAttackDamageMultiplier = 0.6f;
    [SerializeField] private float mAttackSpeedMultiplier;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new AutoTurretStrategy(mAttackDamageMultiplier, mAttackSpeedMultiplier);
    }
}
