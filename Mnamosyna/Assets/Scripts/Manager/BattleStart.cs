using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour
{
    public StageBattle stageBattle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ Ʈ���� ������ �����ϸ� ����
            stageBattle.PlayerArrived();
        }
    }
}
