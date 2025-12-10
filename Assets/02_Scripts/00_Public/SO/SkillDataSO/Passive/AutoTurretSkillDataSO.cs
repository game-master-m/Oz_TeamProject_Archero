using UnityEngine;

[CreateAssetMenu(fileName = "AutoTurret", menuName = "Archero/SkillData/Passive/AutoTurretSkillDataSO")]
public class AutoTurretSkillDataSO : SkillDataSO
{
    [Header("오토터렛 능력치")]
    [SerializeField] private float mAttackDamageMultiplier = 0.6f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new AutoTurretStrategy(mAttackDamageMultiplier);
    }
}
