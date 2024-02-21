using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float blinkInterval = 0.002f; // 깜빡거리는 간격
    public int blinkCount = 5; // 깜빡거리는 횟수

    private Renderer objectRenderer;
    private bool isBlinking = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // 'P' 키를 눌렀을 때 깜빡이는 효과를 주도록 합니다.
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(Blink());
            }
        }
    }

    private IEnumerator Blink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            objectRenderer.enabled = false;
            yield return new WaitForSeconds(blinkInterval);
            objectRenderer.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }
        isBlinking = false;
    }
}
