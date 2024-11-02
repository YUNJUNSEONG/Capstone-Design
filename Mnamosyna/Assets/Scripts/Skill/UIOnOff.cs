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
        // ���� ���� ���¿��� ���ο� ���� �ε�Ǿ��� �� ������Ʈ �ı�
        if (player != null && player.isGameOver)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
