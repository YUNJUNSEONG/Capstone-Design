using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public bool isChase;

    public Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    NavMeshAgent nav;
    Animator anim;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart",4);
    }
    private void Update()
    {
        if (isChase)
        {
            nav.SetDestination(target.position);
        }
    }
    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isRun", true);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword")
        {
            if (Player.instance != null)
            {
                int playerMinDmg = Player.instance.MIN_DMG;
                Cur_HP -= Player.instance.MIN_DMG;
            }

            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
    }
    void FixedUpdate()
    {
        FreezeVelocity();
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        yield return new WaitForSeconds(0.3f);

        if(Cur_HP > 0)
        {
        }
        else
        {
            gameObject.layer = 11;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("Die");

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 10, ForceMode.Impulse);

            Destroy(gameObject,4);
        }
    }
}
