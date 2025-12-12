using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_NormalSphere", menuName = "Archero/SkillData/Passive/NormalSphereSkillDataSO")]
public class NormalSphereSkillDataSO : SkillDataSO
{
    [SerializeField] private NormalSphere mNormalSpherePrefab;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {

        return new NormalSphereStrategy(mNormalSpherePrefab);
    }
}
