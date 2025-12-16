using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_VenomCircle", menuName = "Archero/SkillData/Passive/VenomCircleSkillDataSO")]
public class VenomCircleSkillDataSO : SkillDataSO
{
    public VenomCircle VenomCirclePrefab;

    public Vector3 PositionOffset = new Vector3(0, 1.0f, 0);
    public float RotateSpeed = 100.0f;

    public float EffectTime = PublicDamageConstans.VenomEffectTime;
    public float DamageTick = PublicDamageConstans.VenomDamageTick;
    public float DamageDuplicater = PublicDamageConstans.VenomDamageDuplicater;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {

        return new VenomCircleStrategy(this);
    }
}
