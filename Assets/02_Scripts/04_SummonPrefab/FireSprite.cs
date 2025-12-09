using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSprite : MonoBehaviour
{
    [SerializeField] private Projectile mProjectilePrefab;
    [SerializeField] private Vector3 mProjectileOffeset = new Vector3(0, 1.0f, 0);

    private List<IProjectileStrategy> mArrowStrategies = new List<IProjectileStrategy>();

    [SerializeField] private float mTargetRange = 30.0f;
    [SerializeField] private Vector3 mTargetOffset = new Vector3(0, 0, 0);

    [SerializeField] private Vector3 mPositionOffset = new Vector3(1.5f, 1.0f, 0);

    private float mAttackDamage;
    private float mAttackSpeed;
    private WaitForSeconds mWaitAttack;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Define.Tag_Player);

        SetUp(player.GetComponent<PlayerAttack>(), 1);
    }

    public void SetUp(PlayerAttack attack, int spriteCount) 
    {
        //위치 설정
        this.gameObject.transform.position = attack.gameObject.transform.position + mPositionOffset;
        this.gameObject.transform.RotateAround(transform.position, Vector3.up, mTargetRange * spriteCount);

        //스텟 받아오기
        mAttackSpeed = attack.gameObject.GetComponent<PlayerStat>().AttackSpeed;
        mAttackDamage = attack.gameObject.GetComponent<PlayerStat>().AttackDamage * 0.4f;
        mWaitAttack = new WaitForSeconds(mAttackSpeed);

        //공격 시작
        StartCoroutine(AttackCo());
    }

    //Projectile과 거의 같음
    public bool LookTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, mTargetRange, Layers.GetLayerMask(ELayerName.Enemy));

        if (hitColliders.Length == 0)
        {
            Utils.Log("주변에 적이 없습니다.");
            return false;
        }

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        Collider nearCol = null;

        foreach (Collider hitCollider in hitColliders)
        {
            //비활성화된 적은 패스
            if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;

            Vector3 targetDir = hitCollider.transform.position - currentPosition;
            float distanceToTarget = targetDir.sqrMagnitude;

            //적이 겹쳐있어 거리가 매우 가까울 때 벡터연산 오류 방지
            if (distanceToTarget < 0.001f) continue;

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestEnemy = hitCollider.transform;
                nearCol = hitCollider;
            }
        }

        if (nearCol == null)
        {
            Utils.Log("맞은 적 외 주변에 적이 없습니다.");
            return false;
        }

        transform.LookAt(closestEnemy.position + mTargetOffset, Vector3.up);
      
        return true;
    }

    //발사체 생성
    public void MakeProjectile()
    {
        Projectile projectile = Managers.Pool.GetFromPool(mProjectilePrefab);
        if (projectile != null)
        {
            projectile.transform.position = transform.position + mProjectileOffeset;
            projectile.Setup(mArrowStrategies, mAttackDamage);
        }
    }

    //공격 코루틴
    IEnumerator AttackCo() 
    {
        while (true) 
        {
            if (LookTarget())
            {
                MakeProjectile();
            }

            yield return mWaitAttack;
        }
    }
}
