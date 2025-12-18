using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffect : MonoBehaviour
{
    private LineRenderer mLineRenderer;
    private WaitForSeconds mWaitForSeconds = new WaitForSeconds(0.2f);
    private Vector3 mOffset = new Vector3(0f, 1.0f, 0f);

    public void SetLineRenderer(float laserRadius)
    {
        mLineRenderer = GetComponent<LineRenderer>();
        mLineRenderer.startWidth = laserRadius;
        mLineRenderer.endWidth = laserRadius;
        mLineRenderer.startColor = Color.cyan;
        mLineRenderer.endColor = Color.white;
    }

    public void DrawLaser(Vector3 start, Vector3 end)
    {
        mLineRenderer.positionCount = 2;

        mLineRenderer.SetPosition(0, start);
        mLineRenderer.SetPosition(1, end);
    }
}

