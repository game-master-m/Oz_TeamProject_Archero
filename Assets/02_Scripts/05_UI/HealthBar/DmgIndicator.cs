using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class DmgIndicator : MonoBehaviour
{
    [Header("Pool & Offset")]
    [SerializeField] private DmgText mTextPrefab;
    [SerializeField] private Vector3 mSpawnOffset = new Vector3(0, 2.0f, 0); // 머리 위 위치 보정값
    [SerializeField] private Vector2 mRandomSpread = new Vector2(0.5f, 0.2f); // 겹침 방지용 랜덤 오프셋 (X, Y)

    private LivingEntity mLivingEntity;

    private void Awake()
    {
        mLivingEntity = GetComponent<LivingEntity>();
    }

    private void Start()
    {
        Managers.Pool.CreatePool(mTextPrefab, 40, Managers.Pool.transform);
    }

    private void OnEnable()
    {
        mLivingEntity.onDmgTaken += SpawnDamageText;
    }

    private void OnDisable()
    {
        mLivingEntity.onDmgTaken -= SpawnDamageText;
    }

    private void SpawnDamageText(float damage, EDmgElement element, bool isCritical = false)
    {
        DmgText textPrefab = Managers.Pool.GetFromPool(mTextPrefab);
        if (textPrefab == null) return;

        // 위치 설정: 내 위치(프리팹 발 밑) + 위로 오프셋 + 약간의 랜덤 확산
        Vector3 randomPos = new Vector3(
            Random.Range(-mRandomSpread.x, mRandomSpread.x),
            Random.Range(-mRandomSpread.y, mRandomSpread.y),
            0f
        );

        textPrefab.transform.position = transform.position + mSpawnOffset + randomPos;

        // 3. 데이터 세팅
        textPrefab.Setup(damage, element, isCritical);
    }
}