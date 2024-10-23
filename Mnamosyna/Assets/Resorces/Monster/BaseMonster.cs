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

    public float invincibleTime = 0.5f; // 공격받은 후 무적 시간
    protected float switchTime = 1.0f;
    // 공격받을 때 깜빡이는 용도
    protected float flashDuration = 0.1f;
    protected int flashCount = 5;

    protected float rotationSpeed = 5.0f;

    protected float lastDamagedTime;

    public delegate void DeathHandler();
    public delegate void Action();
    public event DeathHandler OnDeath;

    public Spawner spawner;
    public GameObject exclamationMark;
    public Text damageText;
    protected Vector3 originalPosition;
    protected Color originalColor;

    protected GameObject player;

    protected Rigidbody rigid; // 리지드 바디
    protected NavMeshAgent nav; // 내브메쉬
    protected System.Random random; 
    protected Animator anim;
    protected List<Renderer> renderers;

    protected const float WAIT_TIME = 0.2f;
    protected bool isChase;

    protected bool isDamage = false;
    protected bool isDead = false;

    public bool isAttack = false;
    public bool isSkill01 = false;
    protected float AttackCanUse;
    protected float Skill01CanUse;

    // 애니메이션용
    protected static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    protected static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    public int attack01Hash;
    protected static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    public int attack02Hash;
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

        AttackCanUse = AttackCoolTime;
        Skill01CanUse = SkillCoolTime1;
    }
    void Start()
    {
        StartCoroutine(ChangeToChaseAfterDelay(0.5f));
    }


    // 몬스터의 상태 변경
    public virtual void ChangeState(State state)
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

    // 몬스터를 플레이어 쪽으로 회전
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


    // 몬스터의 플레이어 추적 상태 출력 함수
    void Chase()
    {
        if (currentState == State.Die || isAttack || isSkill01)
        {
            return;
        }
        SetNavSpeed(Move_Speed);
        anim.SetBool(RunHash, true);
        nav.SetDestination(player.transform.position);
    }

    // NavMeshAgent의 속도를 몬스터의 이동 속도로 설정
    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            nav.speed = speed;
        }
    }

    // Attack 상태일 때 스킬 쿨타임에 따른 공격 실행 함수
    protected virtual void Attack()
    {

        int skillIndex = random.Next(0, 2); // NumberOfSkills는 2로 설정

        switch (skillIndex)
        {
            case 0:  // 기본 공격
                TryAttack(ref AttackCanUse, AttackRadius, AttackCoolTime, Attack01);
                break;
            case 1: // 스킬 공격
                TryAttack(ref Skill01CanUse, Skill01Radius, SkillCoolTime1, Attack02, true);
                break;
        }
    }

    protected void TryAttack(ref float attackCooldown, float attackRadius, float skillCoolTime, Action attackAction, bool setIsAttack02 = false)
    {
        if (attackCooldown <= 0 && (transform.position - player.transform.position).sqrMagnitude <= attackRadius * attackRadius)
        {
            MonsterAttackStart();
            if (setIsAttack02) isSkill01 = true;
            attackAction();
            attackCooldown = skillCoolTime;
        }
        else
        {
            anim.SetTrigger(BattleIdleHash);
        }
    }


    // 공격 애니메이션
    protected void Attack01()
    {
        anim.SetTrigger(Attack01Hash);
        float delay = GetAnimationLength(attack01Hash); // 공격 애니메이션의 길이

        // Attack1의 애니메이션 종료 후 실행할 메서드를 지연 호출
        StartCoroutine(DelayedAction(delay, OnFirstAttackAnimationEnd));
    }

    protected virtual void Attack02()
    {
        anim.SetTrigger(Attack02Hash);
        float delay = GetAnimationLength(attack02Hash); // 공격 애니메이션의 길이

        // Attack2의 애니메이션 종료 후 실행할 메서드를 지연 호출
        StartCoroutine(DelayedAction(delay, OnSecondAttackAnimationEnd));
    }

    // 일정 시간 후에 지연하여 실행할 메서드를 처리하는 코루틴
    protected IEnumerator DelayedAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    // 애니메이션의 길이를 가져오는 메서드
    protected float GetAnimationLength(int hash)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == hash)
        {
            return stateInfo.length;
        }
        else
        {
            return 0f;
        }
    }

    // 몬스터의 공격에 따른 데미지 배정
    public virtual int Damage(int skillIndex)
    {
        int damage = 0;

        switch (skillIndex)
        {
            case 0: // 기본 공격
                damage = ATK;
                break;
            case 1: // 스킬 공격
                damage = Skill01;
                break;
            default:
                // 지정되지 않은 스킬이면 damage 0
                break;
        }
        return damage;
    }

    // 일정 시간 동안 느낌표를 띄우는 코루틴
    IEnumerator ShowExclamationMarkForSeconds(float seconds)
    {
        exclamationMark.SetActive(true);
        yield return new WaitForSeconds(seconds);
        exclamationMark.SetActive(false);
    }

    // 몬스터의 State 업데이트
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

        // 1초마다 스킬 쿨 감소
        AttackCanUse -= Time.deltaTime;
        Skill01CanUse -= Time.deltaTime;

        RotateMonsterToCharacter();

        if (player != null)
        {
            nav.SetDestination(player.transform.position);
        }
    }

    // 몬스터의 피격 상황 처리 함수
    public void TakeDamage(int damage)
    {
        if (isDead) return; // 이미 죽은 몬스터인 경우 데미지를 받지 않음

        if (currentState != State.Idle)
        {
            ChangeState(State.Chase);
        }

        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            // 최종 데미지 = 플레이어의 공격데미지 * (1 - 방어력%)
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

    void ClearDamageText()
    {
        damageText.text = "";
    }

    // 피격 시 몬스터 점멸 효과 출력 함수
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    // 점멸 효과 출력 코루틴
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

    // 사망 처리 함수
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

        Debug.Log("남은 몬스터:" + spawner.aliveCount);
    }

    // Monster deletion
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected void MonsterAttackStart()
    {
        isAttack = true;
        anim.SetBool(RunHash, false); // 공격 중일 때 이동 애니메이션 비활성화
        //nav.isStopped = true; // 공격 중일 때 NavMeshAgent 정지
    }

    void MonsterAttackEnd()
    {
        isAttack = false;
        anim.SetBool(RunHash, true); // 공격이 끝나면 이동 애니메이션 활성화
        //nav.isStopped = false; // 공격이 끝나면 NavMeshAgent 다시 활성화
    }

    // 첫 번째 공격 애니메이션이 끝날 때 호출될 함수
    public void OnFirstAttackAnimationEnd()
    {
        isAttack = false;
        anim.SetBool(RunHash, true); // 공격이 끝나면 이동 애니메이션 활성화
        //nav.isStopped = false; // 공격이 끝나면 NavMeshAgent 다시 활성화
    }

    // 두 번째 공격 애니메이션이 끝날 때 호출될 함수
    public void OnSecondAttackAnimationEnd()
    {
        isSkill01 = false;
        anim.SetBool(RunHash, true); // 공격이 끝나면 이동 애니메이션 활성화
        //nav.isStopped = false; // 공격이 끝나면 NavMeshAgent 다시 활성화
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("플레이어 감지됨"); // 추가된 로그
            if (isAttack)
            {
                if (isSkill01)
                {
                    player.TakeDamage(Damage(1)); // 스킬 공격
                }
                else
                {
                    player.TakeDamage(Damage(0)); // 기본 공격
                }
            }
        }
    }


}