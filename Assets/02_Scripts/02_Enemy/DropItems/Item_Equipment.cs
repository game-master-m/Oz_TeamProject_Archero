using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipment : ItemBase
{
    private void OnTriggerEnter(Collider other)
    {
        //Layer설정으로 플레이와만 반응 함
        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mGetExpSound);
        Managers.Data.AddItemToInventory(ItemDataSO, 1);
        Managers.Pool.ReturnToPool(this);
    }

    public override void ReturnPool()
    {
        Managers.Pool.ReturnToPool(this);
    }
}
