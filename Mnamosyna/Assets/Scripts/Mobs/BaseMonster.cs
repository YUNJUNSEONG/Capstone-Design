using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseMonster : MobStat
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Die
    }
    protected State currentState;

    public float invincibleTime = 0.5f; // ���ݹ��� �� ���� �ð�
    protected float switchTime = 1.0f;
    // ���ݹ��� �� �����̴� �뵵
    protected float flashDuration = 0.1f;
    protected int flashCount = 5;

    protected float rotationSpeed = 5.0f;

    private float lastDamagedTime;

    public delegate void DeathHandler();
    public delegate void Action();
    public event DeathHandler OnDeath;

    public Spawner spawner;
    public BossSpawner bossSpawner;
    public GameObject exclamationMark;
    public Text damageText;
    private Vector3 originalPosition;
    private Color originalColor;

    protected GameObject player;

    protected Rigidbody rigid; // ������ �ٵ�
    protected NavMeshAgent nav; // ����޽�
    protected System.Random random; 
    protected Animator anim;
    private List<Renderer> renderers;

    protected const float WAIT_TIME = 0.2f;
    protected bool isChase;

    protected bool isDamage = false;
    protected bool isDead = false;

    public bool isAttack01 = false;
    public bool isAttack02 = false;
    protected float Attack01CanUse;
    protected float Attack02CanUse;

    // �ִϸ��̼ǿ�
    protected static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    protected static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    public int attack01Hash;
    protected static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    public int attack02Hash;
    //protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int GetHitHash = Animator.StringToHash("GetHit");
    protected static readonly int DieHash = Animator.StringToHash("Die");


    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = approach;
        random = new System.Random();
        exclamationMark.SetActive(false);
        anim = GetComponent<Animator>();
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        damageText.gameObject.SetActive(false);
        originalPosition = damageText.transform.position;
        originalColor = damageText.color;

        currentState = State.Idle;

        Attack01CanUse = SkillCoolTime1;
        Attack02CanUse = SkillCoolTime2;
    }
    void Start()
    {
        StartCoroutine(ChangeToChaseAfterDelay(0.5f));
    }


    // ������ ���� ����
    public void ChangeState(State state)
    {
        if (currentState == State.Idle && state == State.Chase)
        {
            StartCoroutine(ShowExclamationMarkForSeconds(0.5f));
        }
        currentState = state;
    }

    private IEnumerator ChangeToChaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(State.Chase);
    }

    // ���͸� �÷��̾� ������ ȸ��
    protected void RotateMonsterToCharacter()
    {
        if (currentState == State.Die)
        {
            return;
        }

        Vector3 directionToCharacter = player.transform.position - transform.position;
        if (directionToCharacter != Vector3.zero)
        {
            directionToCharacter.y = 0;
            Quaternion rotationToCharacter = Quaternion.LookRotation(directionToCharacter);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToCharacter, Time.deltaTime * rotationSpeed);
        }
    }


    // ������ �÷��̾� ���� ���� ��� �Լ�
    void Chase()
    {
        if (currentState == State.Die || isAttack01 || isAttack02)
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

        int skillIndex = random.Next(0, 2); // NumberOfSkills�� 2�� ����

        switch (skillIndex)
        {
            case 0:  // �⺻ ����
                TryAttack(ref Attack01CanUse, attack1Radius, SkillCoolTime1, Attack1);
                break;
            case 1: // ��ų ����
                TryAttack(ref Attack02CanUse, attack2Radius, SkillCoolTime2, Attack2, true);
                break;
        }
    }

    protected void TryAttack(ref float attackCooldown, float attackRadius, float skillCoolTime, Action attackAction, bool setIsAttack02 = false)
    {
        if (attackCooldown <= 0 && (transform.position - player.transform.position).sqrMagnitude <= attackRadius * attackRadius)
        {
            MonsterAttackStart();
            if (setIsAttack02) isAttack02 = true;
            attackAction();
            attackCooldown = skillCoolTime;
        }
        else
        {
            anim.SetTrigger(BattleIdleHash);
        }
    }


    // ���� �ִϸ��̼�
    protected void Attack1()
    {
        anim.SetTrigger(Attack01Hash);
        float delay = GetAnimationLength(attack01Hash); // ���� �ִϸ��̼��� ����

        // Attack1�� �ִϸ��̼� ���� �� ������ �޼��带 ���� ȣ��
        StartCoroutine(DelayedAction(delay, OnFirstAttackAnimationEnd));
    }

    protected virtual void Attack2()
    {
        anim.SetTrigger(Attack02Hash);
        float delay = GetAnimationLength(attack02Hash); // ���� �ִϸ��̼��� ����

        // Attack2�� �ִϸ��̼� ���� �� ������ �޼��带 ���� ȣ��
        StartCoroutine(DelayedAction(delay, OnSecondAttackAnimationEnd));
    }

    // ���� �ð� �Ŀ� �����Ͽ� ������ �޼��带 ó���ϴ� �ڷ�ƾ
    protected IEnumerator DelayedAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
    // �ִϸ��̼��� ���̸� �������� �޼���
    protected float GetAnimationLength(int hash)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == hash)
        {
            return stateInfo.length;
        }
        else
        {
            Debug.LogWarning("Animation with hash " + hash + " not found in the current state.");
            return 0f;
        }
    }

    // ������ ���ݿ� ���� ������ ����
    public virtual int Damage(int skillIndex)
    {
        int damage = 0;

        switch (skillIndex)
        {
            case 0: // �⺻ ����
                damage = ATK1;
                Debug.Log("�Ϲ� ���� ������ : "+ATK1);
                break;
            case 1: // ��ų ����
                damage = ATK2;
                Debug.Log("��ų ���� ������ : "+ATK2);
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
        if (currentState == State.Die)
        {
            return;
        }

        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }

        // 1�ʸ��� ��ų �� ����
        Attack01CanUse -= Time.deltaTime;
        Attack02CanUse -= Time.deltaTime;

        RotateMonsterToCharacter();
    }

    // ������ �ǰ� ��Ȳ ó�� �Լ�
    public void TakeDamage(int damage)
    {
        if (isDead) return; // �̹� ���� ������ ��� �������� ���� ����

        if (currentState != State.Idle)
        {
            ChangeState(State.Chase);
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
        ChangeState(State.Die);
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
        isAttack01 = true;
        anim.SetBool(RunHash, false); // ���� ���� �� �̵� �ִϸ��̼� ��Ȱ��ȭ
        //nav.isStopped = true; // ���� ���� �� NavMeshAgent ����
    }

    void MonsterAttackEnd()
    {
        isAttack01 = false;
        anim.SetBool(RunHash, true); // ������ ������ �̵� �ִϸ��̼� Ȱ��ȭ
        //nav.isStopped = false; // ������ ������ NavMeshAgent �ٽ� Ȱ��ȭ
    }

    // ù ��° ���� �ִϸ��̼��� ���� �� ȣ��� �Լ�
    public void OnFirstAttackAnimationEnd()
    {
        isAttack01 = false;
        anim.SetBool(RunHash, true); // ������ ������ �̵� �ִϸ��̼� Ȱ��ȭ
        //nav.isStopped = false; // ������ ������ NavMeshAgent �ٽ� Ȱ��ȭ
    }

    // �� ��° ���� �ִϸ��̼��� ���� �� ȣ��� �Լ�
    public void OnSecondAttackAnimationEnd()
    {
        isAttack02 = false;
        anim.SetBool(RunHash, true); // ������ ������ �̵� �ִϸ��̼� Ȱ��ȭ
        //nav.isStopped = false; // ������ ������ NavMeshAgent �ٽ� Ȱ��ȭ
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (isAttack01)
        {
            if (isAttack02)
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

