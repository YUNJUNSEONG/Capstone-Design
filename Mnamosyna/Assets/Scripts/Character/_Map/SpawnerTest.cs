using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public Spawner spawner;
    public RandomTalk randomTalk;
    void Start()
    {   
        if(spawner == null)
        {
            Debug.LogError("스크립트 못찾");
            return;
        }
         
    }
    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Collider>().enabled)//other.gameObject.CompareTag("Player") && 
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("몬스터스폰");
            // 랜덤 대화 출력
            if (randomTalk != null)
            {
                randomTalk.DisplayRandomDialogue();
            }
            spawner.SpawnWaves();
            spawner.waitTime -= Time.deltaTime;

        }
    }


}
