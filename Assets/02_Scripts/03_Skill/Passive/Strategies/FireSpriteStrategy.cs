using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpriteStrategy : IPassiveStrategy
{
    [SerializeField] private FireSprite mFireSpritePrefab;
    [SerializeField] private int mSpriteCount;

    private FireSprite mFireSprite;

    public FireSpriteStrategy(int spriteCount, FireSprite spritePrefab)
    {
        mFireSpritePrefab = spritePrefab;   
        mSpriteCount = spriteCount;
        Managers.Pool.CreatePool(mFireSpritePrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack) 
    {
        mFireSprite = Managers.Pool.GetFromPool(mFireSpritePrefab);
        mFireSprite.SetUp(attack, mSpriteCount);
    }

    public void OnUpdate(PlayerAttack attack)
    {
        
    }

    public void OnUnequip(PlayerAttack attack)
    {
        mFireSprite.StopAllCoroutines();
        mFireSprite.Detach();
        Managers.Pool.ReturnToPool(mFireSprite);
    }
}
