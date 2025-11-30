
using UnityEngine;

public class Utils
{
    public static void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}
public class AnimHash
{
    public static readonly int idle = Animator.StringToHash("Idle");
    public static readonly int move = Animator.StringToHash("Move");


}

public class Layers
{
    public static int GetLayerMask(ELayerName layerName)
    {
        return 1 << (int)layerName;
    }
}

