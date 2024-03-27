using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : MonoBehaviour
{
    
    public enum MonsterState
    {
        Patrol,
        Chase,
        Attack
    }
    private MonsterState currentState;
    [SerializeField] public float MaxHP;
    [SerializeField] public float CurrentHP;
    [SerializeField] private float ATK;
    [SerializeField] private float SkillATK;
    [SerializeField] private float DEF;
    [SerializeField] private float ATKSpeed;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private int NumberOfSkills;
    [SerializeField] private float SkillCoolTime1;
    [SerializeField] private float SkillCoolTime2;
    [SerializeField] private float SkillCoolTime3;
    [SerializeField] private float distance;
    [SerializeField] private float patrolRadius = 20f;
    
    
    public float invincibleTime = 2f; // 공격받은후무적 시간
    private float lastDamagedTime;
    public MonsterSpawner spawner;
    public GameObject exclamationMark;
    
    private float switchTime = 2.0f;
    private GameObject player;
    private float distanceToPlayer;
    private Animator animator;
    private NavMeshAgent agent;
    private System.Random random;

    public bool isAttacking;
    private float Skill1CanUse=0;
    private float Skill2CanUse=0;
    private float Skill3CanUse=0;
    public static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    public static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    public static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    public static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = distance;
        random = new System.Random();
        exclamationMark.SetActive(false);
    }
    void RotateMonsterToCharacter()
    {
        Vector3 directionToCharacter = player.transform.position - transform.position;
        directionToCharacter.y = 0;  
        Quaternion rotationToCharacter = Quaternion.LookRotation(directionToCharacter);
        transform.rotation = rotationToCharacter;
    }
    public void ChangeState(MonsterState state)
    {
        if (currentState == MonsterState.Patrol && state == MonsterState.Chase)  
        {
            StartCoroutine(ShowExclamationMarkForSeconds(3.0f));
        }
        currentState = state;
        
    }

    void Patrol()
    {
        animator.SetBool("Run", true);
        
        if (switchTime <= 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
            {
                Vector3 finalPosition = hit.position;
                agent.SetDestination(finalPosition);
            }
            switchTime = Random.Range(2.0f, 5.0f);
        }
        else 
        {switchTime -= Time.deltaTime;}
    }
    
    void Chase()
    {
        //Debug.Log("버섯: 쫓기");
        agent.SetDestination(player.transform.position);
        animator.SetBool("Run", true);
    }
    
    void Attack()
    {
        //Debug.Log("버섯: 공격");
        
        RotateMonsterToCharacter();
        
        int skillIndex = random.Next(0, NumberOfSkills);
        
        switch (skillIndex)
        {
            case 0:
                if (Skill1CanUse<=0)
                {
                    Skill1();
                    Skill1CanUse = SkillCoolTime1;
                }
                else{animator.SetTrigger(BattleIdleHash);}
                break;
            case 1:
                if (Skill2CanUse<=0)
                {
                    Skill2();
                    Skill2CanUse = SkillCoolTime2;
                }
                else{animator.SetTrigger(BattleIdleHash);}
                break;
            case 2:
                if (Skill3CanUse<=0)
                {
                    Skill3();
                    Skill3CanUse = SkillCoolTime3;
                }
                else{animator.SetTrigger(BattleIdleHash);}
                break;
        }
    }

    void Skill1()
    {
        //Debug.Log("버섯:공격1");
        animator.SetTrigger(Attack01Hash);
    }
    void Skill2()
    {
        //Debug.Log("버섯:공격2");
        animator.SetTrigger(Attack02Hash);
    }
    void Skill3()
    {
        //Debug.Log("버섯:공격3");
        animator.SetTrigger(Attack03Hash);
    }
    
    void Update()
    {
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
        
        Skill1CanUse -= Time.deltaTime;
        Skill2CanUse -= Time.deltaTime;
        Skill3CanUse -= Time.deltaTime;
    }
    
    public void TakeDamage(int damage)
    {
        if (currentState != MonsterState.Patrol) {ChangeState(MonsterState.Chase);}
        
        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            CurrentHP -= damage;
            lastDamagedTime = Time.time;
        }
        
        
        
        if (CurrentHP <= 0)   
        {
            Die();  
        }
    }

    void Die()
    {
        Destroy(gameObject);
        
        if (spawner != null)
        {
            spawner.aliveCount--;
            spawner.CheckAliveCount();
            Debug.Log("남은 몬스터:"+spawner.aliveCount);
        }
        else
        {
            Debug.LogError("몬스터 스크립트에서 몬스터 스포너 못 찾아옴");
        }
    }

    void MonsterAttackStart()
    {
        isAttacking = true;
    }
    void MonsterAttackEnd()
    {
        isAttacking = false;
    }
    
    IEnumerator ShowExclamationMarkForSeconds(float seconds)
    {
        exclamationMark.SetActive(true); 
        yield return new WaitForSeconds(seconds);  
        exclamationMark.SetActive(false);
    }
    
}
