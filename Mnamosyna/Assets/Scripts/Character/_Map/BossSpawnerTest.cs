using UnityEngine;
using UnityEngine.UI;

public class BossSpawnerTest: MonoBehaviour
{
    public BossSpawner spawner;

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


            spawner.SpawnWaves();  // Start spawning waves
        }
    }

}
