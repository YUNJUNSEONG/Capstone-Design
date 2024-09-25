using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIMannger : MonoBehaviour
{
    public GameObject bossUI; // ���� UI�� ����

    void Start()
    {
        bossUI.SetActive(false); // ���� ���� �� ���� UI�� ��Ȱ��ȭ
    }

    // ���� ��ȯ �� UI Ȱ��ȭ
    public void ShowBossUI()
    {
        bossUI.SetActive(true);
    }

    // ���� ��� �� UI ��Ȱ��ȭ
    public void HideBossUI()
    {
        bossUI.SetActive(false);
    }
}
