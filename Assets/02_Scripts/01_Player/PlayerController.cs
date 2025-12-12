using UnityEngine;

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


    private PlayerStatDataSO mStatDataSO;
    #endregion

    #region States
    private StateMachine mStateMachine;

    private StopState mStopState;
    private MoveState mMoveState;
    private ThrowState mThrowState;

    #endregion

    #region Private Member
    //이동 관련
    private float mCurrentSpeedSqr;
    private Vector2 mInputDir;
    private Vector3 mMoveDir;

    //코루틴 딜레이 관련
    private WaitForSeconds mAttackDelayWait;
    private readonly float mAttackDelay = 0.3f;
    #endregion

    #region Properties
    public Animator Anim => mAnim;
    public PlayerAttack Attack => mAttack;
    public PlayerStat Stat => mStat;
    public bool CanMove { get; set; } = true;


    public ThrowState ThrowState => mThrowState;
    public StopState StopState => mStopState;
    public StateMachine StateMachine => mStateMachine;
    public Vector2 InputDir => mInputDir;

    #endregion
    private void Awake()
    {
        //캐싱
        mAnim = GetComponent<Animator>();
        mCharacterController = GetComponent<CharacterController>();
        mStat = GetComponent<PlayerStat>();
        mAttack = GetComponent<PlayerAttack>();
        mAttackDelayWait = new WaitForSeconds(mAttackDelay);


        mStatDataSO = mStat.StatDataSO;
        
        

        //States
        mStateMachine = new StateMachine();

        //mStopState = new StopState(this);
        mStopState = new StopState(this,mStatDataSO);
        mMoveState = new MoveState(this);
        mThrowState = new ThrowState(this,mStatDataSO);

        //상태전환 조건들
        InitTransitions();

    }
    void Start()
    {
        mAttack.InitStat(mStat);
        mStateMachine.ChangeState(mStopState);
        //mStateMachine.ChangeState(mThrowState);
    }

    void Update()
    {
        //테스트용 키보드 입력 -> 조이스틱 입력으로 바꿔야 함
        Inputs();
        Movements(mMoveDir);
        mStateMachine.Update();
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
        //mStateMachine.AddAnyTransition(mMoveState, () => true && !mStateMachine.IsCurrentState(mMoveState));

        //Stop
        mStateMachine.AddTransition(mStopState, mMoveState, () => mCurrentSpeedSqr > 0.01f);

        //Move
        mStateMachine.AddTransition(mMoveState, mStopState, () => mCurrentSpeedSqr < 0.01f);

        //attack
        //mStateMachine.AddTransition(mThrowState, mMoveState, () => mCurrentSpeedSqr > 0.01f);
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
        RotateToMoveDir(moveDir);
    }
    private void RotateToMoveDir(Vector3 moveDir)
    {
        if (moveDir == Vector3.zero) return;
        Quaternion lookRot = Quaternion.LookRotation(moveDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * mStat.RotateSpeed);
    }
    #endregion

    #region CoRoutines


    #endregion


}
