using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public GameObject pauseMenu;  // �Ͻ� ���� �޴��� ������ ����
    public Button resumeButton;   // Resume ��ư
    public Button mainMenuButton; // Main Menu ��ư
    public Button quitButton;     // Quit ��ư

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        quitButton.onClick.AddListener(QuitGame);

        // �Ͻ� ���� �޴� �ʱ� ���´� ��Ȱ��ȭ
        pauseMenu.SetActive(false);
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        // Esc Ű�� ������ ��
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

    // ���� �Ͻ� ����
    void PauseGame()
    {
        Time.timeScale = 0f;  // ���� �ð��� ����
        pauseMenu.SetActive(true);  // �Ͻ� ���� �޴� Ȱ��ȭ
    }

    // ���� �簳
    void ResumeGame()
    {
        Time.timeScale = 1f;  // ���� �ð� �簳
        pauseMenu.SetActive(false);  // �Ͻ� ���� �޴� ��Ȱ��ȭ
    }

    // Ÿ��Ʋ�� ���ư���
    void GoToMainMenu()
    {
        Time.timeScale = 1f;  // ���� �ð� �簳
        SceneManager.LoadScene("MainMenu");  // Ÿ��Ʋ ȭ������ �̵�
    }

    // ���� ����
    void QuitGame()
    {
        Application.Quit();  // ���� ����
    }
}
