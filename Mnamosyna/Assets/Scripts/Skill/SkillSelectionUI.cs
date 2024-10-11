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
            Destroy(this.gameObject); // ���� ���� �� ������Ʈ �ı�
        }
    }

}
