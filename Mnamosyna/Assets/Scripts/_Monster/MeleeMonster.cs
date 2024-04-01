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
        Attack,
        Die
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
    private bool isDead = false; 
    private float Skill1CanUse=0;
    private float Skill2CanUse=0;
    private float Skill3CanUse=0;
    
    //애니메이션용
    private static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    private static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    private static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    private static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int GetHitHash = Animator.StringToHash("GetHit");
    private static readonly int DieHash = Animator.StringToHash("Die");
    
    //공격받을때 깜빡이는 용도
    private float flashDuration = 0.1f;
    private int flashCount = 2;
    private List<Renderer> renderers;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = distance;
        random = new System.Random();
        exclamationMark.SetActive(false);
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
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
        animator.SetBool(RunHash, true);
        
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
    void Chase()
    {
        if (currentState == MonsterState.Die)
        {
            return;
        }
        animator.SetBool(RunHash, true);
        agent.SetDestination(player.transform.position);
    }
    IEnumerator ShowExclamationMarkForSeconds(float seconds)
    {
        exclamationMark.SetActive(true); 
        yield return new WaitForSeconds(seconds);  
        exclamationMark.SetActive(false);
    }
    void Attack()
    {
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
        animator.SetTrigger(Attack01Hash);
    }
    void Skill2()
    {
        animator.SetTrigger(Attack02Hash);
    }
    void Skill3()
    {
        animator.SetTrigger(Attack03Hash);
    }
    public void TakeDamage(int damage)
    {
        if (currentState != MonsterState.Patrol) {ChangeState(MonsterState.Chase);}
        
        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            CurrentHP -= damage;
            lastDamagedTime = Time.time;
            animator.SetTrigger(GetHitHash);
            Flash();
        }
        
        if (CurrentHP <= 0)   
        {
            Die();
        }
    }
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
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }
    void Die()
    {
        if (isDead) return;

        isDead = true;
        ChangeState(MonsterState.Die);
        animator.SetTrigger(DieHash);
        agent.isStopped = true;  
        animator.SetBool(RunHash, false);
        Invoke("DestroyObject", 3.0f);
    
        Collider collider = GetComponent<Collider>();
        if (collider != null) {collider.enabled = false;}
        
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
    
    void Update()
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
        
        Skill1CanUse -= Time.deltaTime;
        Skill2CanUse -= Time.deltaTime;
        Skill3CanUse -= Time.deltaTime;
    }
    
    void MonsterAttackStart()
    {
        isAttacking = true;
    }
    void MonsterAttackEnd()
    {
        isAttacking = false;
    }
    
}
