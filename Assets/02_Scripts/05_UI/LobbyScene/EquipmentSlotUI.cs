using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotUI : SlotUI
{
    public override void OnButtonClick()
    {
        if (PlayerInventory.Instance == null) return;

        PlayerInventory.Instance.UnEquipItem(mEquipment);
    }
}
