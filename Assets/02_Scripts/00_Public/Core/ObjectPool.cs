using System.Collections.Generic;
using UnityEngine;

public interface IPoolTypeCheckable
{
    void EnqueueAfterTypeCheck(MonoBehaviour obj);
    int GetCurrentPoolSize();
    void ReturnAll();
}

public class ObjectPool<T> : IPoolTypeCheckable where T : MonoBehaviour
{
    private T mPrefab;
    private Transform mRoot;

    // 1. 대기열 (나갈 순서)
    private Queue<T> mPoolQueue = new Queue<T>();

    // 2. 활동 명단 (현재 씬에 나가있는 녀석들)
    private List<T> mActiveList = new List<T>();

    // 3. [핵심] 중복 방지 체크용 (이미 풀에 들어와있는지 검사)
    private HashSet<T> mInPoolCheckSet = new HashSet<T>();

    public ObjectPool(T prefab, int initCount, Transform parent = null)
    {
        mPrefab = prefab;
        string name = prefab.name;
        mRoot = new GameObject($"{name}_pool").transform;
        if (parent != null) mRoot.SetParent(parent, false);

        for (int i = 0; i < initCount; i++)
        {
            T inst = CreateInstance();
            mPoolQueue.Enqueue(inst);
            mInPoolCheckSet.Add(inst);
        }
    }

    private T CreateInstance()
    {
        T inst = GameObject.Instantiate(mPrefab, mRoot);
        inst.name = mPrefab.name;
        inst.gameObject.SetActive(false);
        return inst;
    }

    public T Dequeue()
    {
        T inst;
        if (mPoolQueue.Count == 0)
        {
            inst = CreateInstance();
        }
        else
        {
            inst = mPoolQueue.Dequeue();
            mInPoolCheckSet.Remove(inst);
        }

        inst.gameObject.SetActive(true);
        mActiveList.Add(inst);
        return inst;
    }

    public void Enqueue(T item)
    {
        if (item == null) return;

        // [절대 방어] 이미 풀 안에 있는 녀석이면 무시
        if (mInPoolCheckSet.Contains(item))
        {
            return;
        }

        // 활동 명단에 있다면 제거
        if (mActiveList.Contains(item))
        {
            mActiveList.Remove(item);
        }

        item.gameObject.SetActive(false);
        item.transform.SetParent(mRoot);

        mPoolQueue.Enqueue(item);
        mInPoolCheckSet.Add(item); // 풀에 들어왔다고 명단 등록
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

    public void ReturnAll()
    {
        // 리스트가 변경되는 것을 막기 위해 배열로 복사 후 순회
        var activeArray = mActiveList.ToArray();
        mActiveList.Clear(); // 활동 명단은 즉시 초기화

        foreach (var item in activeArray)
        {
            if (item != null)
            {
                // 이렇게 하면 OnDisable에서 ReturnPool을 또 호출해도 안전함
                Enqueue(item);
            }
        }
    }
}