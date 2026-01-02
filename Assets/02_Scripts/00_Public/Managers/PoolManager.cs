using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<string, IPoolTypeCheckable> mPools = new Dictionary<string, IPoolTypeCheckable>();

    public void CreatePool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) return;
        string key = prefab.name;
        if (mPools.ContainsKey(key)) return;
        mPools.Add(key, new ObjectPool<T>(prefab, initCount, parent));
    }

    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null) return null;
        string key = prefab.name;
        if (!mPools.TryGetValue(key, out var box)) return null;
        var pool = box as ObjectPool<T>;
        if (pool == null) return null;
        return pool.Dequeue();
    }

    public void ReturnToPool<T>(T instance) where T : MonoBehaviour
    {
        if (instance == null) return;
        if (!mPools.ContainsKey(instance.name))
        {
            Destroy(instance.gameObject);
            return;
        }

        mPools[instance.name].EnqueueAfterTypeCheck(instance);
    }

    public void ReturnAllObjects()
    {
        foreach (var pool in mPools.Values)
        {
            pool.ReturnAll();
        }
    }
}
