using System.Collections.Generic;
using UnityEngine;

public interface IPoolTypeCheckable
{
    void EnqueueAfterTypeCheck(MonoBehaviour obj);
    int GetCurrentPoolSize();
}
public class ObjectPool<T> : IPoolTypeCheckable where T : MonoBehaviour
{
    private T mPrefab;
    private Queue<T> mPoolQueue = new Queue<T>();
    public Transform root;

    public ObjectPool(T prefab, int initCount, Transform parent = null)
    {
        this.mPrefab = prefab;
        string name = prefab.name;
        root = new GameObject($"{name}_pool").transform;
        if (parent != null) root.SetParent(parent, false);

        for (int i = 0; i < initCount; i++)
        {
            T inst = GameObject.Instantiate(prefab, root);
            inst.name = prefab.name;
            inst.gameObject.SetActive(false);
            mPoolQueue.Enqueue(inst);
        }
    }

    public T Dequeue()
    {
        if (mPoolQueue.Count == 0)
        {
            T instance = GameObject.Instantiate(mPrefab, root);
            instance.name = mPrefab.name;
            return instance;
        }
        T inst = mPoolQueue.Dequeue();
        inst.gameObject.SetActive(true);
        return inst;
    }

    public void Enqueue(T prefab)
    {
        if (prefab == null) return;
        prefab.gameObject.SetActive(false);
        mPoolQueue.Enqueue(prefab);
    }

    public void EnqueueAfterTypeCheck(MonoBehaviour obj)
    {
        if (obj is T typeObj)
        {
            Enqueue(typeObj);
        }
    }

    public int GetCurrentPoolSize()
    {
        return mPoolQueue.Count;
    }
}
