using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_FireSprite", menuName = "Archero/SkillData/Passive/FireSpriteSkillDataSO")]
public class FireSpriteSkillDataSO : SkillDataSO
{
    [SerializeField] private FireSprite mFireSpritePrefab;
    [SerializeField] private int mSpriteCount = 1;

    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public override IPassiveStrategy CreatePassiveStrategy()
    {
        
        return new FireSpriteStrategy(mSpriteCount, mFireSpritePrefab);
    }
}
