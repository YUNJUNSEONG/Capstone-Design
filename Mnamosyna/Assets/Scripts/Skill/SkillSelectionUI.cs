using UnityEngine;

public class SkillSelectionUI : MonoBehaviour
{
    private Player player;
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
