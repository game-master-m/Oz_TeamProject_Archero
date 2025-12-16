using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_LightningCircle", menuName = "Archero/SkillData/Passive/LightningCircleSkillDataSO")]
public class LightningCircleSkillDataSO : SkillDataSO
{
    public LightningCircle LightningCirclePrefab;

    public Vector3 PositionOffset = new Vector3(0, 1.0f, 0);
    public float RotateSpeed = 100.0f;

    public int MaxChainCount = 8;
    public float ChainRange = 10.0f;
    public float DamageDuplicater = PublicDamageConstans.FireDamageDuplicater;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {

        return new LightningCircleStarategy(this);
    }
}
