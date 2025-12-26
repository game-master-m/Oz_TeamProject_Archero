using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCircleStrategy : IPassiveStrategy
{
    private FireCircleSkillDataSO mSkillDataSO;
    private FireCircle mFireCirclePrefab;

    private FireCircle mFireCircle;

    public FireCircleStrategy(FireCircleSkillDataSO skillDataSO)
    {
        mFireCirclePrefab = skillDataSO.FireCirclePrefab;
        mSkillDataSO = skillDataSO;

        Managers.Pool.CreatePool(mFireCirclePrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mFireCircle = Managers.Pool.GetFromPool(mFireCirclePrefab);

        mFireCircle.SetOwner(attack);
        mFireCircle.SetUp(mSkillDataSO);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        if (mFireCircle == null) return;
        mFireCircle.Detach();
        Managers.Pool.ReturnToPool(mFireCircle);
    }
}
