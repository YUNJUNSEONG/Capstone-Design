using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleTest : MonoBehaviour
{
    public NonBattle nonBattle;
    //public RandomTalk randomTalk;
    //public Text aliveCountText;  // Reference to the UI Text element

    void Awake()
    {
        if (nonBattle == null)
        {
            Debug.LogError("Spawner script not found.");
            return;
        }
        //aliveCountText.gameObject.SetActive(false);
        //spawner.OnAliveCountChanged += UpdateAliveCountText;  // Subscribe to the alive count change event
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("Monster spawn triggered");

            /*Display random dialogue
            if (randomTalk != null)
            {
                randomTalk.DisplayRandomDialogue();
            }*/

            //aliveCountText.gameObject.SetActive(true);
            nonBattle.firstTimeToMap();  // Start spawning waves
        }
    }
}
