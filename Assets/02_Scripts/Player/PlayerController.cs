using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Inspector
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 8.0f;
    #endregion

    #region component
    private Animator mAnim;
    private CharacterController mCharacterController;
    #endregion

    #region States
    private StateMachine mStateMachine;

    private StopState mStopState;
    private MoveState mMoveState;

    #endregion

    #region Private Member
    //이동 관련
    private float mCurrentSpeedSqr;
    private Vector2 mInputDir;
    private Vector3 mMoveDir;

    //코루틴 딜레이 관련
    private WaitForSeconds attackDelay;
    private readonly float mAttackDelay = 0.3f;
    #endregion

    #region Properties
    public Animator Anim => mAnim;
    public bool CanMove { get; set; } = true;
    #endregion
    private void Awake()
    {
        //캐싱
        mAnim = GetComponent<Animator>();
        mCharacterController = GetComponent<CharacterController>();
        attackDelay = new WaitForSeconds(mAttackDelay);

        //States
        mStateMachine = new StateMachine();

        mStopState = new StopState(this);
        mMoveState = new MoveState(this);

        //상태전환 조건들
        InitTransitions();

    }
    void Start()
    {
        mStateMachine.ChangeState(mStopState);
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

        //Stop
        mStateMachine.AddTransition(mStopState, mMoveState, () => mCurrentSpeedSqr > 0.01f);

        //Move
        mStateMachine.AddTransition(mMoveState, mStopState, () => mCurrentSpeedSqr < 0.01f);
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

        mCharacterController.Move(moveDir * moveSpeed * Time.deltaTime);
        mCurrentSpeedSqr = mCharacterController.velocity.sqrMagnitude;
        RotateToMoveDir(moveDir);
    }
    private void RotateToMoveDir(Vector3 moveDir)
    {
        if (moveDir == Vector3.zero) return;
        Quaternion lookRot = Quaternion.LookRotation(moveDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
    }
    #endregion

    #region CoRoutines


    #endregion


}
