using UnityEngine;

[CreateAssetMenu(fileName = "MoveSpeed_UP_", menuName = "Archero/SkillData/Passive/MoveSpeed_UP")]
public class MoveSpeed_UP_SkillDataSO : SkillDataSO
{
    [Header("이속증가 능력치")]
    [SerializeField] private float mMoveSpeedMultiplier = 1.1f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new MoveSpeed_UP_Strategy(mMoveSpeedMultiplier);
    }
}
