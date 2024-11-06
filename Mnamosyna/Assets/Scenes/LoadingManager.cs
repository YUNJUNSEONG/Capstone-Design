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

    // 씬 로딩 메소드
    public static void LoadScene(string sceneName)
    {
        nextScenes = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        // 로딩 화면을 위해 DontDestroyOnLoad 객체 숨기기
        HideDontDestroyOnLoadObjects();

        // 씬 로딩을 시작합니다.
        StartCoroutine(LoadSceneAsync("MainScene"));
    }

    private void HideDontDestroyOnLoadObjects()
    {
        // DontDestroyOnLoad로 설정된 객체들을 숨깁니다.
        dontDestroyOnLoadObjects = GameObject.FindGameObjectsWithTag("DontDestroyOnLoad");
        foreach (var obj in dontDestroyOnLoadObjects)
        {
            obj.SetActive(false);
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // 씬을 비동기적으로 로드합니다.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float timer = 0f;
        // 씬 로딩이 완료될 때까지 진행 상황을 업데이트합니다.
        while (!operation.isDone)
        {
            yield return null;
            if (operation.progress < 0.9f)
            {
                progressBar.fillAmount = operation.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime; // Time.unscaledDeltaTime으로 수정
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // Mathf.Lerp로 수정
                if (progressBar.fillAmount >= 1f)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        // 로딩 완료 후 DontDestroyOnLoad 객체 다시 활성화
        ShowDontDestroyOnLoadObjects();
    }

    private void ShowDontDestroyOnLoadObjects()
    {
        // 모든 DontDestroyOnLoad 객체를 다시 활성화합니다.
        foreach (var obj in dontDestroyOnLoadObjects)
        {
            obj.SetActive(true);
        }
    }
}
