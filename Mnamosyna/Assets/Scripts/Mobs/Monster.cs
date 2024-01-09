using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Max_HP;
    public int Cur_HP;

    public int ATK;
    public int Skill_ATK;
    public float DEF;
    public int Skill_CoolDown;

    public float ATK_Speed;
    public float Skill_Speed;
    public float Move_Speed;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    PlayerStat stat;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword")
        {
            stat = GetComponent<PlayerStat>();
            Cur_HP -= stat.MIN_ATK; 

            Debug.Log("sword : " + Cur_HP);
        }
    }

    IEnumerator OnDamage()
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.3f);

        if(Cur_HP > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color= Color.black;
            Destroy(gameObject,3);
        }
    }
}
