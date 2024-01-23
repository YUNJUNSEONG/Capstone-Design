using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{
    public Transform destinationTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform playerTransform)
    {
        if (destinationTransform != null)
        {
            playerTransform.position = destinationTransform.position;
            playerTransform.rotation = destinationTransform.rotation;
        }
    }

}
