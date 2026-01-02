using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUI : SlotUI
{
    //발송...
    [SerializeField] private ItemSlotEventChannelSO mOnInvenItemSelected;   //InvenItemInfoUI가 구독

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        //아이템 인포 패널 활성화
        mOnInvenItemSelected?.Raised(mSlot);
    }

}
