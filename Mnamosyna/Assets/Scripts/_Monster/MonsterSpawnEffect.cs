using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnEffect : MonoBehaviour
{
    public GameObject spawnEffectPrefab;
    
    [SerializeField] private float yVector = 0;
    [SerializeField] private float zVector = 0;

    void Start()
    {
        StartSpawnEffect();
    }

    public void StartSpawnEffect()
    {
        if (spawnEffectPrefab != null)
        {
            GameObject spawnEffect = Instantiate(spawnEffectPrefab, transform.position + new Vector3(0f, yVector, zVector), transform.rotation);
            
            Renderer[] renderers = spawnEffect.GetComponentsInChildren<Renderer>();
            
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    StartCoroutine(FadeMaterial(material, 2f));
                }
            }
            
            Destroy(spawnEffect, 2f);
        }
    }

    IEnumerator FadeMaterial(Material material, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float ratio = (Time.time - startTime) / duration;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 1 - ratio);
            yield return null;
        }
    }
}
