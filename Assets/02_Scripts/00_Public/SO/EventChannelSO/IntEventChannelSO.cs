using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIntEvent", menuName = "Archero/EventChannel/Int Event Channel")]
public class IntEventChannelSO : ScriptableObject
{
    public event Action<int> onEvent;
    public void Raised(int num)
    {
        onEvent?.Invoke(num);
    }

}
