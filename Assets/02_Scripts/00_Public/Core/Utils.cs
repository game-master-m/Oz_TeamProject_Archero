
using System.Text;
using UnityEngine;

public static class Utils
{
    private static StringBuilder mBuilder = new StringBuilder(64);
    private static readonly string[] mSuffixes = { "", "k", "m", "B", "T" };
    public static StringBuilder DamageAppend(int value)
    {
        mBuilder.Clear();
        mBuilder.Append("- ");
        mBuilder.Append(value);
        return mBuilder;
    }
    public static StringBuilder StringAppend(string value)
    {
        mBuilder.Clear();
        return mBuilder.Append(value);
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
    public static StringBuilder LevelIntAppend(int value)
    {
        mBuilder.Clear();
        mBuilder.Append("Lv.");
        mBuilder.Append(value);
        return mBuilder;
    }
    public static StringBuilder IntSlashInt(int left, int right)
    {
        mBuilder.Clear();
        mBuilder.Append(left);
        mBuilder.Append("/");
        mBuilder.Append(right);
        return mBuilder;
    }
    public static StringBuilder GoldIntAppend(int value)
    {
        mBuilder.Clear();

        if (value < 1000)
        {
            mBuilder.Append(value);
            return mBuilder;
        }
        double val = value;
        int index = 0;
        while (val >= 1000 && index < mSuffixes.Length - 1)
        {
            val /= 1000.0;
            index++;
        }

        mBuilder.Append(val.ToString("F1")).Append(mSuffixes[index]);
        return mBuilder;
    }

    public static void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }


}




