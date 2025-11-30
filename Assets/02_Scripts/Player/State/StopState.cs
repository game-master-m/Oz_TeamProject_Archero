
using UnityEngine;
public class StopState : PlayerState
{
    public StopState(PlayerController player, IState parent = null) : base(player, parent)
    {
    }
    public override void Enter()
    {
        Utils.Log("Stop Enter");
        player.Anim.CrossFade(AnimHash.idle, 0.1f);
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }

}
