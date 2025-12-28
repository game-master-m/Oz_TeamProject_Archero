using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemSlotsEvent", menuName = "Archero/EventChannel/ItemSlots Event Channel")]
public class ItemSlotsEventChannelSO : ScriptableObject
{
    public event Action<List<ItemSlot>> onEvent;
    public void Raised(List<ItemSlot> slots)
    {
        onEvent?.Invoke(slots);
    }
}
