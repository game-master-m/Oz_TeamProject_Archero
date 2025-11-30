using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private readonly string stageSceneName = "Stage_Temp";
    private readonly string lobbySceneName = "Lobby_Temp";

    [Header("데이터 참조")]

    [Header("이벤트 구독")]


    [Header("이벤트 발행")]
    [SerializeField] VoidEventChannelSO onGamePause;        //PauseUI 구독
    [SerializeField] VoidEventChannelSO onGameResume;       //PuaseUI 구독

    private bool isPause = false;
    private bool isGameOver = false;
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

    }
    public void LoadStageScene()
    {
        SceneManager.LoadScene(stageSceneName);
    }
    public void LoadLobbyScene()
    {

    }
    public void TogglePause()
    {
        //편의 상 게임오버에서 esc키를 누르면 stage 재로드
        if (isGameOver)
        {
            LoadStageScene();
            return;
        }
        //게임오버가 아닐 때,
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0.0f;
            onGamePause.Raised();
        }
        else
        {
            Time.timeScale = 1.0f;
            onGameResume.Raised();
        }
    }
    public void HandleGameOver()
    {

    }
}
