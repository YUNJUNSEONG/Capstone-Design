using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : BaseMonster
{
    [Header("���� ����")]
    [SerializeField]
    protected int Skill02;
    [SerializeField]
    protected int Skill03;

    [Header("���� ��Ÿ��")]
    [SerializeField]
    protected float SkillCoolTime2;
    [SerializeField]
    protected float SkillCoolTime3;


    private bool canBasicAttack = true;
    private bool canUsePattern1 = true;
    private bool canUsePattern2 = true;
    private bool canUsePattern3 = true;

    protected float Skill02CanUse;
    protected float Skill03CanUse;

    protected float pattern1Cooldown;
    protected float pattern2Cooldown;
    protected float pattern3Cooldown;

    private float lastPattern1Time;
    private float lastPattern2Time;
    private float lastPattern3Time;

    // �ִϸ��̼ǿ�
    protected static readonly int ScreamHash = Animator.StringToHash("Scream");
    protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    public int attack03Hash;
    protected static readonly int Attack04Hash = Animator.StringToHash("Attack04");
    public int attack04Hash;

    // ���� ���� �ʵ�
    private float attackRange;
    private float attackCooldown;
    private float lastAttackTime;

    protected override void Awake()
    {
        base.Awake();

        attackRange = ApproachRadius;
        attackCooldown = AttackCoolTime;
        pattern1Cooldown = SkillCoolTime1;
        pattern2Cooldown = SkillCoolTime2;
        pattern3Cooldown = SkillCoolTime3;
    }
    void Start()
    {
        StartCoroutine(ChangeToChaseAfterDelay(0.5f));
    }

    protected override void Update()
    {
        base.Update();

        RotateMonsterToCharacter();

        if (canBasicAttack)
        {
            StartCoroutine(BasicAttack());
        }

        if (Cur_HP <= Max_HP * 0.8f && Cur_HP > Max_HP * 0.5f && canUsePattern1 && Time.time >= lastPattern1Time + pattern1Cooldown)
        {
            StartCoroutine(UsePattern1());
            lastPattern1Time = Time.time;
        }

        if (Cur_HP <= Max_HP * 0.5f && Cur_HP > Max_HP * 0.3f && canUsePattern2 && Time.time >= lastPattern2Time + pattern2Cooldown)
        {
            StartCoroutine(UsePattern2());
            lastPattern2Time = Time.time;
        }

        if (Cur_HP <= Max_HP * 0.3f && canUsePattern3 && Time.time >= lastPattern3Time + pattern3Cooldown)
        {
            StartCoroutine(UsePattern3());
            lastPattern3Time = Time.time;
        }

        // 1�ʸ��� ��ų �� ����
        AttackCoolTime -= Time.deltaTime;
        SkillCoolTime1 -= Time.deltaTime;
        SkillCoolTime2 -= Time.deltaTime;
        SkillCoolTime3 -= Time.deltaTime;

    }

    public override void ChangeState(State state)
    {
        currentState = state;
    }

    protected virtual void UpdateIdleState()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 5f)
        {
            currentState = State.Chase;
            anim.SetBool(RunHash, true);
        }
    }

    protected virtual void UpdateChaseState()
    {
        nav.SetDestination(player.transform.position);
        SetNavSpeed(Move_Speed);

        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            currentState = State.Attack;
            anim.SetBool(RunHash, false);
        }
    }

    private IEnumerator ChangeToChaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(State.Chase);
    }
    

    // NavMeshAgent�� �ӵ��� ������ �̵� �ӵ��� ����
    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            nav.speed = speed;
        }
    }

    protected virtual void UpdateAttackState()
    {
        //nav.SetDestination(transform.position); // ������ ���� ���� ���� ����ϴ�.
        nav.isStopped = true;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (canBasicAttack)
            {
                StartCoroutine(BasicAttack());
            }
            lastAttackTime = Time.time;
        }

        if (Vector3.Distance(player.transform.position, transform.position) > attackRange)
        {
            currentState = State.Chase;
            anim.SetBool(RunHash, true);
        }
    }

    protected override void Die()
    {
        isDead = true;
        nav.isStopped = true;
        gameObject.layer = 11;
        ChangeState(State.Die);
        anim.SetTrigger(DieHash);
        anim.SetBool(RunHash, false);


        // �ݶ��̴� ��Ȱ��ȭ�� 2�� �ڿ� ó���մϴ�.
        Invoke("DisableCollider", 2.0f);
        Invoke("DestroyObject", 3.0f); // 4�� �� ��ü�� �����մϴ�.

        spawner.aliveCount--;
        spawner.CheckAliveCount();
        spawner.NotifyAliveCountChanged();
    }

    void DisableCollider()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
    }


    protected virtual IEnumerator FlashOnHit()
    {
        isDamage = true;
        for (int i = 0; i < flashCount; i++)
        {
            foreach (var renderer in renderers)
            {
                renderer.material.color = Color.red;
            }
            yield return new WaitForSeconds(flashDuration);
            foreach (var renderer in renderers)
            {
                renderer.material.color = originalColor;
            }
            yield return new WaitForSeconds(flashDuration);
        }
        isDamage = false;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }


    protected IEnumerator BasicAttack()
    {
        canBasicAttack = false;
        anim.SetTrigger("Attack01");

        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            player.GetComponent<Player>().TakeDamage(ATK);
        }
        else
        {
            anim.SetTrigger(BattleIdleHash);
        }

        yield return new WaitForSeconds(AttackCanUse);
        canBasicAttack = true;
    }

    IEnumerator UsePattern1()
    {
        canUsePattern1 = false;
        // ���� 1 �ִϸ��̼� ���
        anim.SetTrigger("Attack02");
        // ���� 1�� ���� ����
        player.GetComponent<Player>().TakeDamage(Skill01);
        yield return new WaitForSeconds(Skill01CanUse);
        canUsePattern1 = true;
    }

    IEnumerator UsePattern2()
    {
        canUsePattern2 = false;
        // ���� 2 �ִϸ��̼� ���
        anim.SetTrigger("Attack03");
        // ���� 2�� ���� ����
        player.GetComponent<Player>().TakeDamage(Skill02);
        yield return new WaitForSeconds(Skill02CanUse);
        canUsePattern2 = true;
    }

    IEnumerator UsePattern3()
    {
        canUsePattern3 = false;
        // ���� 3 �ִϸ��̼� ���
        anim.SetTrigger("Attack04");
        // ���� 3�� ���� ����
        player.GetComponent<Player>().TakeDamage(Skill03);
        yield return new WaitForSeconds(Skill03CanUse);
        canUsePattern3 = true;
    }


}
