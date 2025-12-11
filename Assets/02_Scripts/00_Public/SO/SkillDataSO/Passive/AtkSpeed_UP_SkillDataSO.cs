using UnityEngine;

[CreateAssetMenu(fileName = "AtkSpeed_UP_", menuName = "Archero/SkillData/Passive/AtkSpeed_UP")]
public class AtkSpeed_UP_SkillDataSO : SkillDataSO
{
    [Header("공속증가 능력치")]
    [SerializeField] private float mAtkSpeedMultiplier = 1.1f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new AtkSpeed_UP_Strategy(mAtkSpeedMultiplier);
    }
}
