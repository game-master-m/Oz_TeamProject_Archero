using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIntListEvent", menuName = "Archero/EventChannel/Int List Event Channel")]
public class IntListEventChannelSO : ScriptableObject
{
    public event Action<int, List<int>> onEvent;
    public void Raised(int num, List<int> intList)
    {
        onEvent?.Invoke(num, intList);
    }

}