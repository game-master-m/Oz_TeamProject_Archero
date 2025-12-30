using UnityEngine;

public class SnakeBall : EnemyProjectileBase
{
    [Header("웨이브 이동 세팅용")]
    [SerializeField] private float mWaveWidth = 3.0f;
    [SerializeField] private float mWaveFrequency = 16.0f;

    private float localTime;
    private float fixedY;
    private Vector3 startPos;

    public override void Setup(float damage, float speed, Vector3 direction, EnemyBase owner)
    {
        base.Setup(damage, speed, direction, owner);

        startPos = transform.position;
        fixedY = transform.position.y;
        localTime = 0f;
    }

    protected override void Update()
    {
        base.Update();

        localTime += Time.deltaTime;

        float forwardOffset = localTime * mMoveSpeed;
        float sideOffset = Mathf.Sin(localTime * mWaveFrequency) * mWaveWidth;

        Vector3 forwardDir = new Vector3(transform.forward.x, 0f, transform.forward.z);
        Vector3 sideDir = new Vector3(transform.right.x, 0f, transform.right.z);

        Vector3 target = startPos + forwardDir * forwardOffset + sideDir * sideOffset;
        target.y = fixedY;

        transform.position = target;
    }

    protected override void MoveAndRotate()
    {
   
    }
}
