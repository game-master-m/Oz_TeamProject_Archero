using UnityEngine;

public class FireTrail : EffectBase
{
    [Header("FireTrail ÂüÁ¶")]
    [SerializeField] private float mMaxSizeMultiplier = 3.0f;

    protected override void FixedUpdate()
    {
        mTimer += Time.fixedDeltaTime;

        if (mTimer < mMaxSizeMultiplier)
        {
            foreach (var p in mParticles)
            {
                var main = p.main;
                main.startSizeMultiplier = mTimer;
            }
        }
    }
}
