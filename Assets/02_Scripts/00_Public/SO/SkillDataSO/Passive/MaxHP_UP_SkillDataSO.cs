using UnityEngine;

[CreateAssetMenu(fileName = "MaxHP_UP_", menuName = "Archero/SkillData/Passive/MaxHP_UP")]
public class MaxHP_UP_SkillDataSO : SkillDataSO
{
    [Header("최대체력증가 능력치")]
    [SerializeField] private float mMaxHPMultiplier = 1.1f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new MaxHP_UP_Strategy(mMaxHPMultiplier);
    }
}
