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

    protected const float WAIT_TIME = 0.2f;

    protected bool isChase = false;
    protected bool isAttack = false;
    protected bool isDamage = false;
    protected bool isDead = false;

    public Transform target;
    public BoxCollider attackArea;
    public BoxCollider skillArea;

    protected float skillCool; 

    protected Rigidbody rigid;
    protected BoxCollider boxCollider;
    protected Material mat;
    protected NavMeshAgent nav;
    protected Animator anim;


    // �߰�: �ڷ�ƾ ������ ���� ����
    private Coroutine stateCoroutine;


    protected void Start()
    {
        // ����: �ڷ�ƾ ������ �޼���� ȣ��
        StartStateCoroutines();

        skillCool = mobStat.skill_colltime; // �ʱ� ��ų ��ٿ� ����
        StartCoroutine(SkillCooldownTimer());
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
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

    // ����: �ڷ�ƾ�� �����ϴ� �޼���
    void StartStateCoroutines()
    {
        stateCoroutine = StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    // ����: �ڷ�ƾ �����ϴ� �޼���
    void StopStateCoroutines()
    {
        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);
    }

    protected virtual IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(WAIT_TIME);

            float dist = Vector3.Distance(player.position, transform.position);
        }
    }

    protected virtual IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            yield return null;
        }
    }

    //�÷��̾� ���� ����
    void ChaseStart()
    {
        SetNavSpeed(mobStat.move_speed);
        isChase = true;
        anim.SetBool("isChase", true);
    }

    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            // NavMeshAgent�� �ӵ��� ������ �̵� �ӵ��� �����մϴ�.
            nav.speed = speed;
        }
    }

    IEnumerator SkillCooldownTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1�ʸ��� ��ų ��ٿ��� ���ҽ�Ŵ
            skillCool = Mathf.Max(0, skillCool - 1);
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
        SetColor(Color.white); // ���� �������� ����
    }
    // �ǰ� �� ���� ���� �Լ�
    void SetColor(Color color)
    {
        mat.color = color;
    }

    // ���� ���� ����

    // ���ظ� ó���ϴ� �Լ�
    void Hit(Vector3 reactVec)
    {
        isDamage = true;
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 1.5f, ForceMode.Impulse);
    }

    // ����� ó���ϴ� �Լ�
    public virtual void Death(Vector3 reactVec)
    {
        isDead = true;
        anim.SetTrigger("isDead");
        gameObject.layer = 11; // ����� ������ ���̾� ����
        isChase = false; // ���� ����
        nav.enabled = false; // �׺���̼� ��Ȱ��ȭ
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 5, ForceMode.Impulse);

        Destroy(gameObject, 2); // ���� �ð� �� ���� ������Ʈ ����
        OnDestroy();
    }
    #endregion

    // ����: ���� ������Ʈ�� �ı��� �� �ڷ�ƾ ����
    protected void OnDestroy()
    {
        StopStateCoroutines();
    }

}
