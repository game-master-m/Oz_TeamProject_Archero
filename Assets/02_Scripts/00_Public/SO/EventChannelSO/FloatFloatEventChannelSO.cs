using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatFloatEvent", menuName = "Archero/EventChannel/FloatFloat Event Channel")]
public class FloatFloatEventChannelSO : ScriptableObject
{
    public event Action<float, float> onEvent;
    public void Raised(float left, float right)
    {
        onEvent?.Invoke(left, right);
    }

}
