using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[Header("데이터 참조")]

    [Header("이벤트 발행")]
    [SerializeField] private VoidEventChannelSO mOnGamePause;        //PauseUI 구독
    [SerializeField] private VoidEventChannelSO mOnGameResume;       //PuaseUI 구독
    [SerializeField] private VoidEventChannelSO mOnSceneChanged;     //StageManager,DataManager 구독

    private bool bIsPause = false;
    private bool bIsGameOver = false;
    public bool CanPause { get; set; } = false;

    private void Start()
    {
        //LoadLobbyScene();
    }
    private void OnEnable()
    {
        //씬 전환관련
        SceneManager.sceneLoaded += HandleOnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleOnSceneLoad;
    }
    public void HandleOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        mOnSceneChanged.Raised();
    }
    public void LoadStageScene()
    {
        Time.timeScale = 1.0f;
        bIsPause = false;
        Managers.Pool.ReturnAllObjects();
        SceneManager.LoadScene(Define.Scene_Stage);
    }
    public void LoadLobbyScene()
    {
        Time.timeScale = 1.0f;
        bIsPause = false;
        Managers.Pool.ReturnAllObjects();
        SceneManager.LoadScene(Define.Scene_Lobby);
    }
    public void TogglePause()
    {
        if (!CanPause) return;

        //편의 상 게임오버에서 esc키를 누르면 stage 재로드
        if (bIsGameOver)
        {
            LoadStageScene();
            return;
        }
        //게임오버가 아닐 때,
        bIsPause = !bIsPause;
        if (bIsPause)
        {
            Time.timeScale = 0.0f;
            mOnGamePause.Raised();
        }
        else
        {
            Time.timeScale = 1.0f;
            mOnGameResume.Raised();
        }
    }
    public void HandleGameOver()
    {

    }
}
