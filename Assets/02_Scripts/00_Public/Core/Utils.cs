
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Utils
{
    private static StringBuilder mBuilder = new StringBuilder(64);
    private static readonly string[] mSuffixes = { "", "k", "m", "B", "T" };

    private static Dictionary<float, WaitForSeconds> mWaitCache = new Dictionary<float, WaitForSeconds>();
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
    public static StringBuilder ShortenIntSlashInt(int left, int right)
    {
        mBuilder.Clear();
        ShortenInt(left);
        mBuilder.Append("/");
        ShortenInt(right);
        return mBuilder;
    }
    public static StringBuilder ShortenIntAppend(int value)
    {
        mBuilder.Clear();
        return ShortenInt(value);
    }
    public static StringBuilder ShortenInt(int value)
    {
        if (value < 10000)
        {
            mBuilder.Append(value.ToString("N0"));
            return mBuilder;
        }
        double val = value;
        int index = 0;
        while (val >= 1000 && index < mSuffixes.Length - 1)
        {
            val /= 1000.0;
            index++;
        }
        if (val >= 100) mBuilder.Append(val.ToString("F0"));
        else if (val >= 10) mBuilder.Append(val.ToString("F1"));
        else mBuilder.Append(val.ToString("F2"));
        mBuilder.Append(mSuffixes[index]);
        return mBuilder;
    }

    public static WaitForSeconds GetWaitForSeconds(float waitTime)
    {
        if (!mWaitCache.TryGetValue(waitTime, out var wait))
        {
            wait = new WaitForSeconds(waitTime);
            mWaitCache.Add(waitTime, wait);
        }
        return wait;
    }
    public static void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }


}




