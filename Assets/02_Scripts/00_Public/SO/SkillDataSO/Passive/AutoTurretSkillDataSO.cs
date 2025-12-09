using UnityEngine;

[CreateAssetMenu(fileName = "AutoTurret", menuName = "Archero/SkillData/Passive/AutoTurretSkillDataSO")]
public class AutoTurretSkillDataSO : SkillDataSO
{
    [SerializeField] private float mAttackSpeedMultiplier = 0.5f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new AutoTurretStrategy(mAttackSpeedMultiplier);
    }
}
