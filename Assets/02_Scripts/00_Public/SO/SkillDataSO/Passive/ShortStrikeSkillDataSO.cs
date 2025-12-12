using UnityEngine;

[CreateAssetMenu(fileName = "ShortStrike_", menuName = "Archero/SkillData/Passive/ShortStrike")]
public class ShortStrikeSkillDataSO : SkillDataSO
{
    [Header("단거리사격 능력치")]
    [SerializeField] private float mAttackRangeMultiplier = 0.6f;
    [SerializeField] private float mAttackDamageMultiplier = 1.2f;
    [SerializeField] private float mAttackSpeedMultiplier = 2.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new ShortStrikeStrategy(mAttackRangeMultiplier, mAttackDamageMultiplier, mAttackSpeedMultiplier);
    }
}
