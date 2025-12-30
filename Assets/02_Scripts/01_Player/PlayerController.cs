using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Inspector
    #endregion

    #region component
    private PlayerStat mStat;
    private Animator mAnim;
    private CharacterController mCharacterController;
    private PlayerAttack mAttack;
    #endregion

    #region States
    private StateMachine mStateMachine;

    private StopState mStopState;
    private MoveState mMoveState;
    private ThrowState mThrowState;
    private DeathState mDeathState;
    #endregion

    #region Private Member
    //이동 관련
    private float mCurrentSpeedSqr;
    private Vector2 mInputDir;
    private Vector3 mMoveDir;

    //코루틴 관련
    private WaitForSeconds mZeroDotOneWait;
    private readonly float mZeroDotOneDelay = 0.1f;
    //StopState에서 돌고 있는 코루틴을 받을 변수

    //Collider배열을 계속 생성하면 성능에 안 좋기 때문에, 범위안에 적들이 있는지만 체크하는 Collider Buffer
    private Collider[] mEnemyColBuffer = new Collider[30];
    #endregion

    #region Properties
    public Animator Anim => mAnim;
    public PlayerAttack Attack => mAttack;
    public PlayerStat Stat => mStat;
    public CharacterController CharacterController => mCharacterController;
    public bool CanMove { get; set; } = true;
    public Vector2 InputDir => mInputDir;
    public WaitForSeconds ZeroDotWait => mZeroDotOneWait;

    public bool IsFindEnemy { get; set; } = false; //StopState 코루틴에서 변경
    public Transform CurrentClosestEnemy { get; private set; } = null;
    public Coroutine CheckEnemyInRangeCo { get; set; }
    public Coroutine RotateToTargetCo { get; set; }

    public GameObject EnemyMarker;
    #endregion
    private void Awake()
    {
        //캐싱
        mAnim = GetComponent<Animator>();
        mCharacterController = GetComponent<CharacterController>();
        mCharacterController.enabled = true;

        mStat = GetComponent<PlayerStat>();
        mAttack = GetComponent<PlayerAttack>();
        mZeroDotOneWait = new WaitForSeconds(mZeroDotOneDelay);

        //States
        mStateMachine = new StateMachine();

        mStopState = new StopState(this);
        mMoveState = new MoveState(this);
        mDeathState = new DeathState(this);

        //ThrowState의 부모로 StopState를 설정하면, StopState -> 다른State 전환조건을 ThrowState도 같이 가짐
        //(예, StopState -> MoveState, () => speed > 0.01f 의 조건으로 ThrowState -> MoveState 으로 전환 됨)
        mThrowState = new ThrowState(this, mStopState);

        //상태전환 조건들
        InitTransitions();

        //적 마커
        EnemyMarker = Instantiate(EnemyMarker);
        EnemyMarker.SetActive(false);
    }
    void Start()
    {
        mAttack.InitStat(mStat);
        mStateMachine.ChangeState(mStopState);
    }

    void Update()
    {
        //테스트용 키보드 입력 -> 조이스틱 입력으로 바꿔야 함
        Inputs();
        Movements(mMoveDir);
        mStateMachine.Update();
        MoveEnemyMarker(GetClosestEnemyInRange());
    }
    private void FixedUpdate()
    {
        mStateMachine.FixedUpdate();
    }
    #region Transitions
    private void InitTransitions()
    {
        //Any
        //본인 State에서 본인 State로 계속 넘어가기 때문에 변수추가
        mStateMachine.AddAnyTransition(mDeathState, () => Stat.IsDead && !mStateMachine.IsCurrentState(mDeathState));

        //Stop에서 전환
        mStateMachine.AddTransition(mStopState, mMoveState, () => mCurrentSpeedSqr > 0.01f);
        mStateMachine.AddTransition(mStopState, mThrowState, () => IsFindEnemy && !mStateMachine.IsCurrentState(mThrowState));

        //Move에서 전환
        mStateMachine.AddTransition(mMoveState, mStopState, () => mCurrentSpeedSqr < 0.01f);

        //attack
        mStateMachine.AddTransition(mThrowState, mStopState, () => !IsFindEnemy || Attack.IsAutoTurret);
        //move2
        //mStateMachine.AddTransition(mMoveState, mThrowState, () => mCurrentSpeedSqr < 0.01f);


    }
    #endregion

    #region Input & Move
    private void Inputs()
    {
        //나중에 조이스틱 입력으로 x, z만 바꾸면 됨
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        mInputDir = new Vector2(x, z).normalized;
        mMoveDir = new Vector3(x, 0.0f, z).normalized;
    }
    private void Movements(Vector3 moveDir)
    {
        //피격이나 움직이지 못 하는 경우, CanMove를 false로 바꿔준다.
        //예) Hurt상태일 동안 CanMove = false, HurtState.Exit()에서 true로 바꿔줌
        if (!CanMove) return;
        if (transform.position.y > 0.1f)
        {
            moveDir += Vector3.down * 9.8f;
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        }
        else
        {
            moveDir.y = 0.0f;
        }
        mCharacterController.Move(moveDir * Stat.MoveSpeed * Time.deltaTime);
        mCurrentSpeedSqr = mCharacterController.velocity.sqrMagnitude;
        if (mCurrentSpeedSqr > 0.01f) RotateToMoveDir(moveDir);
    }
    private void RotateToMoveDir(Vector3 moveDir)
    {
        if (moveDir == Vector3.zero) return;
        Vector3 rotateDir = moveDir;
        rotateDir.y = 0.0f;
        Quaternion lookRot = Quaternion.LookRotation(rotateDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * mStat.RotateSpeed);
    }
    #endregion

    private Transform GetClosestEnemyInRange()
    {
        Transform closestEnemy = null;

        //OverlapShepreNonAlloc()를 써야 찾은 Collider를 새로 생성하지 않고 BufferCollider배열에 저장 함.
        int count = Physics.OverlapSphereNonAlloc(transform.position, Stat.AttackRange, mEnemyColBuffer, Layers.GetLayerMask(ELayerName.Enemy));
        if (count == 0) return null;

        float minDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Transform enemyTrans = mEnemyColBuffer[i].transform;

            float distSqr = (enemyTrans.position - transform.position).sqrMagnitude;

            if (distSqr < minDistance)
            {
                minDistance = distSqr;
                closestEnemy = enemyTrans;
            }
        }
        return closestEnemy;
    }

    private void MoveEnemyMarker(Transform target) 
    {
        if (target == null) 
        { 
            EnemyMarker.SetActive(false);
            return;
        }

        EnemyMarker.SetActive(true);
        EnemyMarker.transform.position = target.position;
    }

    #region CoRoutines
    //코루틴은 StopState에서 실행
    public IEnumerator CheckEnemyInAttackRange()
    {
        while (true)
        {
            //0.1초 대기, 성능 최적화를 위해 코루틴 사용
            Transform closestEnemy = GetClosestEnemyInRange();
            if (closestEnemy != null)
            {
                //탐지 했으면, bool 변수를 true로 바꿈
                //이 bool변수를 PlayerController에서 Transition 조건으로 사용
                IsFindEnemy = true;

                if (closestEnemy != CurrentClosestEnemy)
                {
                    CurrentClosestEnemy = closestEnemy;
                    if (RotateToTargetCo != null) StopCoroutine(RotateToTargetCo);
                    RotateToTargetCo = StartCoroutine(RotateToTarget(CurrentClosestEnemy, transform, 30.0f));
                }
                else if (RotateToTargetCo == null)
                {
                    RotateToTargetCo = StartCoroutine(RotateToTarget(CurrentClosestEnemy, transform, 30.0f));
                }
            }
            else
            {
                IsFindEnemy = false;
                if (RotateToTargetCo != null)
                {
                    StopCoroutine(RotateToTargetCo);
                    RotateToTargetCo = null;
                }
                CurrentClosestEnemy = null;
            }

            yield return ZeroDotWait;
        }
    }

    public IEnumerator RotateToTarget(Transform targetTrans, Transform myTrans, float rotateSpeed)
    {
        if (targetTrans == null)
        {
            RotateToTargetCo = null;
            yield break;
        }

        while (targetTrans != null && targetTrans.gameObject.activeInHierarchy)
        {
            Vector3 targetDir = targetTrans.position - myTrans.position;
            targetDir.y = 0;

            if (targetDir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(targetDir, Vector3.up);
                myTrans.rotation = Quaternion.Slerp(myTrans.rotation, lookRot, Time.deltaTime * rotateSpeed);
            }

            yield return null;
        }
        RotateToTargetCo = null;
    }

    #endregion


}
