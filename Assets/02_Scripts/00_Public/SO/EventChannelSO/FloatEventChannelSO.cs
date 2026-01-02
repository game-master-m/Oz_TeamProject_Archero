using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatEvent", menuName = "Archero/EventChannel/Float Event Channel")]
public class FloatEventChannelSO : ScriptableObject
{
    public event Action<float> onEvent;
    public void Raised(float amount)
    {
        onEvent?.Invoke(amount);
    }

}
