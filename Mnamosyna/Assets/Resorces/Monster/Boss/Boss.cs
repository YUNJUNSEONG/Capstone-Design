using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : BaseMonster
{
    [Header("공격 관련")]
    [SerializeField]
    protected int Skill02;
    [SerializeField]
    protected int Skill03;

    [Header("공격 쿨타임")]
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

    // 애니메이션용
    protected static readonly int ScreamHash = Animator.StringToHash("Scream");
    protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    public int attack03Hash;
    protected static readonly int Attack04Hash = Animator.StringToHash("Attack04");
    public int attack04Hash;

    // 공격 관련 필드
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

        // 1초마다 스킬 쿨 감소
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
    

    // NavMeshAgent의 속도를 몬스터의 이동 속도로 설정
    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            nav.speed = speed;
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

    protected override void Die()
    {
        isDead = true;
        nav.isStopped = true;
        gameObject.layer = 11;
        ChangeState(State.Die);
        anim.SetTrigger(DieHash);
        anim.SetBool(RunHash, false);


        // 콜라이더 비활성화는 2초 뒤에 처리합니다.
        Invoke("DisableCollider", 2.0f);
        Invoke("DestroyObject", 3.0f); // 4초 후 객체를 삭제합니다.

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
        // 패턴 1 애니메이션 재생
        anim.SetTrigger("Attack02");
        // 패턴 1의 공격 로직
        player.GetComponent<Player>().TakeDamage(Skill01);
        yield return new WaitForSeconds(Skill01CanUse);
        canUsePattern1 = true;
    }

    IEnumerator UsePattern2()
    {
        canUsePattern2 = false;
        // 패턴 2 애니메이션 재생
        anim.SetTrigger("Attack03");
        // 패턴 2의 공격 로직
        player.GetComponent<Player>().TakeDamage(Skill02);
        yield return new WaitForSeconds(Skill02CanUse);
        canUsePattern2 = true;
    }

    IEnumerator UsePattern3()
    {
        canUsePattern3 = false;
        // 패턴 3 애니메이션 재생
        anim.SetTrigger("Attack04");
        // 패턴 3의 공격 로직
        player.GetComponent<Player>().TakeDamage(Skill03);
        yield return new WaitForSeconds(Skill03CanUse);
        canUsePattern3 = true;
    }


}
