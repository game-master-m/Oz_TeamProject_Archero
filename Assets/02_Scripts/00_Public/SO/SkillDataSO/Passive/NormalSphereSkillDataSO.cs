using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_NormalSphere", menuName = "Archero/SkillData/Passive/NormalSphereSkillDataSO")]
public class NormalSphereSkillDataSO : SkillDataSO
{
    public NormalSphere NormalSpherePrefab;

    public Vector3 PositionOffset = new Vector3(0, 1.0f, 0);
    public float RotateSpeed = 100.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {

        return new NormalSphereStrategy(this);
    }
}
