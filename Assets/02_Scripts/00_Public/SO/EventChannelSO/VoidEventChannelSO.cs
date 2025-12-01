using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVoidEvent", menuName = "Archero/EventChannel/Void Event Channel")]
public class IntEventChannelSO : ScriptableObject
{
    public event Action onEvent;
    public void Raised()
    {
        onEvent?.Invoke();
    }

}
