using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip normalBGM;       // �Ϲ� BGM
    public AudioClip lowHealthBGM;    // ü���� ���� ���� BGM
    private AudioSource audioSource;  // AudioSource ������Ʈ�� ������ ����
    private bool isLowHealthBGMPlaying = false;
    private Player player;            // Player ������Ʈ�� ü���� �����ϱ� ���� ����

    void Start()
    {
        // AudioSource ������Ʈ�� �����ɴϴ�.
        audioSource = GetComponent<AudioSource>();

        // Player ������Ʈ�� ã�Ƽ� �����մϴ�. (���� Player�� �ִ� ��쿡��)
        player = FindObjectOfType<Player>();

        // �⺻ BGM�� ������ �� ����մϴ�.
        PlayNormalBGM();
    }

    void Update()
    {
        // Player ������Ʈ�� ������ ��쿡�� ü�� üũ�� �����մϴ�.
        if (player != null)
        {
            if (player.cur_hp <= player.max_hp * 0.3f)
            {
                if (!isLowHealthBGMPlaying)
                {
                    PlayLowHealthBGM();
                }
            }
            else
            {
                if (isLowHealthBGMPlaying)
                {
                    PlayNormalBGM();
                }
            }
        }
    }

    // �Ϲ� BGM�� ����ϴ� �Լ�
    public void PlayNormalBGM()
    {
        audioSource.clip = normalBGM;
        audioSource.Play();
        isLowHealthBGMPlaying = false;
    }

    // ü���� ���� �� BGM�� ����ϴ� �Լ�
    public void PlayLowHealthBGM()
    {
        audioSource.clip = lowHealthBGM;
        audioSource.Play();
        isLowHealthBGMPlaying = true;
    }
}
