using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Monster : MonoBehaviour
{
    public MobStat mobStat;
    public Transform player;

    protected const float WAIT_TIME = 0.1f;
    bool isChase;
    bool isDamage;

    public Transform target;
    public BoxCollider attackArea;
    public BoxCollider skillArea;

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public Material mat;
    public NavMeshAgent nav;
    public Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        mobStat = GetComponent<MobStat>();
        player = GameObject.FindWithTag("Player").transform;

        Invoke("ChaseStart", 1.0f);
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
        if (other.tag == "Sword")
        {
            if (!isDamage)
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

    public int Damage()
    {
        int damage = mobStat.attack;
        return damage;

    }
    public int SkillDamage()
    {
        int skilldamage = mobStat.skill_attack;
        return skilldamage;
    }

    #region OnDamage
    // ���� �ð� ��� ����
    private const float INVINCIBILITY_TIME = 1f;
    IEnumerator OnDamage(Vector3 reactVec)
    {
        if (isDamage)
            yield break;

        isDamage = true;
        SetInvincible(true); // ���͸� ���� ���·� ����

        SetColor(Color.black); // �ǰ� �� ������ ����

        yield return new WaitForSeconds(WAIT_TIME);

        if (mobStat.cur_hp > 0)
        {
            Hit(reactVec); // ���ظ� ó���ϴ� �Լ� ȣ��
        }
        else
        {
            Death(reactVec); // ��� ó���ϴ� �Լ� ȣ��
        }

        yield return new WaitForSeconds(INVINCIBILITY_TIME);
        SetInvincible(false); // ���� ���� ����
    }
    // �ǰ� �� ���� ���� �Լ�
    void SetColor(Color color)
    {
        mat.color = color;
    }

    // ���� ���� ���� �Լ�
    void SetInvincible(bool invincible)
    {
        isDamage = invincible;
    }

    // ���ظ� ó���ϴ� �Լ�
    void Hit(Vector3 reactVec)
    {
        SetColor(Color.white); // ���� �������� ����
        anim.SetBool("getHit", true); // �ǰ� �ִϸ��̼� ���

        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 1.5f, ForceMode.Impulse);

        rigid.isKinematic = true; // ���� �ۿ� ����
    }

    // ����� ó���ϴ� �Լ�
    void Death(Vector3 reactVec)
    {
        SetColor(Color.black);  // ���� �������� ����
        gameObject.layer = 11; // ����� ������ ���̾� ����
        isChase = false; // ���� ����
        nav.enabled = false; // �׺���̼� ��Ȱ��ȭ
        anim.SetTrigger("Die"); // ��� �ִϸ��̼� ���

        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 5, ForceMode.Impulse);

        Destroy(gameObject, 2); // ���� �ð� �� ���� ������Ʈ ����

        rigid.isKinematic = true; // ���� �ۿ� ����
    }
    #endregion

}
