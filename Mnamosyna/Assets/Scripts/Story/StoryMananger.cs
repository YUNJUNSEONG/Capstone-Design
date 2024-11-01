using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Text tutorialText;
    public GameObject tutorialUI;
    public Image characterImage;

    public Player player;

    private List<string> tutorialMessages;
    private int currentMessageIndex = 0;
    private bool isTutorialActive = false;

    // 이벤트를 추가하여 튜토리얼이 끝날 때 알림
    public delegate void TutorialFinishedHandler();
    public event TutorialFinishedHandler OnTutorialFinished;

    // Enumeration for tutorial types
    public enum TutorialType
    {
        Start,
        Combat,
        PostCombat,
        SkillUnlock,
        SkillLevelUp,
        Potal,
        Healing,
        FirstBoss,
        SecondBoss,
        ThirdBoss,
        FirstBossEnd,
        SecondBossEnd,
        ThirdBossEnd,
        PlayerDeath
    }

    private TutorialType currentTutorial;

    void Start()
    {
        DontDestroyOnLoad(this);
        tutorialMessages = new List<string>();
        tutorialUI.SetActive(false);
    }

    void Update()
    {
        if (isTutorialActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            ShowNextMessage();
        }
    }

    public void StartTutorial(TutorialType tutorialType, List<string> messages)
    {
        currentTutorial = tutorialType;
        tutorialMessages = messages;
        currentMessageIndex = 0;
        isTutorialActive = true;
        tutorialUI.SetActive(true);
        PauseGame();
        ShowNextMessage();
    }

    private void ShowNextMessage()
    {
        if (currentMessageIndex < tutorialMessages.Count)
        {
            tutorialText.text = tutorialMessages[currentMessageIndex];
            currentMessageIndex++;
        }
        else
        {
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        isTutorialActive = false;
        tutorialUI.SetActive(false);
        ResumeGame();

        // 튜토리얼 종료 시 이벤트 호출
        if (OnTutorialFinished != null)
        {
            OnTutorialFinished.Invoke();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
