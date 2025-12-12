using UnityEngine;

[CreateAssetMenu(fileName = "GiantStrength_", menuName = "Archero/SkillData/Passive/GiantStrength")]
public class GiantStrengthSkillDataSO : SkillDataSO
{
    [Header("거인의 힘 능력치")]
    [SerializeField] private float mAttackDamageMultiplier = 1.2f;
    [SerializeField] private float mMaxHPMultiplier = 1.25f;
    [SerializeField] private float mMoveSpeedMultiplier = 0.6f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new GiantStrengthStrategy(mAttackDamageMultiplier, mMaxHPMultiplier, mMoveSpeedMultiplier);
    }
}
