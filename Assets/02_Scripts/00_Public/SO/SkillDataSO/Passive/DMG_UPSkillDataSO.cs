using UnityEngine;

[CreateAssetMenu(fileName = "DMG_UP_", menuName = "Archero/SkillData/Passive/DMG_UP")]
public class DMG_UPSkillDataSO : SkillDataSO
{
    [Header("데미지업 능력치")]
    [SerializeField] private float mAttackDamageMultiplier = 1.1f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new DMG_UP_Strategy(mAttackDamageMultiplier);
    }
}
