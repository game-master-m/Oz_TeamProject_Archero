
using System.Text;
using UnityEngine;

public static class Utils
{
    private static StringBuilder mBuilder = new StringBuilder(64);

    public static StringBuilder DamageAppend(int value)
    {
        mBuilder.Clear();
        mBuilder.Append("- ");
        mBuilder.Append(value);
        return mBuilder;
    }
    public static StringBuilder DamageAppend(string prefix, int value)
    {
        mBuilder.Clear();
        mBuilder.Append(prefix);
        mBuilder.Append("- ");
        mBuilder.Append(value);
        return mBuilder;
    }
    public static StringBuilder IntAppend(int value)
    {
        mBuilder.Clear();
        mBuilder.Append(value);
        return mBuilder;
    }

    public static void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }


}




