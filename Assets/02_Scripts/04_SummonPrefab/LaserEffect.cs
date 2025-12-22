using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffect : MonoBehaviour
{
    private LineRenderer mLineRenderer;
    private int mSegmentCount = 20;

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

    public void DrawLaser(List<Transform> pointLlist)
    {
        if (pointLlist == null || pointLlist.Count < 2) return;
        List<Vector3> curvePoints = GenerateCatmullRomCurve(pointLlist, mSegmentCount);

        mLineRenderer.positionCount = curvePoints.Count;
        mLineRenderer.SetPositions(curvePoints.ToArray());
    }

    private List<Vector3> GenerateCatmullRomCurve(List<Transform> pointList, int segmentCount) 
    {
        List<Vector3> result = new List<Vector3>();
        int count = pointList.Count;

        for (int i = 0; i < count - 1; i++) 
        {
            Vector3 p0 = pointList[Mathf.Clamp(i - 1,0,count - 1)].position;
            Vector3 p1 = pointList[Mathf.Clamp(i, 0, count - 1)].position;
            Vector3 p2 = pointList[Mathf.Clamp(i + 1, 0, count - 1)].position;
            Vector3 p3 = pointList[Mathf.Clamp(i + 2, 0, count - 1)].position;

            for (int k = 0; k < segmentCount; k++) 
            {
                float t = k / (float)segmentCount;
                result.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        return result;
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) 
    {
        float t2 = t*t;
        float t3 = t*t2;

        return 0.5f * (
            (2f * p1)
            + (-p0 + p2) * t
            + (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2
            + (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }
}

