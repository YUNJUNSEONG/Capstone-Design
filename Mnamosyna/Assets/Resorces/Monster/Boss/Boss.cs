using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : BaseMonster
{
    [Header("공격 관련")]
    [SerializeField] protected int Skill02;
    [SerializeField] protected int Skill03;

    [Header("공격 쿨타임")]
    [SerializeField] protected float SkillCoolTime2;
    [SerializeField] protected float SkillCoolTime3;

    [Header("공격 이펙트")]
    [SerializeField] private GameObject skill01EffectPrefab;
    [SerializeField] private GameObject skill02EffectPrefab;
    [SerializeField] private GameObject skill03EffectPrefab;

    // 이펙트 위치를 위한 참조
    [SerializeField] private Transform skill01EffectSpawnPoint;
    [SerializeField] private Transform skill02EffectSpawnPoint;
    [SerializeField] private Transform skill03EffectSpawnPoint;


    protected float Skill02CanUse;
    protected float Skill03CanUse;

    private bool canBasicAttack = true;
    private bool canUsePattern1 = true;
    private bool canUsePattern2 = true;
    private bool canUsePattern3 = true;

    protected float pattern1Cooldown;
    protected float pattern2Cooldown;
    protected float pattern3Cooldown;

    private float lastPattern1Time;
    private float lastPattern2Time;
    private float lastPattern3Time;

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

        InitializeEffect(skill01EffectPrefab, skill01EffectSpawnPoint);
        InitializeEffect(skill02EffectPrefab, skill02EffectSpawnPoint);
        InitializeEffect(skill03EffectPrefab, skill03EffectSpawnPoint);
    }
    private void InitializeEffect(GameObject effectPrefab, Transform spawnPoint)
    {
        if (effectPrefab != null && spawnPoint != null)
        {
            effectPrefab = Instantiate(effectPrefab, spawnPoint.position, Quaternion.identity);
            effectPrefab.SetActive(false);
        }
    }
    protected override void Update()
    {
        base.Update();
        RotateMonsterToCharacter();

        // 기본 공격 실행
        if (canBasicAttack && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(BasicAttack());
        }

        // 패턴 사용
        HandlePatternUsage();
    }

    private void HandlePatternUsage()
    {
        if (Cur_HP <= Max_HP * 0.8f && Cur_HP > Max_HP * 0.5f && canUsePattern1 && Time.time >= lastPattern1Time + pattern1Cooldown)
        {
            StartCoroutine(UsePattern1());
            lastPattern1Time = Time.time;
        }
        else if (Cur_HP <= Max_HP * 0.5f && Cur_HP > Max_HP * 0.3f && canUsePattern2 && Time.time >= lastPattern2Time + pattern2Cooldown)
        {
            StartCoroutine(UsePattern2());
            lastPattern2Time = Time.time;
        }
        else if (Cur_HP <= Max_HP * 0.3f && canUsePattern3 && Time.time >= lastPattern3Time + pattern3Cooldown)
        {
            StartCoroutine(UsePattern3());
            lastPattern3Time = Time.time;
        }
    }

    protected override void Die()
    {
        if (isDead) return;

        isDead = true;
        nav.isStopped = true;
        gameObject.layer = 11;
        ChangeState(State.Die);
        anim.SetTrigger(DieHash);
        anim.SetBool(RunHash, false);

        Invoke("DisableCollider", 2.0f);
        Invoke("DestroyObject", 3.0f);

        spawner.aliveCount--;
        spawner.CheckAliveCount();
        spawner.NotifyAliveCountChanged();
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

    private void ShowEffect(GameObject effectPrefab, Transform spawnPoint)
    {
        if (effectPrefab != null)
        {
            effectPrefab.transform.position = spawnPoint.position;
            effectPrefab.SetActive(true);
            StartCoroutine(HideEffectAfterDelay(effectPrefab, 2.0f)); // 이펙트를 2초 후에 비활성화
        }
    }

    private IEnumerator HideEffectAfterDelay(GameObject effectPrefab, float delay)
    {
        yield return new WaitForSeconds(delay);
        HideEffect(effectPrefab);
    }


    private void HideEffect(GameObject effectPrefab)
    {
        if (effectPrefab != null)
        {
            effectPrefab.SetActive(false);
        }
    }

    IEnumerator UsePattern1()
    {
        canUsePattern1 = false;
        anim.SetTrigger("Attack02");

        ShowEffect(skill01EffectPrefab, skill01EffectSpawnPoint);

        player.GetComponent<Player>().TakeDamage(Skill01);
        yield return new WaitForSeconds(Skill01CanUse);
        canUsePattern1 = true;
    }

    IEnumerator UsePattern2()
    {
        canUsePattern2 = false;
        anim.SetTrigger("Attack03");

        ShowEffect(skill02EffectPrefab, skill02EffectSpawnPoint);

        player.GetComponent<Player>().TakeDamage(Skill02);
        yield return new WaitForSeconds(Skill02CanUse);
        canUsePattern2 = true;
    }

    IEnumerator UsePattern3()
    {
        canUsePattern3 = false;
        anim.SetTrigger("Attack04");

        ShowEffect(skill03EffectPrefab, skill03EffectSpawnPoint);

        player.GetComponent<Player>().TakeDamage(Skill03);
        yield return new WaitForSeconds(Skill03CanUse);
        canUsePattern3 = true;
    }
}
