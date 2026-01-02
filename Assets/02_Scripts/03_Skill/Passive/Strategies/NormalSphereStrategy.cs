using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSphereStrategy : IPassiveStrategy
{
    private NormalSphereSkillDataSO mSkillDataSO;
    private NormalSphere mNormalSpherePrefab;

    private NormalSphere mNormalSphere;

    public NormalSphereStrategy(NormalSphereSkillDataSO skillDataSO)
    {
        mNormalSpherePrefab = skillDataSO.NormalSpherePrefab;
        mSkillDataSO = skillDataSO;

        Managers.Pool.CreatePool(mNormalSpherePrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mNormalSphere = Managers.Pool.GetFromPool(mNormalSpherePrefab);

        mNormalSphere.SetOwner(attack);
        mNormalSphere.SetUp(mSkillDataSO);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
       mNormalSphere.Detach();
       Managers.Pool.ReturnToPool(mNormalSphere);
    }
}
