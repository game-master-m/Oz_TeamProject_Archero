using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemSlotListsEvent", menuName = "Archero/EventChannel/Item Slot List Event Channel")]
public class ItemSlotsEventChannelSO : ScriptableObject
{
    public event Action<List<ItemSlot>> onEvent;
    public void Raised(List<ItemSlot> slots)
    {
        onEvent?.Invoke(slots);
    }
}
