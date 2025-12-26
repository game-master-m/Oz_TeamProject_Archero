using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] private float mRotateSpeed = 2f;
    [SerializeField] private float mFloatingRange = 0.2f;
    [SerializeField] private float mFloatingSpeed = 2f;

    [SerializeField] private ItemType mItemType;
    [SerializeField] private ItemEffect mEffect;

    private Vector3 mStartPos;

    private void OnEnable()
    {
        mStartPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime, Space.World);

        //사인으로 위아래 둥실둥실
        float posY = mStartPos.y + Mathf.Sin(Time.time * mFloatingSpeed) * mFloatingRange;
        transform.position = new Vector3(mStartPos.x, posY, mStartPos.z);
    }
}
