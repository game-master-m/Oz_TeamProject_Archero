using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
    [Header("EffectBase ÂüÁ¶")]
    [SerializeField] protected float mLifeTime;
    [SerializeField] protected float mSizeMultiplier = 1.0f;
    [SerializeField] protected float mPlayBackSpeed = 1.0f;

    protected ParticleSystem[] mParticles;
    protected float mTimer = 0.0f;

    protected virtual void Awake()
    {
        mParticles = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem p in mParticles)
        {
            var main = p.main;
            main.startSizeMultiplier = mSizeMultiplier;
            main.simulationSpeed = mPlayBackSpeed;
        }
    }
    public virtual void SetSize(float sizeMultiplier)
    {
        foreach (ParticleSystem p in mParticles)
        {
            var main = p.main;
            main.startSizeMultiplier = sizeMultiplier;
        }
    }
    public virtual void SetSpeed(float playBackSpeed)
    {
        foreach (ParticleSystem p in mParticles)
        {
            var main = p.main;
            main.simulationSpeed = playBackSpeed;
        }
    }
    public virtual void Setup(Vector3 spawnPos, Quaternion rotation)
    {
        transform.position = spawnPos;
        transform.rotation = rotation;

        mTimer = 0.0f;
    }
    public virtual void Play()
    {
        gameObject.SetActive(true);
        foreach (ParticleSystem p in mParticles)
        {
            p.Play();
        }
    }
    public virtual void Stop()
    {
        foreach (ParticleSystem p in mParticles)
        {
            p.Stop();
        }
        gameObject.SetActive(false);
    }
    protected virtual void FixedUpdate()
    {
        mTimer += Time.fixedDeltaTime;
        if (mTimer > mLifeTime)
        {
            mTimer = 0.0f;
            Stop();
            ReturnToPool();
        }
    }

    protected virtual void ReturnToPool()
    {
        Managers.Pool.ReturnToPool(this);
    }
    public virtual void ExecuteEffect()
    {
        Stop();
        ReturnToPool();
    }
}
