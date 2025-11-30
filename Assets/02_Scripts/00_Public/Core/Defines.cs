using UnityEngine;

public static class Define
{
    // Tags
    public const string Tag_Player = "Player";
    public const string Tag_Enemy = "Enemy";
    public const string Tag_Projectile = "Projectile";
    public const string Tag_Obstacle = "Obstacle";

    // Scenes
    public const string Scene_Lobby = "Lobby_Temp";
    public const string Scene_Stage = "Stage_Temp";
}

//애니메이터
public class AnimHash
{
    public static readonly int idle = Animator.StringToHash("Idle");
    public static readonly int move = Animator.StringToHash("Move");
}

//레이어
public class Layers
{
    public static int GetLayerMask(ELayerName layerName)
    {
        return 1 << (int)layerName;
    }
}

//Enums
public enum ELayerName
{
    Default, TransparentFX, IgnoreRaycast, Enemy, Water, UI, Player, Obstacle
}


