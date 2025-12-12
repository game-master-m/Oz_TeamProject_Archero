using UnityEngine;

[CreateAssetMenu(fileName = "FOL_", menuName = "Archero/SkillData/Passive/FOL")]
public class FOL_SkillDataSO : SkillDataSO
{
    [Header("최대체력증가 능력치")]
    [SerializeField] private float mMaxHPMultiplier = 1.15f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new FOL_Strategy(mMaxHPMultiplier);
    }
}
