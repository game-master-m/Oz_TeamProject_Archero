using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //¹üÀ§ -160 ~ 200
    [SerializeField] private float mMinZpos = -190f;
    [SerializeField] private float mMaxZpos = -176f;
    [SerializeField] private Vector3 mOffset = new Vector3(0f, 0f, -182);
    private Transform mPlayer;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, mPlayer.position.z) + mOffset;
        if (targetPos.z >= mMaxZpos || targetPos.z <= mMinZpos) return;
        transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * 5f);
    }
}
