using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic0 : MonoBehaviour
{
    Collider myCollider;
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
        
        if (this.name == "Magic-3") {collision.transform.position = new Vector3(-40.89f, 1.27f, 334.344f);}//3-1~3->heal
        
        //포탈 오브젝트 아래에 있는 portal blue, portal yellow를 맞는 이름으로 바꿔야함 
        if (this.name == "Magic-healto4") {collision.transform.position = new Vector3(-28.84f, 1.27f, 407.64f);} //heal->4-1
        
        if (this.name == "Magic-4to5_1") {collision.transform.position = new Vector3(-98.1f, 1.27f, 493f);} //4-1->5-1
        if (this.name == "Magic-4to5_2") {collision.transform.position = new Vector3(55.39f, 1.27f, 488.57f);} //4-1->5-2
        
        if (this.name == "Magic-5to6_1") {collision.transform.position = new Vector3(-110.8f, 1.27f, 580.94f);} //5-1->6-1
        if (this.name == "Magic-5to6_2") {collision.transform.position = new Vector3(83.25f, 1.27f, 547.61f);} //5-1~2->6-2
        if (this.name == "Magic-5to6_3") {collision.transform.position = new Vector3(6.86f, 1.27f, 684.92f);} //5-2->6-3
        
        if (this.name == "Magic-6toBoss") {collision.transform.position = new Vector3(18.56f, 1.27f, 793.5f);} //6-1~3->boss
        

        MonsterSpawnEffect monsterSpawnEffect = collision.gameObject.GetComponent<MonsterSpawnEffect>();
        if (monsterSpawnEffect != null) {monsterSpawnEffect.StartSpawnEffect();}
    }

}
