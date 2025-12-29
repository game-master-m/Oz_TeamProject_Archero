using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemSlotEvent", menuName = "Archero/EventChannel/ItemSlot Event Channel")]
public class ItemSlotEventChannelSO : ScriptableObject
{
    public event Action<ItemSlot> onEvent;
    public void Raised(ItemSlot slot)
    {
        onEvent?.Invoke(slot);
    }
}
