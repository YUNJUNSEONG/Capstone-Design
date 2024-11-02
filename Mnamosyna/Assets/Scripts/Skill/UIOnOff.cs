using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOnOff : MonoBehaviour
{
    public Player player;

    void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 게임 오버 상태에서 새로운 씬이 로드되었을 때 오브젝트 파괴
        if (player != null && player.isGameOver)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
