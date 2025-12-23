using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyAttackCol : MonoBehaviour
{
    private float mTailAttackDmg;
    private CapsuleCollider mCapsuleCollider;
    private Rigidbody mRigidbody;
    private void Awake()
    {
        mCapsuleCollider = GetComponent<CapsuleCollider>();
        mRigidbody = GetComponent<Rigidbody>();
        mRigidbody.isKinematic = true;
        mCapsuleCollider.enabled = false;
    }

    public void StartAttack()
    {
        mCapsuleCollider.enabled = true;
    }

    public void EndAttack()
    {
        mCapsuleCollider.enabled = false;
    }

    public void SetUpDmg(float dmg)
    {
        mTailAttackDmg = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        Utils.Log("EnemyAttackCol TriggerEnter 진입");
        if (other.CompareTag(Define.Tag_Player))
        {
            var player = other.gameObject.GetComponent<LivingEntity>();
            if (player != null)
            {
                player.TakeDamage(mTailAttackDmg);
            }
            else
            {
                Utils.Log("플레이어가 널임");
            }
            EndAttack();
        }
    }
}
