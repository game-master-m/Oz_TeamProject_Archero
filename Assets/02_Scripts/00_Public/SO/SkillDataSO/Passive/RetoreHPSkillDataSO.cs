using UnityEngine;

[CreateAssetMenu(fileName = "RestoreHP_", menuName = "Archero/SkillData/Passive/RestoreHP")]
public class RestoreHPSkillSO : SkillDataSO
{
    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new RestoreHPStrategy();
    }
}
