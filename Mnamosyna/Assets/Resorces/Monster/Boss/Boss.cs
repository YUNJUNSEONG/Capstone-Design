using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RedDragon : BossStat
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Die
    }
    protected State currentState;

    public float invincibleTime = 0.5f; // 공격받은 후 무적 시간
    protected float switchTime = 1.0f;
    // 공격받을 때 깜빡이는 효과
    protected float flashDuration = 0.1f;
    protected int flashCount = 5;

    protected float rotationSpeed = 5.0f;

    private float lastDamagedTime;

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

    public BossSpawner spawner;
    public Text damageText;
    private Vector3 originalPosition;
    private Color originalColor;

    protected GameObject player;

    protected Rigidbody rigid; // 리지드 바디
    protected NavMeshAgent nav; // 내브메쉬
    protected System.Random random;
    protected Animator anim;
    private List<Renderer> renderers;

    protected const float WAIT_TIME = 0.2f;
    protected bool isChase;

    protected bool isDamage = false;
    protected bool isDead = false;

    private bool canBasicAttack = true;
    private bool canUsePattern1 = true;
    private bool canUsePattern2 = true;
    private bool canUsePattern3 = true;

    protected float AttackCanUse;
    protected float Skill01CanUse;
    protected float Skill02CanUse;
    protected float Skill03CanUse;

    protected float pattern1Cooldown;
    protected float pattern2Cooldown;
    protected float pattern3Cooldown;

    private float lastPattern1Time;
    private float lastPattern2Time;
    private float lastPattern3Time;

    // 애니메이션용
    protected static readonly int ScreamHash = Animator.StringToHash("Scream");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int GetHitHash = Animator.StringToHash("GetHit");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    // 공격 관련 필드
    private float attackRange;
    private float attackCooldown;
    private float lastAttackTime;

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        random = new System.Random();
        anim = GetComponent<Animator>();
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());

        originalPosition = transform.position;
        originalColor = renderers[0].material.color;

        attackRange = ApproachRadius;
        attackCooldown = AttackCoolTime;
        pattern1Cooldown = SkillCoolTime1;
        pattern2Cooldown = SkillCoolTime2;
        pattern3Cooldown = SkillCoolTime3;
    }

    protected virtual void Update()
    {
        if (Cur_HP <= 0)
        {
            Die();
            return;
        }

        switch (currentState)
        {
            case State.Idle:
                UpdateIdleState();
                break;
            case State.Chase:
                UpdateChaseState();
                break;
            case State.Attack:
                UpdateAttackState();
                break;
            case State.Die:
                UpdateDieState();
                break;
        }

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
    }

    protected virtual void UpdateIdleState()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 10f)
        {
            currentState = State.Chase;
            anim.SetBool(RunHash, true);
        }
    }

    protected virtual void UpdateChaseState()
    {
        nav.SetDestination(player.transform.position);

        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            currentState = State.Attack;
            anim.SetBool(RunHash, false);
        }
    }

    protected virtual void UpdateAttackState()
    {
        //nav.SetDestination(transform.position); // 보스가 공격 중일 때는 멈춥니다.
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

    protected virtual void UpdateDieState()
    {

    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 이미 죽은 몬스터인 경우 데미지를 받지 않음

        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            // 최종 데미지 = 플레이어의 공격데미지 * (1 - 방어력%)
            int finalDamage = Mathf.RoundToInt(damage * (1 - Defense));
            Cur_HP -= finalDamage;
            lastDamagedTime = Time.time;
            anim.SetTrigger(GetHitHash);
            StartCoroutine(FlashOnHit());
            ShowDamageText(finalDamage);
        }

        if (Cur_HP <= 0)
        {
            Die();
        }
    }

    void ShowDamageText(int damage)
    {
        // 기존에 활성화되어 있는 텍스트의 위치를 위로 이동시킵니다.
        damageText.transform.localPosition += new Vector3(0, 30, 0);

        // 데미지 텍스트를 갱신합니다.
        damageText.text = damage.ToString();

        // 텍스트 위치를 초기 위치로 설정합니다.
        damageText.transform.localPosition = originalPosition;
        // 텍스트 색상을 초기 색상으로 설정합니다.
        damageText.color = originalColor;
        // 텍스트를 활성화합니다.
        damageText.gameObject.SetActive(true);

        StartCoroutine(AnimateDamageText());
    }

    System.Collections.IEnumerator AnimateDamageText()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 startPosition = originalPosition;
        Vector3 endPosition = startPosition + new Vector3(0, 30, 0); // 1 픽셀 위로 이동

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 텍스트 위치 이동
            damageText.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            // 텍스트 색상 변화 (투명해짐)
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);

            yield return null;
        }

        damageText.gameObject.SetActive(false);
    }

    protected virtual void Die()
    {
        isDead = true;
        currentState = State.Die;
        anim.SetTrigger(DieHash);

        OnDeath?.Invoke();

        spawner.aliveCount--;
        spawner.CheckAliveCount();
        spawner.NotifyAliveCountChanged();

        // 콜라이더 비활성화는 2초 뒤에 처리합니다.
        Invoke("DisableCollider", 2.0f);
        Invoke("DestroyObject", 4.0f); // 4초 후 객체를 삭제합니다.
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

    /*
    protected virtual void OnTriggerStay(Collider other)
    {
        if (isDamage)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDamage(Damage(1));
            }
        }
    }*/

    protected virtual IEnumerator BasicAttack()
    {
        canBasicAttack = false;
        anim.SetTrigger("BasicAttack");

        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            player.GetComponent<Player>().TakeDamage(ATK);
        }

        yield return new WaitForSeconds(AttackCanUse);
        canBasicAttack = true;
    }

    IEnumerator UsePattern1()
    {
        canUsePattern1 = false;
        // 패턴 1 애니메이션 재생
        anim.SetTrigger("Pattern1");
        // 패턴 1의 공격 로직
        player.GetComponent<Player>().TakeDamage(Skill01);
        yield return new WaitForSeconds(Skill01CanUse);
        canUsePattern1 = true;
    }

    IEnumerator UsePattern2()
    {
        canUsePattern2 = false;
        // 패턴 2 애니메이션 재생
        anim.SetTrigger("Pattern2");
        // 패턴 2의 공격 로직
        player.GetComponent<Player>().TakeDamage(Skill02);
        yield return new WaitForSeconds(Skill02CanUse);
        canUsePattern2 = true;
    }

    IEnumerator UsePattern3()
    {
        canUsePattern3 = false;
        // 패턴 3 애니메이션 재생
        anim.SetTrigger("Pattern3");
        // 패턴 3의 공격 로직
        player.GetComponent<Player>().TakeDamage(Skill03);
        yield return new WaitForSeconds(Skill03CanUse);
        canUsePattern3 = true;
    }
}
