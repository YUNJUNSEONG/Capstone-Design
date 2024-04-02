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

    public float invincibleTime = 1f; // 공격받은후무적 시간
    private float lastDamagedTime;
    public MonsterSpawner spawner;
    public GameObject exclamationMark;


    protected Transform player;
    protected float switchTime = 2.0f;

    protected Rigidbody rigid; //리지드 바디
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

    //애니메이션용
    protected static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    protected static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    protected static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int GetHitHash = Animator.StringToHash("GetHit");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    //공격받을때 깜빡이는 용도
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
    // 몬스터의 상태 변경
    public void ChangeState(MonsterState state)
    {
        if (currentState == MonsterState.Patrol && state == MonsterState.Chase)
        {
            StartCoroutine(ShowExclamationMarkForSeconds(3.0f));
        }
        currentState = state;
    }
    // 몬스터가 주변을 배회 => 추후 삭제 할 수도 있음
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
    // 몬스터를 플레이어 쪽으로 회전
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

    //몬스터의 플레이어 추적 상태 출력 함수
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
    // NavMeshAgent의 속도를 몬스터의 이동 속도로 설정
    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            nav.speed = speed;
        }
    }

    // Attack 상태일 때 스킬 쿨타임에 따른 공격 실행 함수
    public void Attack()
    {
        RotateMonsterToCharacter();
        // 쿨타임이 0이하 인 공격중 랜덤하게 출력
        int skillIndex = random.Next(0, NumberOfSkills);

        switch (skillIndex)
        {
            case 0:  //기본 공격
                if (Skill1CanUse<=0 && attack1Radius <= 0)
                {
                    Skill1();
                    Damage(skillIndex);
                    Skill1CanUse = SkillCoolTime1;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 1: // 스킬 공격1
                if (Skill2CanUse<=0 && attack2Radius <= 0)
                {
                    Skill2();
                    Damage(skillIndex);
                    Skill2CanUse = SkillCoolTime2;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 2: //스킬 공격2
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
    // 공격 애니메이션
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
    // 몬스터의 공격에 따른 데미지 배정
    public int Damage(int skillIndex)
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
            case 2: // 스킬 공격2
                damage = Skill_ATK2;
                break;
            default:
                // 지정되지 않은 스킬이면 damage 0
                break;
        }
        return damage;
    }

    // 일정 시간동안 느낌표를 띄우는 코루틴
    IEnumerator ShowExclamationMarkForSeconds(float seconds)
    {
        exclamationMark.SetActive(true);
        yield return new WaitForSeconds(seconds);
        exclamationMark.SetActive(false);
    }

    // 몬스터의 State 업데이트
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
        // 1초마다 스킬 쿨 감소
        Skill1CanUse -= Time.deltaTime;
        Skill2CanUse -= Time.deltaTime;
        Skill3CanUse -= Time.deltaTime;

    }
    // 몬스터의 피격 상황 처리 함수
    public void TakeDamage(int damage)
    {
        if (currentState != MonsterState.Patrol) { ChangeState(MonsterState.Chase); }

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
    // 피격시 몬스터 점멸효과 출력 함수
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }
    //점멸효과 출력 코루틴
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
  
    //사망 처리 함수
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
            Debug.Log("남은 몬스터:"+spawner.aliveCount);
        }
        else
        {
            //Debug.LogError("몬스터 스크립트에서 몬스터 스포너 못 찾아옴");
        }
    }
    //사망시 넉백 처리
    protected void Knockback()
    {
        rigid.AddForce((transform.forward.normalized + Vector3.up) * 5f, ForceMode.Impulse);
    }
    // 몬스터 삭제
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
