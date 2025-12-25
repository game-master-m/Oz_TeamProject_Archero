using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyBase : LivingEntity
{
    [Header("Enemy Base 참조")]
    [SerializeField] protected EnemyStatDataSO mStatData;
    [SerializeField] private ExpPrefab mExpPrefab;
    [SerializeField] private EEnemyType mEnemyType = EEnemyType.Melee;

    [Header("개인 드랍 아이템 리스트")]
    [SerializeField] private List<DropItemData> mDropItems;

    [Header("공통 드랍 아이템 테이블")]
    [SerializeField] private DropTableSO mCommonDropTable;

    protected Animator mAnim;
    protected NavMeshAgent mAgent;
    protected CapsuleCollider mCapsuleCollider;
    protected StateMachine mStateMachine;

    protected float mAttackDamage;
    protected float mAttackRange;
    protected float mAttackSpeed;
    protected float mRotateSpeed;
    protected float mDetectRange;

    //프리팹들이 z축을 바라보고 있지 않아서 각 에너미 StatDataSO에 보정 회전값 설정
    protected Vector3 mRotationOffset;
    protected Quaternion mCorrectionQtrn;

    public float AttackRange => mAttackRange;
    public float RotateSpeed => mRotateSpeed;
    public float AttackSpeed => mAttackSpeed;
    public float AttackDamage => mAttackDamage;
    public float DetectRange => mDetectRange;
    public Quaternion CorrectionQtrn => mCorrectionQtrn;
    public NavMeshAgent Agent => mAgent;
    public EEnemyType EEnemyType => mEnemyType;

    //행동트리에서 사용할 데이터 묶음
    public BlackBoard Board { get; private set; }

    //플레이어 추적용 타겟
    protected Transform mTarget;

    public event Action<EnemyBase> onEnemyDie;
    public Animator Anim => mAnim;
    public Transform Target => mTarget;

    protected virtual void Awake()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();

        mCapsuleCollider = GetComponent<CapsuleCollider>();
        mCapsuleCollider.isTrigger = true;

        // NavMeshAgent 세팅 (속도, 회전 등)
        mAgent.updateRotation = false;

        mStateMachine = new StateMachine();

        //BlackBoard 컴포넌트 주입
        Board = new BlackBoard();

        Managers.Pool.CreatePool(mExpPrefab, 60, Managers.Pool.transform);
        if (mDropItems != null)
        {
            foreach (var drop in mDropItems)
            {
                if (drop != null && drop.itemPrefab != null)
                    Managers.Pool.CreatePool(drop.itemPrefab, 10, Managers.Pool.transform);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // 부모의 체력 초기화 실행

        if (mCapsuleCollider != null) mCapsuleCollider.enabled = true;

        // 적이 다시 살아날 때(풀링) 필요한 초기화
        if (mAgent != null)
        {
            mAgent.enabled = false; // NavMeshAgent 비활성화, StageManager에서 활성화 처리
        }
    }
    protected virtual void OnDisable()
    {
        ResetAgentSetting();
        mCapsuleCollider.enabled = false;
    }
    protected virtual void OnDestroy()
    {
        ResetAgentSetting();
    }
    protected virtual void Update()
    {
        mStateMachine?.Update();
    }
    protected virtual void FixedUpdate()
    {
        mStateMachine?.FixedUpdate();
    }

    private void ResetAgentSetting()
    {
        mTarget = null;
        if (mAgent != null)
        {
            if (mAgent.isOnNavMesh)
            {
                mAgent.velocity = Vector3.zero;
                mAgent.isStopped = true;
                mAgent.enabled = false;
            }
        }
    }
    public virtual void InitStats(EnemyStatDataSO data)
    {
        base.Init(data.MaxHP);
        mAttackDamage = data.AttackDamage;
        mAttackRange = data.AttackRange;
        mAttackSpeed = data.AttackSpeed;
        mRotateSpeed = data.RotateSpeed;
        mDetectRange = data.DetectRange;
        mRotationOffset = data.RotateOffset;

        //속도는 NavMesh Agent에 직접 설정
        mAgent.speed = data.MoveSpeed;

        //보정 회전값 설정
        mCorrectionQtrn = Quaternion.Euler(mRotationOffset);
    }

    //StageManager.cs에서 생성할 때 호출해서 플레이어 Transform을 주입해줌
    public virtual void SetTarget(Transform target)
    {
        mTarget = target;
        Board.Target = target;
    }

    public override void Die()
    {
        // 1. 움직임 멈춤
        if (mAgent.isOnNavMesh)
        {
            mAgent.isStopped = true;
        }
        mAgent.enabled = false;

        // 2. 콜라이더 끄기 (시체에 공격 안 막히게)
        mCapsuleCollider.enabled = false;

        // 3. 죽음 방송~
        onEnemyDie?.Invoke(this);

        // 4. 경험치 드랍
        ExpPrefab exp = Managers.Pool.GetFromPool(mExpPrefab);
        exp.transform.position = transform.position + Vector3.up * 0.5f;
        exp.SetTarget(mTarget);

        // 5. 아이템 드랍
        DropItems();

        // 6. StageManager에게 알리기 (필요시 이벤트나 매니저 호출)

        // 7. 애니메이션 재생 후 풀로 반환(각 DeathState에서 제어하자)
        //Managers.Pool.ReturnToPool(this);
        base.Die();
    }

    #region 드랍
    private void DropItems()
    {
        foreach (var drop in mDropItems)
        {
            TryDrop(drop);
        }

        if (mCommonDropTable != null)
        {
            foreach (var drop in mCommonDropTable.commonDrops)
            {
                TryDrop(drop);
            }
        }
    }
    private void TryDrop(DropItemData drop)
    {
        if (drop == null || drop.itemPrefab == null) return;

        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand <= drop.dropChance)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitSphere * 2f;
            Vector3 dropPos = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
            ItemBase item = Managers.Pool.GetFromPool(drop.itemPrefab);
            item.transform.position = transform.position + Vector3.up * 0.5f;
            item.gameObject.SetActive(true);
        }
    }
    #endregion

    #region 넉백
    public void KnockBack(Vector3 attackerPos, float force, float duration = 0.3f)
    {
        // 1. 넉백 방향 계산
        Vector3 dir = (transform.position - attackerPos).normalized;

        // 2. 에이전트 잠시 끄기
        if (mAgent != null) mAgent.enabled = false;

        // 3. 넉백 코루틴 실행
        StartCoroutine(KnockBackCo(dir, force, duration));
    }

    private IEnumerator KnockBackCo(Vector3 dir, float force, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + dir * force;
        endPos.y = startPos.y;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 마지막 부분 Mathf는 이징 함수라고 함
            // > 처음엔 느리다가 중간에 빠르고 마지막에 다시 느려짐
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.Sin(t * Mathf.PI * 0.5f));

            yield return null;
        }

        mAgent.enabled = true;
        bool isOnNavMesh = false;
        while (!isOnNavMesh)
        {
            if (mAgent.isOnNavMesh)
            {
                mAgent.isStopped = false;
                isOnNavMesh = true;
            }
            yield return null;
        }
    }
    #endregion

    #region 헬퍼함수
    protected bool CheckInDistance(Transform target, float distance)
    {
        float distSqr = distance * distance;

        if ((target.position - transform.position).sqrMagnitude <= distSqr)
        {
            return true;
        }
        return false;
    }
    public void SetMoveSpeed(float multiplier)
    {
        mAgent.speed = mStatData.MoveSpeed * multiplier;
    }
    public void LookAtDiretion(Vector3 moveDir)
    {
        LookAtDiretion(moveDir, RotateSpeed);
    }
    public void LookAtDiretion(Vector3 moveDir, float rotateSpeed)
    {
        moveDir.y = 0;

        // velocity가 0이면 회전하지 않도록 체크
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(moveDir.normalized);
            //회전값 보정(곱하는 순서가 중요, Forward까지의 회전값 * 보정 회전값)
            Quaternion targetRot = lookRot * CorrectionQtrn;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }
    #endregion
}
