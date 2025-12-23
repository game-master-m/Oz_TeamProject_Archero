using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float mMoveSpeed = 24f;
    [SerializeField] private ExplodeEffect mExplodeEffectPrefab;
    [SerializeField] private WarningCircleEffect mCircleEffectPrefab;
    private ExplodeEffect mExplodeEffect;
    private WarningCircleEffect mCircleEffect;

    private GameObject mOwner;

    private Vector3 mStartPos;
    private Vector3 mEndPos;
    private Vector3 mHideOffset = new Vector3(0f, -2, 0f);
    private float mJumpHeight;
    private float mBombRange;

    private float mElapsedTime = 0;
    private float mTotalTime = 0;
    private bool mIsJumping = false;

    protected WaitForSeconds mWaitEffect;

    // Update is called once per frame
    void Update()
    {
        if (mIsJumping)
        {
            mElapsedTime += Time.deltaTime;

            Vector3 horizontalPos = Vector3.Lerp(mStartPos, mEndPos, mElapsedTime / mTotalTime);

            float verticalPos = mJumpHeight * 4 * (mElapsedTime / mTotalTime) * (1 - mElapsedTime / mTotalTime);

            transform.position = new Vector3(horizontalPos.x, mStartPos.y + verticalPos, horizontalPos.z);

            if (mElapsedTime >= mTotalTime)
            {
                mIsJumping = false;
                Explode();
            }
        }
    }

    public void SetUp(GameObject owner, float height, float range)
    {
        mOwner = owner;
        mJumpHeight = height;
        mBombRange = range;
        mWaitEffect = new WaitForSeconds(1.5f);

        Managers.Pool.CreatePool(mExplodeEffectPrefab, 3, Managers.Pool.transform);
        Managers.Pool.CreatePool(mCircleEffectPrefab, 3, Managers.Pool.transform);
    }

    //y = 4 * height * (경과시간/전체점프시간) * (1 - 경과시간/전체점프시간)
    public void DoJump(Vector3 target, float height)
    {
        mStartPos = this.gameObject.transform.position;
        mEndPos = target;
        mJumpHeight = height;
        transform.Translate(0, -transform.position.y, 0);

        float horizontalDistance
            = Vector3.Distance(new Vector3(mStartPos.x, 0, mStartPos.z), new Vector3(mEndPos.x, 0, mEndPos.z));

        mTotalTime = horizontalDistance / mMoveSpeed;
        mElapsedTime = 0f;

        mIsJumping = true;

        SetWarningEffect(target);
    }

    private void Explode()
    {
        if (!mOwner.TryGetComponent(out BombFairy fairy)) 
        {
            return;
        }
        var owner = fairy;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, mBombRange, Layers.GetLayerMask(ELayerName.Enemy));

        foreach (Collider hitCollider in hitColliders)
        {
            //비활성화된 적은 패스
            if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;

            if (hitCollider.TryGetComponent(out EnemyBase enemy)) 
            {
                owner.Explode(enemy);
            }
        }

        Managers.Pool.ReturnToPool(mCircleEffect);

        SetExplodeEffect();

        StartCoroutine(EffectCo());

        this.gameObject.transform.position = this.gameObject.transform.position + mHideOffset;
    }

    public void ReturnPool()
    {
        Managers.Pool.ReturnToPool(mExplodeEffect);
        Managers.Pool.ReturnToPool(this);
    }

    private void SetWarningEffect(Vector3 targetPos)
    {
        mCircleEffect = Managers.Pool.GetFromPool(mCircleEffectPrefab);
        mCircleEffect.transform.localScale = Vector3.one * mBombRange * 2f;
        mCircleEffect.gameObject.transform.position
            = new Vector3(targetPos.x, 0.1f, targetPos.z);
    }

    private void SetExplodeEffect()
    {
        Managers.Pool.ReturnToPool(mCircleEffect);

        mExplodeEffect = Managers.Pool.GetFromPool(mExplodeEffectPrefab);
        mExplodeEffect.transform.localScale = Vector3.one * mBombRange;
        mExplodeEffect.gameObject.transform.position
            = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z);

        if (mExplodeEffect != null)
        {
            mExplodeEffect.gameObject.SetActive(true);
            ParticleSystem[] particles = mExplodeEffect.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particles) { ps.Play(); }
        }

        StartCoroutine(EffectCo());
    }

    IEnumerator EffectCo()
    {
        yield return mWaitEffect;

        ReturnPool();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, mBombRange);

        if (mCircleEffect != null) 
        {
            Gizmos.color = Color.green;
            float radius = mCircleEffect.transform.localScale.x * 0.5f;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        if (mExplodeEffect != null)
        {
            Gizmos.color = Color.blue;
            float radius = mExplodeEffect.transform.localScale.x * 0.5f;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}