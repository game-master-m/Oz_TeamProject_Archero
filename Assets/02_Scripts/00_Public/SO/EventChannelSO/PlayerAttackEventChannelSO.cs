using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerAttackEvent", menuName = "Archero/EventChannel/PlayerAttack Event Channel")]
public class PlayerAttackEventChannelSO : ScriptableObject
{
    public event Action<PlayerAttack> onEvent;
    public void Raised(PlayerAttack attack)
    {
        onEvent?.Invoke(attack);
    }

}
