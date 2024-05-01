using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Text tutorialText;
    private int currentStep = 0;

    private string[] tutorialSteps = {
        "Welcome to the tutorial! Press SPACE to jump.",
        "Great! Now use the arrow keys to move around.",
        "You can interact with objects by pressing E.",
        "That's it! You've completed the tutorial!"
    };

    void Start()
    {
        // ������ �� ù ��° �ܰ��� Ʃ�丮�� �޽����� ǥ���մϴ�.
        UpdateTutorialText();
    }

    void Update()
    {
        // ���� ���, ���� �ܰ�� �����Ϸ��� �����̽��ٸ� ������ ���� ������ �߰��� �� �ֽ��ϴ�.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���� �ܰ�� �����մϴ�.
            currentStep++;
            // ��� �ܰ踦 �Ϸ��� ��� Ʃ�丮���� �����մϴ�.
            if (currentStep >= tutorialSteps.Length)
            {
                Debug.Log("Tutorial completed!");
                return;
            }
            // ���� �ܰ��� Ʃ�丮�� �޽����� ǥ���մϴ�.
            UpdateTutorialText();
        }
    }

    void UpdateTutorialText()
    {
        // ���� �ܰ��� Ʃ�丮�� �޽����� Text UI ��ҿ� ǥ���մϴ�.
        tutorialText.text = tutorialSteps[currentStep];
    }
}

