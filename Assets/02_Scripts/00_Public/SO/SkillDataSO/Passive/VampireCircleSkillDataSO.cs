using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VampireCircle_Normal", menuName = "Archero/SkillData/Passive/VampireCircleSkillDataSO")]
public class VampireCircleSkillDataSO : SkillDataSO
{
    public VampireCircle VampireCirclePrefab;

    public Vector3 PositionOffset = new Vector3(0, 1.0f, 0);
    public float RotateSpeed = 100.0f;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        return new VampireCircleStarategy(this);
    }
}
