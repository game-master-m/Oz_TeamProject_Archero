using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSphereStrategy : IPassiveStrategy
{
    [SerializeField] private NormalSphere mNormalSpherePrefab;
 

    private NormalSphere mNormalSphere;

    public NormalSphereStrategy(NormalSphere spherePrefab)
    {
        mNormalSpherePrefab = spherePrefab;
    }

    public void OnEquip(PlayerAttack attack)
    {
      
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
       
    }
}
