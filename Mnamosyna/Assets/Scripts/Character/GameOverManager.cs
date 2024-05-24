using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerStart playerStart;
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("start");
        playerStart.RestartGameInitialization(); // PlayerStart의 초기화 메서드 호출
    }
}
