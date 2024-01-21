using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public MobStat mobStat;

    public bool isChase;
    public bool isAttack;
    public bool isSkill;
    bool isSkillCool = false;

    public Transform target;
    public BoxCollider attackArea;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        mobStat = new MobStat();

        Invoke("ChaseStart",4);
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
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
                int damage = Player.instance.Damage();

                int finalDamage = Mathf.RoundToInt(damage * (1 - mobStat.defense));
                mobStat.cur_hp -= finalDamage;

                Debug.Log("몬스터가 받은 피해 :" + finalDamage);

            }

            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec));
        }
    }



    void FixedUpdate()
    {
        FreezeVelocity();
        Targeting();
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,LayerMask.GetMask("Player"));
        
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.2f);
        attackArea.enabled = true;

        StartCoroutine(Skill());

        yield return new WaitForSeconds(mobStat.atk_speed);
        attackArea.enabled = false;

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);

        StartCoroutine(SkillCool());
    }

    IEnumerator Skill()
    {
        anim.SetBool("isSkill", true);

        yield return new WaitForSeconds(1f);

        anim.SetBool("isSkill", false);
    }

    IEnumerator SkillCool() 
    {
        isSkillCool = true;
        yield return new WaitForSeconds(mobStat.skill_colltime);
        isSkillCool = false;
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.black;
        yield return new WaitForSeconds(0.3f);

        if (mobStat.cur_hp > 0)
        {
            mat.color = Color.white;

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 3, ForceMode.Impulse);

            anim.SetBool("getHit", true);
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
