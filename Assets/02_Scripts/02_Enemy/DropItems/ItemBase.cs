using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public ItemDataSO ItemDataSO;

    [SerializeField] private float mRotateSpeed = 2f;
    [SerializeField] private float mFloatingRange = 0.2f;
    [SerializeField] private float mFloatingSpeed = 2f;

    private Vector3 mStartPos;
    private float mLifeTime = 0;

    private void OnEnable()
    {
        mStartPos = transform.position;
        mLifeTime = 0;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime, Space.World);

        //사인으로 위아래 둥실둥실
        float posY = mStartPos.y + Mathf.Sin(Time.time * mFloatingSpeed) * mFloatingRange;
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);

        mLifeTime += Time.deltaTime;

        if (mLifeTime >= 20) 
        {
            ReturnPool();
        }
    }

    public virtual void ReturnPool() { }
}
