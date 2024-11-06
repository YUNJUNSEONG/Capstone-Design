using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    static string nextScenes;
    public GameObject[] dontDestroyOnLoadObjects;

    public Image progressBar;

    // �� �ε� �޼ҵ�
    public static void LoadScene(string sceneName)
    {
        nextScenes = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        // �ε� ȭ���� ���� DontDestroyOnLoad ��ü �����
        HideDontDestroyOnLoadObjects();

        // �� �ε��� �����մϴ�.
        StartCoroutine(LoadSceneAsync("MainScene"));
    }

    private void HideDontDestroyOnLoadObjects()
    {
        // DontDestroyOnLoad�� ������ ��ü���� ����ϴ�.
        dontDestroyOnLoadObjects = GameObject.FindGameObjectsWithTag("DontDestroyOnLoad");
        foreach (var obj in dontDestroyOnLoadObjects)
        {
            obj.SetActive(false);
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // ���� �񵿱������� �ε��մϴ�.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float timer = 0f;
        // �� �ε��� �Ϸ�� ������ ���� ��Ȳ�� ������Ʈ�մϴ�.
        while (!operation.isDone)
        {
            yield return null;
            if (operation.progress < 0.9f)
            {
                progressBar.fillAmount = operation.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime; // Time.unscaledDeltaTime���� ����
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // Mathf.Lerp�� ����
                if (progressBar.fillAmount >= 1f)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        // �ε� �Ϸ� �� DontDestroyOnLoad ��ü �ٽ� Ȱ��ȭ
        ShowDontDestroyOnLoadObjects();
    }

    private void ShowDontDestroyOnLoadObjects()
    {
        // ��� DontDestroyOnLoad ��ü�� �ٽ� Ȱ��ȭ�մϴ�.
        foreach (var obj in dontDestroyOnLoadObjects)
        {
            obj.SetActive(true);
        }
    }
}
