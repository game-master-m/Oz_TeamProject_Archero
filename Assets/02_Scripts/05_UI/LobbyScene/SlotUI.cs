using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SlotUI : MonoBehaviour
{
    protected ItemBase mItem;
    protected Item_Equipment mEquipment;

    public void SetItemData(ItemBase item) 
    {
        mItem = item;
        SetItemSprite();
    }

    public void SetItemData(Item_Equipment item)
    {
        mEquipment = item;
        mItem = mEquipment;
        SetItemSprite();
    }

    private void SetItemSprite() 
    {
        if (TryGetComponent(out Image image)) 
        {
            image.sprite = mItem.ItemDataSO.ItemSprite;
        }
    }

    public abstract void OnButtonClick();
}
