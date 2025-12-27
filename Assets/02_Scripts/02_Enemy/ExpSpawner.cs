using System.Collections;
using UnityEngine;

public class ExpSpawner : MonoBehaviour
{
    [SerializeField] private ExpPrefab mExpPrefab;

    private int mMaxSpawnCount;
    private readonly float mLifeTime = 0.8f;
    private Transform mTarget;

    private float mTimer = 0.0f;

    //ÄÚ·çÆ¾ Ä³½Ì
    private readonly float mSpawnDelay = 0.05f;
    private WaitForSeconds mWaitSpawnDelay;


    private void Awake()
    {
        Managers.Pool.CreatePool(mExpPrefab, 60, Managers.Pool.transform);
        mWaitSpawnDelay = new WaitForSeconds(mSpawnDelay);
    }
    private void OnEnable()
    {
        mTimer = 0.0f;
    }
    private void FixedUpdate()
    {
        mTimer += Time.fixedDeltaTime;
        if (mTimer > mLifeTime)
        {
            mTimer = 0.0f;
            StopAllCoroutines();
            Managers.Pool.ReturnToPool(this);
        }
    }
    public void SetupExpSpawn(int maxSpawnCount, Transform enemyOrigin, Transform target)
    {
        mMaxSpawnCount = maxSpawnCount;
        mTarget = target;

        transform.position = enemyOrigin.position;

        StartCoroutine(SpawnExp());
    }

    private IEnumerator SpawnExp()
    {
        yield return mWaitSpawnDelay;

        int minDropCount = Mathf.CeilToInt(mMaxSpawnCount / 2.0f);
        int randomDropCount = Random.Range(minDropCount, mMaxSpawnCount);
        int dropRadius = 1 + Mathf.RoundToInt(randomDropCount / 3);

        for (int i = 0; i < randomDropCount; i++)
        {
            ExpPrefab exp = Managers.Pool.GetFromPool(mExpPrefab);
            Vector2 randomePos = Random.insideUnitCircle * dropRadius;
            Vector3 spawnOffset = new Vector3(randomePos.x, 0.5f, randomePos.y);
            Vector3 spawnPos = transform.position + spawnOffset;
            exp.transform.position = transform.position;
            exp.SetTargetAndDestination(mTarget, spawnPos);
            yield return mWaitSpawnDelay;
        }
    }

}
