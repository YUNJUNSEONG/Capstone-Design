using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    //public PlayerStart playerStart;
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("start");
    }
}
