using UnityEngine;

public class ThrowState : PlayerState
{
    public ThrowState(PlayerController player, IState parent = null) : base(player, parent)
    {
    }

    public override void Enter()
    {
        Utils.Log("Throw Enter");
        mPlayer.Anim.CrossFade(AnimHash._throw, 0.1f);

    }
}
