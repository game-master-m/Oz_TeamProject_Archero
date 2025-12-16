using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_FireCircle", menuName = "Archero/SkillData/Passive/FireCircleSkillDataSO")]
public class FireCircleSkillDataSO : SkillDataSO
{
    public FireCircle FireCirclePrefab;

    public Vector3 PositionOffset = new Vector3(0, 1.0f, 0);
    public float RotateSpeed = 100.0f;

    public float EffectTime = PublicDamageConstans.FireEffectTime;
    public float DamageTick = PublicDamageConstans.FireDamageTick;
    public float DamageDuplicater = PublicDamageConstans.FireDamageDuplicater;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {

        return new FireCircleStrategy(this);
    }
}
