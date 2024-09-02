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
        if (this.name == "Magic-0") {collision.transform.position = new Vector3(5.117f, 1.27f, 129.00f);}//0->1
        
        if (this.name == "Magic-1_1") {collision.transform.position = new Vector3(-88.6f, 1.27f, 169.18f);}//1->2-1
        if (this.name == "Magic-1_2") {collision.transform.position = new Vector3(114.7f, 1.27f, 125.62f);}//1->2-2
        
        if (this.name == "Magic-2to3_1") {collision.transform.position = new Vector3(-100.77f, 1.27f, 262.04f);}//2->3-1
        if (this.name == "Magic-2to3_2") {collision.transform.position = new Vector3(24.36f, 1.27f, 238.57f);}//2->3-2
        if (this.name == "Magic-2to3_3") {collision.transform.position = new Vector3(82.18f, 1.27f, 353.23f);}//2->3-3
        
        if (this.name == "Magic-3") {collision.transform.position = new Vector3(-40.89f, 1.27f, 334.344f);}//3-1~3->heal
        
        //포탈 오브젝트 아래에 있는 portal blue, portal yellow를 맞는 이름으로 바꿔야함 
        if (this.name == "Magic-healto4") {collision.transform.position = new Vector3(-37.16f, 1.27f, 414.37f);} //heal->4-1
        
        if (this.name == "Magic-4to5_1") {collision.transform.position = new Vector3(-82.042f, 1.27f, 505.898f);} //4-1->5-1
        if (this.name == "Magic-4to5_2") {collision.transform.position = new Vector3(77.7f, 1.27f, 468.77f);} //4-1->5-2
        
        if (this.name == "Magic-5to6_1") {collision.transform.position = new Vector3(-110.84f, 1.27f, 580.82f);} //5-1->6-1
        if (this.name == "Magic-5to6_2") {collision.transform.position = new Vector3(-25.9f, 1.27f, 700.07f);} //5-1~2->6-2
        if (this.name == "Magic-5to6_3") {collision.transform.position = new Vector3(62.23f, 1.27f, 582.38f);} //5-2->6-3
        
        if (this.name == "Magic-6toBoss") {collision.transform.position = new Vector3(18.56f, 1.27f, 793.5f);} //6-1~3->boss
        
        
        //2스테이지 포탈 좌표
        if (this.name == "Magic2-0to1") {collision.transform.position = new Vector3(11.91f, 4.7f, -46.59f);} //0->1
        
        if (this.name == "Magic2-1to2_1") {collision.transform.position = new Vector3(-40.81f, 4.7f, -16.71f);} //1->2-1
        if (this.name == "Magic2-1to2_2") {collision.transform.position = new Vector3(-39.83f, 4.7f, 52.62f);} //1->2-2
        
        if (this.name == "Magic2-2to3_1") {collision.transform.position = new Vector3(-127.49f, 4.7f, -48.1f);} //2->3-1
        if (this.name == "Magic2-2to3_2") {collision.transform.position = new Vector3(-109.33f, 4.7f, 19.2f);} //2->3-2
        if (this.name == "Magic2-2to3_3") {collision.transform.position = new Vector3(-101.21f, 4.7f, 80.89f);} //2->3-3
        
        if (this.name == "Magic2-3toH") {collision.transform.position = new Vector3(-109.19f, 4.7f, 191.45f);} //3->Heal
        
        if (this.name == "Magic2-Hto4") {collision.transform.position = new Vector3(-200.99f, 4.7f, 16.46f);} //Heal->4
        
        if (this.name == "Magic2-4to5_1") {collision.transform.position = new Vector3(-283.81f, 4.7f, -13.8f);} //4->5-1
        if (this.name == "Magic2-4to5_2") {collision.transform.position = new Vector3(-255.68f, 4.7f, 54.65f);} //4->5-2
        
        if (this.name == "Magic2-5to6_1") {collision.transform.position = new Vector3(-342.92f, 4.7f, -45.25f);} //5->6-1
        if (this.name == "Magic2-5to6_2") {collision.transform.position = new Vector3(-342.59f, 4.7f, 17.03f);} //5->6-2
        if (this.name == "Magic2-5to6_3") {collision.transform.position = new Vector3(-344.75f, 4.7f, 82.66f);} //5->6-3
        
        if (this.name == "Magic2-6toB") {collision.transform.position = new Vector3(-201.89f, 4.7f, 190.98f);} //6->Boss
        
        //3스테이지 포탈 좌표
        if (this.name == "Magic3-0to1") {collision.transform.position = new Vector3(16.77f, 1.2f, 65.24f);} //0->1
        
        if (this.name == "Magic3-1to2_1") {collision.transform.position = new Vector3(-44.42f, 1.2f, -9.28f);} //1->2-1
        if (this.name == "Magic3-1to2_2") {collision.transform.position = new Vector3(-42.25f, 1.2f, 70.85f);} //1->2-2
        
        if (this.name == "Magic3-2to3_1") {collision.transform.position = new Vector3(-108.156f, 1.2f, -25.323f);} //2->3-1
        if (this.name == "Magic3-2to3_2") {collision.transform.position = new Vector3(-105.42f, 1.2f, 41.15f);} //2->3-2
        if (this.name == "Magic3-2to3_3") {collision.transform.position = new Vector3(-110.631f, 1.2f, 96.528f);} //2->3-1
        
        if (this.name == "Magic3-3toH") {collision.transform.position = new Vector3(-0.054f, 1.2f, 169.969f);} //3->Heal
        
        if (this.name == "Magic3-Hto4") {collision.transform.position = new Vector3(18.899f, 1.2f, 232.56f);} //H->4
        
        if (this.name == "Magic3-4to5_1") {collision.transform.position = new Vector3(-40.52f, 1.2f, 202.12f);} //4->5-1
        if (this.name == "Magic3-4to5_2") {collision.transform.position = new Vector3(-46.018f, 1.2f, 286.514f);} //4->5-2
        
        if (this.name == "Magic3-5to6_1") {collision.transform.position = new Vector3(-110.159f, 1.2f, 173.915f);} //5->6-1
        if (this.name == "Magic3-5to6_2") {collision.transform.position = new Vector3(-105.758f, 1.2f, 236.559f);} //5->6-2
        if (this.name == "Magic3-5to6_3") {collision.transform.position = new Vector3(-108.121f, 1.2f, 294.936f);} //5->6-1
        
        if (this.name == "Magic3-6toB") {collision.transform.position = new Vector3(-196.52f, 1.2f, 127.51f);} //6->B
        

        MonsterSpawnEffect monsterSpawnEffect = collision.gameObject.GetComponent<MonsterSpawnEffect>();
        if (monsterSpawnEffect != null) {monsterSpawnEffect.StartSpawnEffect();}
    }

}
