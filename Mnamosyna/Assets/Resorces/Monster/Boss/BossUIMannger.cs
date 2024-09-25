using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIMannger : MonoBehaviour
{
    public GameObject bossUI; // 보스 UI를 연결

    void Start()
    {
        bossUI.SetActive(false); // 게임 시작 시 보스 UI는 비활성화
    }

    // 보스 소환 시 UI 활성화
    public void ShowBossUI()
    {
        bossUI.SetActive(true);
    }

    // 보스 사망 시 UI 비활성화
    public void HideBossUI()
    {
        bossUI.SetActive(false);
    }
}
