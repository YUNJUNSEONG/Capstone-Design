using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Monster : MobStat
{
    public enum MonsterState
    {
        Patrol,
        Chase,
        Attack,
        Die
    }
    private MonsterState currentState;

    public float invincibleTime = 1f; // ���ݹ����Ĺ��� �ð�
    private float lastDamagedTime;
    public MonsterSpawner spawner;
    public GameObject exclamationMark;


    protected Transform player;
    protected float switchTime = 2.0f;

    protected Rigidbody rigid; //������ �ٵ�
    protected NavMeshAgent nav;
    protected System.Random random;
    protected Animator anim;


    protected const float WAIT_TIME = 0.2f;
    public bool isAttack;
    protected bool isChase;
    protected bool isDamage = false;
    protected bool isDead = false;
    protected float Skill1CanUse = 0;
    protected float Skill2CanUse = 0;
    protected float Skill3CanUse = 0;

    //�ִϸ��̼ǿ�
    protected static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    protected static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    protected static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int GetHitHash = Animator.StringToHash("GetHit");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    //���ݹ����� �����̴� �뵵
    private float flashDuration = 0.1f;
    private int flashCount = 2;
    private List<Renderer> renderers;


    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance =distance;
        random = new System.Random();
        exclamationMark.SetActive(false);
        anim = GetComponent<Animator>();
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());

        Invoke("ChaseStart", 1.0f);
    }
    // ������ ���� ����
    public void ChangeState(MonsterState state)
    {
        if (currentState == MonsterState.Patrol && state == MonsterState.Chase)
        {
            StartCoroutine(ShowExclamationMarkForSeconds(3.0f));
        }
        currentState = state;
    }
    // ���Ͱ� �ֺ��� ��ȸ => ���� ���� �� ���� ����
    void Patrol()
    {
        anim.SetBool(RunHash, true);

        if (switchTime <= 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
            {
                Vector3 finalPosition = hit.position;
                nav.SetDestination(finalPosition);
            }
            switchTime = Random.Range(2.0f, 5.0f);
        }
        else
        { switchTime -= Time.deltaTime; }
    }
    // ���͸� �÷��̾� ������ ȸ��
    void RotateMonsterToCharacter()
    {
        if (currentState == MonsterState.Die)
        {
            return;
        }

        Vector3 directionToCharacter = player.transform.position - transform.position;
        directionToCharacter.y = 0;
        Quaternion rotationToCharacter = Quaternion.LookRotation(directionToCharacter);
        transform.rotation = rotationToCharacter;
    }

    //������ �÷��̾� ���� ���� ��� �Լ�
    void Chase()
    {
        if (currentState == MonsterState.Die)
        {
            return;
        }
        SetNavSpeed(move_speed);
        anim.SetBool(RunHash, true);
        nav.SetDestination(player.transform.position);
    }
    // NavMeshAgent�� �ӵ��� ������ �̵� �ӵ��� ����
    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            nav.speed = speed;
        }
    }

    // Attack ������ �� ��ų ��Ÿ�ӿ� ���� ���� ���� �Լ�
    public void Attack()
    {
        RotateMonsterToCharacter();
        // ��Ÿ���� 0���� �� ������ �����ϰ� ���
        int skillIndex = random.Next(0, NumberOfSkills);

        switch (skillIndex)
        {
            case 0:  //�⺻ ����
                if (Skill1CanUse<=0 && attack1Radius <= 0)
                {
                    Skill1();
                    Damage(skillIndex);
                    Skill1CanUse = SkillCoolTime1;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 1: // ��ų ����1
                if (Skill2CanUse<=0 && attack2Radius <= 0)
                {
                    Skill2();
                    Damage(skillIndex);
                    Skill2CanUse = SkillCoolTime2;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 2: //��ų ����2
                if (Skill3CanUse<=0 && attack3Radius <= 0)
                {
                    Skill3();
                    Damage(skillIndex);
                    Skill3CanUse = SkillCoolTime3;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
        }
    }
    // ���� �ִϸ��̼�
    void Skill1()
    {
        anim.SetTrigger(Attack01Hash);
    }
    void Skill2()
    {
        anim.SetTrigger(Attack02Hash);
    }
    void Skill3()
    {
        anim.SetTrigger(Attack03Hash);
    }
    // ������ ���ݿ� ���� ������ ����
    public int Damage(int skillIndex)
    {
        int damage = 0;

        switch (skillIndex)
        {
            case 0: // �⺻ ����
                damage = ATK; 
                break;
            case 1: // ��ų ����1
                damage = Skill_ATK1; 
                break;
            case 2: // ��ų ����2
                damage = Skill_ATK2;
                break;
            default:
                // �������� ���� ��ų�̸� damage 0
                break;
        }
        return damage;
    }

    // ���� �ð����� ����ǥ�� ���� �ڷ�ƾ
    IEnumerator ShowExclamationMarkForSeconds(float seconds)
    {
        exclamationMark.SetActive(true);
        yield return new WaitForSeconds(seconds);
        exclamationMark.SetActive(false);
    }

    // ������ State ������Ʈ
    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(player.position);
            nav.isStopped = !isChase;
        }

        if (currentState == MonsterState.Die)
        {
            return;
        }

        switch (currentState)
        {
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Chase:
                Chase();
                break;
            case MonsterState.Attack:
                Attack();
                break;
        }
        // 1�ʸ��� ��ų �� ����
        Skill1CanUse -= Time.deltaTime;
        Skill2CanUse -= Time.deltaTime;
        Skill3CanUse -= Time.deltaTime;

    }
    // ������ �ǰ� ��Ȳ ó�� �Լ�
    public void TakeDamage(int damage)
    {
        if (currentState != MonsterState.Patrol) { ChangeState(MonsterState.Chase); }

        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            // ���� ������ = �÷��̾��� ���ݵ����� * (1 - ����%)
            int finalDamage = Mathf.RoundToInt(damage * (1 - Defense));
            Cur_HP -= finalDamage;
            lastDamagedTime = Time.time;
            anim.SetTrigger(GetHitHash);
            Flash();
        }

        if (Cur_HP <= 0)
        {
            Die();
        }
    }
    // �ǰݽ� ���� ����ȿ�� ��� �Լ�
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }
    //����ȿ�� ��� �ڷ�ƾ
    private IEnumerator DoFlash()
    {
        for (int i = 0; i < flashCount; i++)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }

            yield return new WaitForSeconds(flashDuration);

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }

            yield return new WaitForSeconds(flashDuration);
        }
    }
  
    //��� ó�� �Լ�
    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        ChangeState(MonsterState.Die);
        anim.SetTrigger(DieHash);
        nav.isStopped = true;
        anim.SetBool(RunHash, false);
        Knockback();
        Invoke("DestroyObject", 2.0f);
        Collider collider = GetComponent<Collider>();
        if (collider != null) { collider.enabled = false; }

        if (spawner != null)
        {
            spawner.aliveCount--;
            spawner.CheckAliveCount();
            Debug.Log("���� ����:"+spawner.aliveCount);
        }
        else
        {
            //Debug.LogError("���� ��ũ��Ʈ���� ���� ������ �� ã�ƿ�");
        }
    }
    //����� �˹� ó��
    protected void Knockback()
    {
        rigid.AddForce((transform.forward.normalized + Vector3.up) * 5f, ForceMode.Impulse);
    }
    // ���� ����
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
