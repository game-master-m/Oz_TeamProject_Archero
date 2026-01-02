using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInt*3Event", menuName = "Archero/EventChannel/Int * 3 Event Channel")]
public class IntTripleEventChannelSO : ScriptableObject
{
    public event Action<int, int, int> onEvent;
    public void Raised(int num1, int num2, int num3)
    {
        onEvent?.Invoke(num1, num2, num3);
    }

}
