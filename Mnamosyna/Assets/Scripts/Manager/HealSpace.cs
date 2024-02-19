using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealSpace : MonoBehaviour
{
    public int healPlayer = 100;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealPlayer();
        }
    }

    public void HealPlayer()
    {
        PlayerStat player = GetComponent<PlayerStat>();

        if (player != null)
        {
            player.cur_hp += healPlayer;
            Destroy(gameObject);
        }
    }
}
