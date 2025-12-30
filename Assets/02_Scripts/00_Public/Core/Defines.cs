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

    // 체력바 UI
    public const string Critical = "Crit! ";

    // 필요 경험치
    public const int RequiredExp = 500;
    public const float NextExpMultiplier = 2.2f;
    public const int GetGoldAmountPerExp = 3;
}

//애니메이터
public class AnimHash
{
    public static readonly int idle = Animator.StringToHash("Idle");
    public static readonly int move = Animator.StringToHash("Move");
    public static readonly int _throw = Animator.StringToHash("Throw");
    public static readonly int attackSpeedMultiplier = Animator.StringToHash("AttackSpeedMultiplier");
    public static readonly int attack = Animator.StringToHash("Attack");
    public static readonly int attackSpin = Animator.StringToHash("AttackSpin");
    public static readonly int attackDown = Animator.StringToHash("AttackDown");
    public static readonly int hit = Animator.StringToHash("Hit");
    public static readonly int death = Animator.StringToHash("Death");
    public static readonly int spawn = Animator.StringToHash("Spawn");
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
    Default, TransparentFX, IgnoreRaycast, Enemy, Water, UI, Player, Obstacle, Projectile, Exp, EnemyAttack, Item
}
public enum EEnemyName { None, Slime }
public enum ESkillGrade { None, Normal, Expert, Epic, Legend }
public enum EDmgElement { Normal, Fire, Lightning, Poison }
public enum EHealthType { None, Player, Enemy, Boss }
public enum EEnemyType { None, Melee, Range, Boss }
public enum ENodeState { Running, Success, Failure }
public enum EProjectileName
{
    SmallFireBall, SmallWindBall, SmallWaterBall, SmallMagicBall, SnakeBall, SplitBall, HomingFireBall
}


