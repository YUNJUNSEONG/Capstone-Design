using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public MobStat mobStat;
    public Transform player;
    public static Monster instance;

    public bool isChase;
    public bool isAttack;
    public bool isSkill;
    bool isSkillCool;
    bool isSkillCoolInProgress;
    bool isDamage;

    public Transform target;
    public BoxCollider attackArea;
    public BoxCollider skillArea;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        mobStat = GetComponent<MobStat>();
        instance = this;
        player = GameObject.FindWithTag("Player").transform;

        Invoke("ChaseStart",4);
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(player.position);
            nav.isStopped = !isChase;
        }
        
    }


    //�÷��̾� ���� ����
    void ChaseStart()
    {
        SetNavSpeed(mobStat.move_speed);
        isChase = true;
        anim.SetBool("isRun", true);
    }

    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            // NavMeshAgent�� �ӵ��� ������ �̵� �ӵ��� �����մϴ�.
            nav.speed = speed;
        }
    }

    //���� �ǰ� Ȯ�� �� ������ �ο�
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword")
        {
            if(!isDamage)
            {
                if (Player.instance != null)
                {
                    int damage = Player.instance.Damage();

                    int finalDamage = Mathf.RoundToInt(damage * (1 - mobStat.defense));
                    mobStat.cur_hp = Mathf.Max(0, mobStat.cur_hp - finalDamage);

                    Debug.Log("���Ͱ� ���� ���� :" + finalDamage);

                }

                Vector3 reactVec = transform.position - other.transform.position;

                StartCoroutine(OnDamage(reactVec));
            }
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

    // ������ �÷��̾� Ÿ����
    void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,LayerMask.GetMask("Player"));
        
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());

            //���Ͱ� �÷��̾ ����ģ ���� ���� ��ų ��Ÿ�� Ȱ��ȭ
            StartCoroutine(SkillCool());
        }
    }

    IEnumerator Attack()
    {

        //��ų�� ��Ÿ���� ���
        if (isSkillCool)
        {
            isChase = false;
            isAttack = true;
            anim.SetBool("isAttack", true);

            yield return new WaitForSeconds(0.2f);
            attackArea.enabled = true;


            yield return new WaitForSeconds(0.3f);
            attackArea.enabled = false;

            yield return new WaitForSeconds(mobStat.atk_speed);
            isChase = true;
            isAttack = false;
            anim.SetBool("isAttack", false);
        }
        //��ų�� ��Ÿ���� �ƴ� ���
        else
        {
            //��ų ���
            StartCoroutine(Skill());
            //��ų ��Ÿ�� ����
            StartCoroutine(SkillCool());
        }

    }

    IEnumerator Skill()
    {
        isChase = false;
        isAttack = true;
        isSkill = true;
        anim.SetBool("isSkill", true);

        yield return new WaitForSeconds(0.2f);
        skillArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        skillArea.enabled = false;

        yield return new WaitForSeconds(mobStat.atk_speed);
        isChase = true;
        isAttack = false;
        isSkill = false;
        anim.SetBool("isSkill", false);
    }

    IEnumerator SkillCool() 
    {
        // ��ų ��Ÿ���� �ʱ�ȭ���� �ʵ��� ����
        if (!isSkillCoolInProgress)
        {
            isSkillCoolInProgress = true;
            isSkillCool = true;
            yield return new WaitForSeconds(mobStat.skill_colltime);
            isSkillCool = false;
            isSkillCoolInProgress = false;  // �߰�: Coroutine�� ���� �� ���� �ʱ�ȭ
        }
    }

    public int Damage()
    {
        int damage = mobStat.attack;
        int skillDamage = mobStat.skill_attack;

        if(isSkill)
        {
            return skillDamage;
        }
        else
        {
            return damage;
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        isDamage = true; //���� �����ð�
        // ���ظ� �޾��� �� ��� ������ �������ϰ� ����
        ChangeMaterialTransparency(0.5f);

        yield return new WaitForSeconds(1f); // �����ð� 1��

        if (mobStat.cur_hp > 0)
        {
            ChangeMaterialTransparency(1.0f);
            anim.SetTrigger("getHit");

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 1.5f, ForceMode.Impulse);
        }
        else
        {
            ChangeMaterialTransparency(1.0f);
            gameObject.layer = 11;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("Die");

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            Destroy(gameObject,3);
        }

        isDamage = false;
    }

    void ChangeMaterialTransparency(float alphaValue)
    {
        // ��Ƽ������ �����Ǿ� ���� ������ ����
        if (mat == null)
        {
            Debug.LogError("Material is not assigned.");
            return;
        }

        // ��Ƽ������ ������ �������� ���� ���� ����
        Color color = mat.color;
        color.a = alphaValue;
        mat.color = color;
    }

}
