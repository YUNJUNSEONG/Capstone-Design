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
            // 플레이어가 트리거 영역에 도착하면 실행
            stageBattle.PlayerArrived();
        }
    }
}
