using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireCircleStarategy : IPassiveStrategy
{
    private VampireCircleSkillDataSO mSkillDataSO;
    private VampireCircle mVampireCirclePrefab;

    private VampireCircle mVampireCircle;

    public VampireCircleStarategy(VampireCircleSkillDataSO skillDataSO)
    {
        mVampireCirclePrefab = skillDataSO.VampireCirclePrefab;
        mSkillDataSO = skillDataSO;

        Managers.Pool.CreatePool(mVampireCirclePrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mVampireCircle = Managers.Pool.GetFromPool(mVampireCirclePrefab);

        mVampireCircle.SetOwner(attack);
        mVampireCircle.SetUp(mSkillDataSO);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        mVampireCircle.Detach();
        Managers.Pool.ReturnToPool(mVampireCircle);
    }
}
