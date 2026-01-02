using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemTypeEvent", menuName = "Archero/EventChannel/ItemType Event Channel")]
public class ItemTypeEventChannelSO : ScriptableObject
{
    public event Action<EItemType> onEvent;
    public void Raised(EItemType type)
    {
        onEvent?.Invoke(type);
    }
}
