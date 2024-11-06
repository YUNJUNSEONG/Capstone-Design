using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public GameObject pauseMenu;  // 일시 정지 메뉴를 연결할 변수
    public Button resumeButton;   // Resume 버튼
    public Button mainMenuButton; // Main Menu 버튼
    public Button quitButton;     // Quit 버튼

    void Start()
    {
        // 버튼 클릭 이벤트 설정
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        quitButton.onClick.AddListener(QuitGame);

        // 일시 정지 메뉴 초기 상태는 비활성화
        pauseMenu.SetActive(false);
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        // Esc 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // 게임 일시 정지
    void PauseGame()
    {
        Time.timeScale = 0f;  // 게임 시간을 멈춤
        pauseMenu.SetActive(true);  // 일시 정지 메뉴 활성화
    }

    // 게임 재개
    void ResumeGame()
    {
        Time.timeScale = 1f;  // 게임 시간 재개
        pauseMenu.SetActive(false);  // 일시 정지 메뉴 비활성화
    }

    // 타이틀로 돌아가기
    void GoToMainMenu()
    {
        Time.timeScale = 1f;  // 게임 시간 재개
        SceneManager.LoadScene("MainMenu");  // 타이틀 화면으로 이동
    }

    // 게임 종료
    void QuitGame()
    {
        Application.Quit();  // 게임 종료
    }
}
