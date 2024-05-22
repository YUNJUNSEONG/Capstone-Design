using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic0 : MonoBehaviour
{
    Collider myCollider;
    //Renderer myRenderer;
    ParticleSystem myParticleSystem;
    
    void Awake()
    {
        myCollider = GetComponent<Collider>();
        myParticleSystem = GetComponent<ParticleSystem>();
    }
    
    void Start()
    {
        if(myCollider) {myCollider.enabled = false;}
        if(myParticleSystem) {myParticleSystem.Stop();}
    }
    
    public void EnableComponents()
    {
        if(myCollider) {myCollider.enabled = true;}
        if(myParticleSystem) {myParticleSystem.Play();}
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (this.name == "Magic-0") {collision.transform.position = new Vector3(5.15f, 0.1f, 116.11f);}
        
        if (this.name == "Magic-1_1") {collision.transform.position = new Vector3(-98.98f, 1.27f, 159.98f);}
        if (this.name == "Magic-1_2") {collision.transform.position = new Vector3(98.11f, 1.27f, 150.22f);}
        
        if (this.name == "Magic-2to3_1") {collision.transform.position = new Vector3(-103.02f, 1.27f, 262.42f);}
        if (this.name == "Magic-2to3_2") {collision.transform.position = new Vector3(10.58f, 1.27f, 238.42f);}
        if (this.name == "Magic-2to3_3") {collision.transform.position = new Vector3(10.58f, 1.27f, 238.42f);}
        
        if (this.name == "Magic-3") {collision.transform.position = new Vector3(-40.89f, 1.27f, 334.344f);}

        MonsterSpawnEffect monsterSpawnEffect = collision.gameObject.GetComponent<MonsterSpawnEffect>();
        if (monsterSpawnEffect != null) {monsterSpawnEffect.StartSpawnEffect();}
    }

}
