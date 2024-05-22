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
    protected MonsterState currentState;

    public float invincibleTime = 1f; // 공격받은 후 무적 시간
    private float lastDamagedTime;
    public Spawner spawner;
    public GameObject exclamationMark;

    // 첫 번째 공격 애니메이션의 길이 (초 단위)
    public float firstAttackAnimationLength;

    // 두 번째 공격 애니메이션의 길이 (초 단위)
    public float secondAttackAnimationLength;

    protected GameObject player;
    protected float switchTime = 2.0f;

    protected Rigidbody rigid; // 리지드 바디
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

    // 애니메이션용
    protected static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    protected static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    protected static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int GetHitHash = Animator.StringToHash("GetHit");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    // 공격받을 때 깜빡이는 용도
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
    }

    // 몬스터의 상태 변경
    public void ChangeState(MonsterState state)
    {
        if (currentState == MonsterState.Patrol && state == MonsterState.Chase)
        {
            StartCoroutine(ShowExclamationMarkForSeconds(3.0f));
        }
        currentState = state;
    }

    // 몬스터가 주변을 배회
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

    // 몬스터를 플레이어 쪽으로 회전
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

    // 몬스터의 플레이어 추적 상태 출력 함수
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
        // 쿨타임이 0이하인 공격 중 랜덤하게 출력
        int skillIndex = random.Next(0, 2); // NumberOfSkills는 2로 설정

        switch (skillIndex)
        {
            case 0:  // 기본 공격
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
            case 1: // 스킬 공격1
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

    // 공격 애니메이션
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

    // 몬스터의 공격에 따른 데미지 배정
    public virtual int Damage(int skillIndex)
    {
        int damage = 0;

        switch (skillIndex)
        {
            case 0: // 기본 공격
                damage = ATK;
                break;
            case 1: // 스킬 공격1
                damage = Skill_ATK1;
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

        // 1초마다 스킬 쿨 감소
        Skill1CanUse -= Time.deltaTime;
        Skill2CanUse -= Time.deltaTime;
        Skill3CanUse -= Time.deltaTime;

        RotateMonsterToCharacter();
    }

    // 몬스터의 피격 상황 처리 함수
    public void TakeDamage(int damage)
    {
        if (isDead) return; // 이미 죽은 몬스터인 경우 데미지를 받지 않음

        if (currentState != MonsterState.Patrol)
        {
            ChangeState(MonsterState.Chase);
        }

        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            // 최종 데미지 = 플레이어의 공격데미지 * (1 - 방어력%)
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
        gameObject.layer = 11;
        ChangeState(MonsterState.Die);
        anim.SetTrigger(DieHash);
        nav.isStopped = true;
        anim.SetBool(RunHash, false);
        Invoke("DestroyObject", 2.0f);
        Collider collider = GetComponent<Collider>();
        if (collider != null) { collider.enabled = false; }

        if (spawner != null)
        {
            spawner.aliveCount--; // 여기서는 1씩만 감소해야 합니다.
            spawner.CheckAliveCount();
            Debug.Log("남은 몬스터:" + spawner.aliveCount);
        }
        else
        {
            // Debug.LogError("몬스터 스크립트에서 몬스터 스포너 못 찾아옴");
        }
    }

    // 몬스터 삭제
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected void MonsterAttackStart()
    {
        isAttack = true;
        anim.SetBool(RunHash, false); // 공격 중일 때 이동 애니메이션 비활성화
        nav.isStopped = true; // 공격 중일 때 NavMeshAgent 정지
    }

    void MonsterAttackEnd()
    {
        isAttack = false;
        anim.SetBool(RunHash, true); // 공격이 끝나면 이동 애니메이션 활성화
        nav.isStopped = false; // 공격이 끝나면 NavMeshAgent 다시 활성화
    }

    // 첫 번째 공격 애니메이션이 끝날 때 호출될 함수
    public void OnFirstAttackAnimationEnd()
    {
        isAttack = false;
        isSkill = false;
        anim.SetBool(RunHash, true); // 공격이 끝나면 이동 애니메이션 활성화
        nav.isStopped = false; // 공격이 끝나면 NavMeshAgent 다시 활성화
    }

    // 두 번째 공격 애니메이션이 끝날 때 호출될 함수
    public void OnSecondAttackAnimationEnd()
    {
        isAttack = false;
        isSkill = false;
        anim.SetBool(RunHash, true); // 공격이 끝나면 이동 애니메이션 활성화
        nav.isStopped = false; // 공격이 끝나면 NavMeshAgent 다시 활성화
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (isAttack)
        {
            if (isSkill)
            {
                if (other.gameObject.TryGetComponent(out Player player))
                {
                    player.TakeDamage(Damage(1)); // monster의 데미지 1번 = 스킬 공격
                }
            }
            else
            {
                if (other.gameObject.TryGetComponent(out Player player))
                {
                    player.TakeDamage(Damage(0)); // monster의 데미지 0번 = 기본 공격
                }
            }
        }
    }
}


