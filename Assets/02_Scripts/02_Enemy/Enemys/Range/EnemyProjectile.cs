using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float mSpeed = 10f;
    private float mDamage;
    private Vector3 mDirection;

    public void Fire(Vector3 targetPos, float damage)
    {
        mDamage = damage;
        mDirection = (targetPos - transform.position).normalized;
    }

    private void Update()
    {
        transform.position += mDirection * mSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // other.GetComponent<LivingEntity>()?.TakeDamage(mDamage);
            Managers.Pool.ReturnToPool(this);
        }
    }
}
