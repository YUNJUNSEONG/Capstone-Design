using UnityEngine;
using UnityEngine.UI;

public class SpawnerTest : MonoBehaviour
{
    public Spawner spawner;
    public RandomTalk randomTalk;
    public Text aliveCountText;  // Reference to the UI Text element

    void Awake()
    {
        if (spawner == null)
        {
            Debug.LogError("Spawner script not found.");
            return;
        }
        aliveCountText.gameObject.SetActive(false);
        spawner.OnAliveCountChanged += UpdateAliveCountText;  // Subscribe to the alive count change event
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

            aliveCountText.gameObject.SetActive(true);
            spawner.SpawnWaves();  // Start spawning waves
        }
    }

    void UpdateAliveCountText(int newAliveCount)
    {
        if (aliveCountText != null)
        {
            aliveCountText.text = "Alive Monsters: " + newAliveCount;

            if (newAliveCount <= 0)
            {
                aliveCountText.gameObject.SetActive(false);
            }
        }
    }
}
