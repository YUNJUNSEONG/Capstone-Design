using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattle : MonoBehaviour
{
    public Magic0[] magicComponents;
    private Collider nonAreaCollider;
    private bool hasSpawnedUpgrade = false;

    void Awake()
    {
        nonAreaCollider = GetComponent<Collider>();
    }
    public void firstTimeToMap()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 playerForward = GameObject.FindGameObjectWithTag("Player").transform.forward;

        Vector3 spawnOffset = new Vector3(3.0f, 0, 0);
        Vector3 spawnPosition = playerPosition + playerForward * spawnOffset.x;
        spawnPosition.y = 1.0f;

        
        foreach (Magic0 magic in magicComponents) { magic.EnableComponents(); }
    }
}
