using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipment : ItemBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.Tag_Player))
        {
            if (PlayerInventory.Instance != null) 
            {
                //여기에 인벤토리에 넣는 내용
                PlayerInventory.Instance.AddItem(this, 1);
            }
            Managers.Pool.ReturnToPool(this);
        }
    }

    public override void ReturnPool()
    {
        Managers.Pool.ReturnToPool(this);
    }
}
