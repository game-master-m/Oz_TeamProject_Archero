using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    private LineRenderer mLineRenderer;
    private WaitForSeconds mWaitForSeconds = new WaitForSeconds(0.2f);
    private Vector3 mOffset = new Vector3(0f, 1.0f, 0f);

    public void SetLineRenderer()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        mLineRenderer.startWidth = 0.5f;
        mLineRenderer.endWidth = 0.5f;
        mLineRenderer.startColor = Color.cyan;
        mLineRenderer.endColor = Color.white;
    }

    public void DrawLightning(Vector3 start, Vector3 end, int segments = 10, float effectStrength = 0.8f)
    {
        SetLineRenderer();
        mLineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float temp = i / segments;
            Vector3 point = Vector3.Lerp(start, end, temp);

            point += Random.insideUnitSphere * effectStrength;

            mLineRenderer.SetPosition(i, point + mOffset);
        }

        StartCoroutine(ReturePoolCo());
    }

    IEnumerator ReturePoolCo() 
    {
        yield return mWaitForSeconds;
        Managers.Pool.ReturnToPool(this);
    }
}
