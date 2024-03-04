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
            HealPlayer(other.gameObject);
        }
    }

    public void HealPlayer(GameObject playerObject)
    {
        PlayerStat player = playerObject.GetComponent<PlayerStat>();

        if (player != null)
        {
            Debug.Log("Player Heal!");
            player.cur_hp += healPlayer;

            Destroy(gameObject);
        }
    }
}
