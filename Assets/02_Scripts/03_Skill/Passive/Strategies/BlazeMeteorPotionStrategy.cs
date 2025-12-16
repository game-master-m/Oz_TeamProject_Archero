using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeMeteorPotionStrategy : IPassiveStrategy
{
    private BlazeMeteorPotionSkillDataSO mSkillDataSO;

    private BlazeMeteorPotion mPotionPrefab;

    private BlazeMeteorPotion mPotion;

    private float mPotionSpawnDelay;
    private float mTimer = 0;

    public BlazeMeteorPotionStrategy(BlazeMeteorPotionSkillDataSO skillDataSO)
    {
        mSkillDataSO = skillDataSO;
        mPotionPrefab = mSkillDataSO.PotionPrefab;
        mPotionSpawnDelay = mSkillDataSO.PotionSpawnDelay;
        Managers.Pool.CreatePool(mPotionPrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mPotion = Managers.Pool.GetFromPool(mPotionPrefab);

        mPotion.SetUp(mSkillDataSO, attack);
    }

    public void OnUpdate(PlayerAttack attack)
    {
        //이미 포션이 있으면 리턴
        if (mPotion.gameObject.activeInHierarchy) 
        {
            return;
        }

        //포션 생성 타이머
        mTimer += Time.deltaTime;
        if (mTimer >= mPotionSpawnDelay) 
        {
            mTimer = 0;
            mPotion = Managers.Pool.GetFromPool(mPotionPrefab);
            mPotion.SetUp(mSkillDataSO, attack);
        }
    }

    public void OnUnequip(PlayerAttack attack)
    {

    }
}
