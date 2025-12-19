using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserCircle", menuName = "Archero/SkillData/Passive/LaserCircleSkillDataSO")]

public class LaserCircleSkillDataSO : SkillDataSO
{
    public LaserCircle LaserCirclePrefab;
    public Vector3 PositionOffset = new Vector3(0, 1.0f, 0);
    public float LaserDuration = 5.0f;
    public float LaserRange = PublicDamageConstans.LaserRange;
    public float LaserDelay = 3.0f;
    public float RotateSpeed = 100.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new LaserCircleStrategy(this);
    }
}
