using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void SceneChange(){
        LoadingManager.LoadScene("Stage-1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
