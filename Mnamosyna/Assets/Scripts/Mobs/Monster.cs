using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MobStat
{
    public enum MonsterState
    {
        Patrol,
        Chase,
        Attack,
        Die
    }
    protected MonsterState currentState;

    public float invincibleTime = 1f; // ���ݹ��� �� ���� �ð�
    private float lastDamagedTime;
    public Spawner spawner;
    public delegate void DeathHandler();
    public event DeathHandler OnDeath;
    public GameObject exclamationMark;

    public Text damageText;
    private Vector3 originalPosition;
    private Color originalColor;

    // ù ��° ���� �ִϸ��̼��� ���� (�� ����)
    public float firstAttackAnimationLength;

    // �� ��° ���� �ִϸ��̼��� ���� (�� ����)
    public float secondAttackAnimationLength;

    protected GameObject player;
    protected float switchTime = 2.0f;

    protected Rigidbody rigid; // ������ �ٵ�
    protected NavMeshAgent nav;
    protected System.Random random;
    protected Animator anim;

    protected const float WAIT_TIME = 0.2f;
    public bool isAttack = false;
    public bool isSkill = false;
    protected bool isChase;
    protected bool isDamage = false;
    protected bool isDead = false;
    protected float Skill1CanUse = 0;
    protected float Skill2CanUse = 0;
    protected float Skill3CanUse = 0;

    // �ִϸ��̼ǿ�
    protected static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    protected static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    protected static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int GetHitHash = Animator.StringToHash("GetHit");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    // ���ݹ��� �� �����̴� �뵵
    private float flashDuration = 0.1f;
    private int flashCount = 2;
    private List<Renderer> renderers;

    protected virtual void Awake()
    {
        isAttack = false;
        player = GameObject.FindWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = distance;
        random = new System.Random();
        exclamationMark.SetActive(false);
        anim = GetComponent<Animator>();
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        damageText.gameObject.SetActive(false);
        originalPosition = damageText.transform.position;
        originalColor = damageText.color;
    }

    // ������ ���� ����
    public void ChangeState(MonsterState state)
    {
        if (currentState == MonsterState.Patrol && state == MonsterState.Chase)
        {
            StartCoroutine(ShowExclamationMarkForSeconds(0.5f));
        }
        currentState = state;
    }

    // ���Ͱ� �ֺ��� ��ȸ
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
        {
            switchTime -= Time.deltaTime;
        }
    }

    // ���͸� �÷��̾� ������ ȸ��
    protected void RotateMonsterToCharacter()
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

    // ������ �÷��̾� ���� ���� ��� �Լ�
    void Chase()
    {
        if (currentState == MonsterState.Die || isAttack)
        {
            return;
        }
        SetNavSpeed(Move_Speed);
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
    protected virtual void Attack()
    {
        // ��Ÿ���� 0������ ���� �� �����ϰ� ���
        int skillIndex = random.Next(0, 2); // NumberOfSkills�� 2�� ����

        switch (skillIndex)
        {
            case 0:  // �⺻ ����
                if (Skill1CanUse <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attack1Radius)
                {
                    MonsterAttackStart();
                    Skill1();
                    Skill1CanUse = SkillCoolTime1;
                }
                else
                {
                    anim.SetTrigger(BattleIdleHash);
                }
                break;
            case 1: // ��ų ����1
                if (Skill2CanUse <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attack2Radius)
                {
                    MonsterAttackStart();
                    isSkill = true;
                    Skill2();
                    Skill2CanUse = SkillCoolTime2;
                }
                else
                {
                    anim.SetTrigger(BattleIdleHash);
                }
                break;
        }
    }

    // ���� �ִϸ��̼�
    protected void Skill1()
    {
        anim.SetTrigger(Attack01Hash);
        Invoke("OnFirstAttackAnimationEnd", firstAttackAnimationLength);
    }

    protected virtual void Skill2()
    {
        anim.SetTrigger(Attack02Hash);
        Invoke("OnSecondAttackAnimationEnd", secondAttackAnimationLength);
    }

    // ������ ���ݿ� ���� ������ ����
    public virtual int Damage(int skillIndex)
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
            default:
                // �������� ���� ��ų�̸� damage 0
                break;
        }
        return damage;
    }

    // ���� �ð� ���� ����ǥ�� ���� �ڷ�ƾ
    IEnumerator ShowExclamationMarkForSeconds(float seconds)
    {
        exclamationMark.SetActive(true);
        yield return new WaitForSeconds(seconds);
        exclamationMark.SetActive(false);
    }

    // ������ State ������Ʈ
    protected virtual void Update()
    {
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

        RotateMonsterToCharacter();
    }

    // ������ �ǰ� ��Ȳ ó�� �Լ�
    public void TakeDamage(int damage)
    {
        if (isDead) return; // �̹� ���� ������ ��� �������� ���� ����

        if (currentState != MonsterState.Patrol)
        {
            ChangeState(MonsterState.Chase);
        }

        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            // ���� ������ = �÷��̾��� ���ݵ����� * (1 - ����%)
            int finalDamage = Mathf.RoundToInt(damage * (1 - Defense));
            Cur_HP -= finalDamage;
            lastDamagedTime = Time.time;
            anim.SetTrigger(GetHitHash);
            Flash();
            ShowDamageText(finalDamage);
        }

        if (Cur_HP <= 0)
        {
            Die();
        }
    }
    void ShowDamageText(int damage)
    {
        // ������ Ȱ��ȭ�Ǿ� �ִ� �ؽ�Ʈ�� ��ġ�� ���� �̵���ŵ�ϴ�.
        damageText.transform.localPosition += new Vector3(0, 30, 0);

        // ������ �ؽ�Ʈ�� �����մϴ�.
        damageText.text = damage.ToString();

        // �ؽ�Ʈ ��ġ�� �ʱ� ��ġ�� �����մϴ�.
        damageText.transform.localPosition = originalPosition;
        // �ؽ�Ʈ ������ �ʱ� �������� �����մϴ�.
        damageText.color = originalColor;
        // �ؽ�Ʈ�� Ȱ��ȭ�մϴ�.
        damageText.gameObject.SetActive(true);

        StartCoroutine(AnimateDamageText());
    }

    System.Collections.IEnumerator AnimateDamageText()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 startPosition = originalPosition;
        Vector3 endPosition = startPosition + new Vector3(0, 30, 0); // 1 �ȼ� ���� �̵�

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // �ؽ�Ʈ ��ġ �̵�
            damageText.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            // �ؽ�Ʈ ���� ��ȭ (��������)
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);

            yield return null;
        }

        damageText.gameObject.SetActive(false);
    }

    void ClearDamageText()
    {
        damageText.text = "";
    }

    // �ǰ� �� ���� ���� ȿ�� ��� �Լ�
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    // ���� ȿ�� ��� �ڷ�ƾ
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

    // ��� ó�� �Լ�
    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        nav.isStopped = true;
        gameObject.layer = 11;
        ChangeState(MonsterState.Die);
        anim.SetTrigger(DieHash);
        anim.SetBool(RunHash, false);
        Invoke("DestroyObject", 2.0f);
        Collider collider = GetComponent<Collider>();
        if (collider != null) { collider.enabled = false; }

        spawner.aliveCount--;
        spawner.CheckAliveCount();
        spawner.NotifyAliveCountChanged();  // Notify subscribers of the alive count change
        Debug.Log("���� ����:" + spawner.aliveCount);
    }

    // Monster deletion
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected void MonsterAttackStart()
    {
        isAttack = true;
        anim.SetBool(RunHash, false); // ���� ���� �� �̵� �ִϸ��̼� ��Ȱ��ȭ
        //nav.isStopped = true; // ���� ���� �� NavMeshAgent ����
    }

    void MonsterAttackEnd()
    {
        isAttack = false;
        anim.SetBool(RunHash, true); // ������ ������ �̵� �ִϸ��̼� Ȱ��ȭ
        //nav.isStopped = false; // ������ ������ NavMeshAgent �ٽ� Ȱ��ȭ
    }

    // ù ��° ���� �ִϸ��̼��� ���� �� ȣ��� �Լ�
    public void OnFirstAttackAnimationEnd()
    {
        isAttack = false;
        isSkill = false;
        anim.SetBool(RunHash, true); // ������ ������ �̵� �ִϸ��̼� Ȱ��ȭ
        //nav.isStopped = false; // ������ ������ NavMeshAgent �ٽ� Ȱ��ȭ
    }

    // �� ��° ���� �ִϸ��̼��� ���� �� ȣ��� �Լ�
    public void OnSecondAttackAnimationEnd()
    {
        isAttack = false;
        isSkill = false;
        anim.SetBool(RunHash, true); // ������ ������ �̵� �ִϸ��̼� Ȱ��ȭ
        //nav.isStopped = false; // ������ ������ NavMeshAgent �ٽ� Ȱ��ȭ
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (isAttack)
        {
            if (isSkill)
            {
                if (other.gameObject.TryGetComponent(out Player player))
                {
                    player.TakeDamage(Damage(1)); // monster�� ������ 1�� = ��ų ����
                }
            }
            else
            {
                if (other.gameObject.TryGetComponent(out Player player))
                {
                    player.TakeDamage(Damage(0)); // monster�� ������ 0�� = �⺻ ����
                }
            }
        }
    }
}


