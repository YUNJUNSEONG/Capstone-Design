using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (this.name == "Magic-0") {collision.transform.position = new Vector3(5.3f, 1.27f, 128.40f);}//0->1
        
        if (this.name == "Magic-1_1") {collision.transform.position = new Vector3(-88.0f, 1.27f, 169.1f);}//1->2-1
        if (this.name == "Magic-1_2") {collision.transform.position = new Vector3(114.8f, 1.27f, 125.3f);}//1->2-2
        
        if (this.name == "Magic-2to3_1") {collision.transform.position = new Vector3(-101.1f, 1.27f, 261.7f);}//2->3-1
        if (this.name == "Magic-2to3_2") {collision.transform.position = new Vector3(24.32f, 1.27f, 239.7f);}//2->3-2
        if (this.name == "Magic-2to3_3") {collision.transform.position = new Vector3(82.1f, 1.27f, 353.3f);}//2->3-3
        
        if (this.name == "Magic-3") {collision.transform.position = new Vector3(-41.2f, 1.27f, 333.9f);}//3-1~3->heal
        
        //포탈 오브젝트 아래에 있는 portal blue, portal yellow를 맞는 이름으로 바꿔야함 
        if (this.name == "Magic-healto4") {collision.transform.position = new Vector3(-37f, 1.27f, 414.1f);} //heal->4-1
        
        if (this.name == "Magic-4to5_1") {collision.transform.position = new Vector3(-81.8f, 1.27f, 505.8f);} //4-1->5-1
        if (this.name == "Magic-4to5_2") {collision.transform.position = new Vector3(77.8f, 1.27f, 469.2f);} //4-1->5-2
        
        if (this.name == "Magic-5to6_1") {collision.transform.position = new Vector3(-110.9f, 1.27f, 581.2f);} //5-1->6-1
        if (this.name == "Magic-5to6_2") {collision.transform.position = new Vector3(-26.1f, 1.27f, 699.6f);} //5-1~2->6-2
        if (this.name == "Magic-5to6_3") {collision.transform.position = new Vector3(62.6f, 1.27f, 582.2f);} //5-2->6-3
        
        if (this.name == "Magic-6toBoss") {collision.transform.position = new Vector3(21.71f, 1.27f, 795.78f);} //6-1~3->boss
        if(this.name == "BosstoMagic2-0")
        {
            SceneManager.LoadScene("Stage-2");
            { collision.transform.position = new Vector3(0.63f, 4.7f, -15.5f); }
        }

        //2스테이지 포탈 좌표
        if (this.name == "Magic2-0to1") {collision.transform.position = new Vector3(-2.4f, 4.7f, -77.7f);} //0->1
        
        if (this.name == "Magic2-1to2_1") {collision.transform.position = new Vector3(-67.92f, 4.7f, -45.93f);} //1->2-1
        if (this.name == "Magic2-1to2_2") {collision.transform.position = new Vector3(-56.1f, 4.7f, 22.77f);} //1->2-2
        
        if (this.name == "Magic2-2to3_1") {collision.transform.position = new Vector3(-115f, 4.7f, -80.9f);} //2->3-1
        if (this.name == "Magic2-2to3_2") {collision.transform.position = new Vector3(-113.5f, 4.7f, -10.5f);} //2->3-2
        if (this.name == "Magic2-2to3_3") {collision.transform.position = new Vector3(-122.41f, 4.7f, 49.74f);} //2->3-3
        
        if (this.name == "Magic2-3toH") {collision.transform.position = new Vector3(-125f, 4.7f, 159.6f);} //3->Heal
        
        if (this.name == "Magic2-Hto4") {collision.transform.position = new Vector3(-214.7f, 4.7f, -14.1f);} //Heal->4
        
        if (this.name == "Magic2-4to5_1") {collision.transform.position = new Vector3(-259.03f, 4.7f, -45.43f);} //4->5-1
        if (this.name == "Magic2-4to5_2") {collision.transform.position = new Vector3(-270.7f, 4.7f, 23.5f);} //4->5-2
        
        if (this.name == "Magic2-5to6_1") {collision.transform.position = new Vector3(-343.4f, 4.7f, -79f);} //5->6-1
        if (this.name == "Magic2-5to6_2") {collision.transform.position = new Vector3(-327.9f, 4.7f, -12.8f);} //5->6-2
        if (this.name == "Magic2-5to6_3") {collision.transform.position = new Vector3(-320.9f, 4.7f, 57.6f);} //5->6-3
        
        if (this.name == "Magic2-6toB") {collision.transform.position = new Vector3(-223f, 4.7f, 153.7f);} //6->Boss

        if (this.name == "BosstoMagic3-0")
        {
            SceneManager.LoadScene("Stage-3");
            { collision.transform.position = new Vector3(83.87f, 24.37f, -134.79f); }
        }
        //3스테이지 포탈 좌표
        if (this.name == "Magic3-0to1") {collision.transform.position = new Vector3(-3.4f, 1.2f, 55.3f);} //0->1
        
        if (this.name == "Magic3-1to2_1") {collision.transform.position = new Vector3(-65.3f, 1.2f, -17.8f);} //1->2-1
        if (this.name == "Magic3-1to2_2") {collision.transform.position = new Vector3(-57.6f, 1.2f, 54.8f);} //1->2-2
        
        if (this.name == "Magic3-2to3_1") {collision.transform.position = new Vector3(-121.7f, 1.2f, -41f);} //2->3-1
        if (this.name == "Magic3-2to3_2") {collision.transform.position = new Vector3(-117f, 1.2f, 22.45f);} //2->3-2
        if (this.name == "Magic3-2to3_3") {collision.transform.position = new Vector3(-126.9f, 1.2f, 82.7f);} //2->3-1
        
        if (this.name == "Magic3-3toH") {collision.transform.position = new Vector3(-0.2f, 1.2f, 170f);} //3->Heal
        
        if (this.name == "Magic3-Hto4") {collision.transform.position = new Vector3(-1.5f, 1.2f, 221.9f);} //H->4
        
        if (this.name == "Magic3-4to5_1") {collision.transform.position = new Vector3(-54.3f, 1.2f, 186.2f);} //4->5-1
        if (this.name == "Magic3-4to5_2") {collision.transform.position = new Vector3(-60.4f, 1.2f, 273.1f);} //4->5-2
        
        if (this.name == "Magic3-5to6_1") {collision.transform.position = new Vector3(-124.2f, 1.2f, 160.8f);} //5->6-1
        if (this.name == "Magic3-5to6_2") {collision.transform.position = new Vector3(-121.3f, 1.2f, 221.3f);} //5->6-2
        if (this.name == "Magic3-5to6_3") {collision.transform.position = new Vector3(-124.3f, 1.2f, 282.2f);} //5->6-1
        
        if (this.name == "Magic3-6toB") {collision.transform.position = new Vector3(-211.7f, 1.2f, 110.4f);} //6->B
        

        MonsterSpawnEffect monsterSpawnEffect = collision.gameObject.GetComponent<MonsterSpawnEffect>();
        if (monsterSpawnEffect != null) {monsterSpawnEffect.StartSpawnEffect();}
    }

}
