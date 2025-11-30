using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVoidEvent", menuName = "Archero/EventChannel/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
    public event Action onEvent;
    public void Raised()
    {
        onEvent?.Invoke();
    }

}
