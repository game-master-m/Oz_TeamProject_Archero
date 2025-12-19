using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("이벤트 구독")]
    [SerializeField] private VoidEventChannelSO mOnGameResume;   //GameManager 발행
    [SerializeField] private VoidEventChannelSO mOnGamePause;    //GameManager 발행

    [Header("참조")]
    [SerializeField] private GameObject mPausePannel;
    [SerializeField] private Button mContinueBtn;
    [SerializeField] private Button mExitBtn;
    [SerializeField] private float mBtnDelay = 0.3f;

    private WaitForSeconds mBtnWait;
    private void Awake()
    {
        mPausePannel.SetActive(false);
        mContinueBtn.onClick.RemoveAllListeners();
        mExitBtn.onClick.RemoveAllListeners();
        mContinueBtn.onClick.AddListener(OnClickContinueBtn);
        mExitBtn.onClick.AddListener(OnClickExitBtn);

        mBtnWait = new WaitForSeconds(mBtnDelay);
    }
    private void OnEnable()
    {
        //이벤트 발생 시 실행 할 메서드 연결
        mOnGameResume.onEvent += HandleGameResume;
        mOnGamePause.onEvent += HandleGamePause;
    }
    private void OnDisable()
    {
        //메서드 연결 해제
        mOnGameResume.onEvent -= HandleGameResume;
        mOnGamePause.onEvent -= HandleGamePause;

    }

    private void Update()
    {
        //편의상 esc키로 -> 나중에 버튼으로!
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.Game.TogglePause();


        }
    }
    private void OnClickContinueBtn()
    {
        Managers.Game.TogglePause();
    }
    private void OnClickExitBtn()
    {
        Managers.Game.LoadLobbyScene();
    }
    private void HandleGamePause()
    {
        mPausePannel.SetActive(true);
    }
    private void HandleGameResume()
    {
        mPausePannel.SetActive(false);
    }


}
