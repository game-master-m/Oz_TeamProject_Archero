using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDataEvent", menuName = "Archero/EventChannel/Item Data Event Channel")]
public class ItemDataEventChannelSO : ScriptableObject
{
    public event Action<ItemDataSO> onEvent;
    public void Raised(ItemDataSO data)
    {
        onEvent?.Invoke(data);
    }
}
