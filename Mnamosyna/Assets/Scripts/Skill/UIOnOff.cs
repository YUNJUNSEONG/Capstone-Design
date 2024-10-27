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
            Destroy(this.gameObject); // 게임 오버 시 오브젝트 파괴
        }
    }

}
