using UnityEngine;
using UnityEngine.UI;

public class BossSpawnerTest: MonoBehaviour
{
    public BossSpawner spawner;
    public RandomTalk randomTalk;

    void Awake()
    {
        if (spawner == null)
        {
            Debug.LogError("Spawner script not found.");
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("Monster spawn triggered");

            // Display random dialogue
            if (randomTalk != null)
            {
                randomTalk.DisplayRandomDialogue();
            }

            spawner.SpawnWaves();  // Start spawning waves
        }
    }

}
