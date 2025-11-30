using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [Header("이벤트 구독")]
    [SerializeField] VoidEventChannelSO onGameResume;   //GameManager 발행
    [SerializeField] VoidEventChannelSO onGamePause;    //GameManager 발행

    [Header("참조")]
    [SerializeField] GameObject pausePannel;
    private void Awake()
    {
        pausePannel.SetActive(false);
    }
    private void OnEnable()
    {
        //이벤트 발생 시 실행 할 메서드 연결
        onGameResume.onEvent += HandleGameResume;
        onGamePause.onEvent += HandleGamePause;
    }
    private void OnDisable()
    {
        //메서드 연결 해제
        onGameResume.onEvent -= HandleGameResume;
        onGamePause.onEvent -= HandleGamePause;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.Game.TogglePause();
        }
    }
    private void HandleGamePause()
    {
        pausePannel.SetActive(true);
    }
    private void HandleGameResume()
    {
        pausePannel.SetActive(false);
    }
}
