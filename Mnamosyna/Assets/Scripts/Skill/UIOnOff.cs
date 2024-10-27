using UnityEngine;

public class UIOnOff : MonoBehaviour
{
    public Player player;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (player != null && player.isGameOver)
        {
            Destroy(this.gameObject); // ���� ���� �� ������Ʈ �ı�
        }
    }

}
