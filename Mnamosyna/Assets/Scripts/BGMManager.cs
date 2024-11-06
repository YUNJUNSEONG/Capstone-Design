using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip normalBGM;       // 일반 BGM
    public AudioClip lowHealthBGM;    // 체력이 낮을 때의 BGM
    private AudioSource audioSource;  // AudioSource 컴포넌트를 참조할 변수
    private bool isLowHealthBGMPlaying = false;
    private Player player;            // Player 오브젝트의 체력을 참조하기 위한 변수

    void Start()
    {
        // AudioSource 컴포넌트를 가져옵니다.
        audioSource = GetComponent<AudioSource>();

        // Player 컴포넌트를 찾아서 참조합니다. (씬에 Player가 있는 경우에만)
        player = FindObjectOfType<Player>();

        // 기본 BGM을 시작할 때 재생합니다.
        PlayNormalBGM();
    }

    void Update()
    {
        // Player 오브젝트가 존재할 경우에만 체력 체크를 수행합니다.
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

    // 일반 BGM을 재생하는 함수
    public void PlayNormalBGM()
    {
        audioSource.clip = normalBGM;
        audioSource.Play();
        isLowHealthBGMPlaying = false;
    }

    // 체력이 낮을 때 BGM을 재생하는 함수
    public void PlayLowHealthBGM()
    {
        audioSource.clip = lowHealthBGM;
        audioSource.Play();
        isLowHealthBGMPlaying = true;
    }
}
