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
        FirstBossEnd,
        PlayerDeath
    }

    private TutorialType currentTutorial;

    void Start()
    {
        // Initialize the tutorial messages list
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
