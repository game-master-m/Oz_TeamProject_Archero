using UnityEngine;

[CreateAssetMenu(fileName = "FairyOfWind_", menuName = "Archero/SkillData/Passive/FairyOfWind")]
public class FairyOfWindSkillDataSO : SkillDataSO
{
    [Header("바람의요정 능력치")]
    [SerializeField] private float mMoveSpeedMultiplier = 1.1f;
    [SerializeField] private float mAttackSpeedMultiplier = 1.1f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new FairyOfWindStrategy(mMoveSpeedMultiplier, mAttackSpeedMultiplier);
    }
}
