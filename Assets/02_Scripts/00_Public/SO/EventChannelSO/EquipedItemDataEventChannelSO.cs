using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipedItemDataEvent", menuName = "Archero/EventChannel/Equiped ItemData Event Channel")]
public class EquipedItemDataEventChannelSO : ScriptableObject
{
    public event Action<Dictionary<EItemType, ItemDataSO>> onEvent;
    public void Raised(Dictionary<EItemType, ItemDataSO> itemData)
    {
        onEvent?.Invoke(itemData);
    }
}
