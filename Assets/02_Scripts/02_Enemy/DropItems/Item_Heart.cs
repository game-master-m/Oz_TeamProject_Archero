using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Heart : ItemBase
{
    [SerializeField] private float mHealAmount = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.Tag_Player)) 
        {
            other.gameObject.TryGetComponent(out PlayerAttack attack);

            attack.Stat.AddHP(mHealAmount);

            Managers.Pool.ReturnToPool(this);
        }
    }

    public override void ReturnPool()
    {
        Managers.Pool.ReturnToPool(this);   
    }
}
