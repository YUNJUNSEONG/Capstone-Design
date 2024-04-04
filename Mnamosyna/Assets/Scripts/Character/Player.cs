using NUnit.Framework.Interfaces;
//using skill;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class Player : PlayerStat
{
    public PlayerStat stat;


    float hAxis;
    float vAxis;

    bool leftDown;
    bool rightDown;
    bool shiftDown;
    bool isBorder;
    bool isDamage;
    public bool isAttack;
    public bool isDash;

    public Camera followCamera;
    public bool isAttackReady = true;

    public float invincibleTime = 1.0f; // 무적 지속 시간
    private float lastDamagedTime;

    //공격받을때 깜빡이는 용도
    private float flashDuration = 0.1f;
    private int flashCount = 4;
    private List<Renderer> renderers;


    Vector3 moveVec;
    Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;
    Sword sword;

    // 스킬 관련 코드
    public int maxSkillSlots = 30;
    public SkillData[] skillSlots;

    // 전체 스킬
    private List<SkillData> allSkills = new List<SkillData>();
    // 플레이어가 보유한 스킬 ID를 저장하는 리스트
    private List<int> unlockSkillIds = new List<int>();
    // 플레이어가 보유한 스킬 데이터를 저장하는 리스트
    private List<SkillData> unlockSkills = new List<SkillData>();
    public List<SkillData> UnlockSkills
    {
        get { return unlockSkills; }
        set { unlockSkills = value; }
    }

    Coroutine CommandCoroutine = null;

    [SerializeField]
    private string skillCammand;
    public string SkillCammand
    {
        get { return skillCammand; }
        set { skillCammand = value; }
    }
 

    float attackDelay;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        sword = GetComponentInChildren<Sword>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        stat = GetComponent<PlayerStat>();  
        instance = this;
    }
    void Start()
    {
        stat.cur_hp = PlayerStat.instance.max_hp;
        stat.cur_stamina = PlayerStat.instance.max_stamina;
    }


    // Update is called once per frame
    new void Update()
    {
        GetInput();
        Move();
        LeftAttack();
        RightAttack();
        Dash();
        Recover();
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        leftDown = Input.GetButtonDown("Fire1");
        rightDown = Input.GetButtonDown("Fire2");
        shiftDown = Input.GetButtonDown("Dash");
    }

    void Move()
    {
        // 플레이어 입력을 화면 공간으로 변환
        Vector3 inputDirection = new Vector3(hAxis, 0, vAxis);
        inputDirection = Camera.main.transform.TransformDirection(inputDirection);
        inputDirection.y = 0; // y축 변환은 무시 (플레이어는 바닥을 따라 움직임)

        // 이동 벡터 정규화
        moveVec = inputDirection.normalized;

        if (!isAttackReady)
        {
            moveVec = Vector3.zero;
        }

        if (!isBorder)
        {
            // 플레이어를 이동 방향으로 이동
            transform.position += moveVec * stat.move_speed * Time.deltaTime;
        }

        // 이동 여부에 따라 애니메이션 설정
        anim.SetBool("isRun", moveVec != Vector3.zero);

        // 플레이어가 이동하는 방향을 바라보도록 함
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
        }
        
        /*
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (!isAttackReady)
        {
            moveVec = Vector3.zero;
        }

        if (!isBorder)
        {
            transform.position += moveVec * stat.move_speed *  Time.deltaTime;
        }

        anim.SetBool("isRun", moveVec != Vector3.zero);


        //키보드 회전
        transform.LookAt(transform.position + moveVec);
        // 마우스 회전
        /* Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if(Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        } */
    }

    void FixedUpdate()
    {
        FreezeRotation();
        stopToWall();
    }
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void stopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.white);
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }

    void LeftAttack()
    {

        attackDelay += Time.deltaTime;
        isAttackReady = stat.atk_speed < attackDelay;

        if (leftDown && isAttackReady && !isAttack && !isDash)
        {
            if (CommandCoroutine != null)
                StopCoroutine(CommandCoroutine);
            skillCammand += 'L';
            CommandCoroutine = StartCoroutine(ClearCommand());
            UseSkill();

            if (!isAttack)
            {
                sword.Use(stat.left_atk_speed, Damage());
                anim.SetTrigger("LeftAttack");
                attackDelay = 0;
            }

        }
    }

   void RightAttack()
    {
        attackDelay += Time.deltaTime;
        isAttackReady = stat.atk_speed < attackDelay;

        if(rightDown && isAttackReady && !isAttack && !isDash)
        {
            if (CommandCoroutine != null)
                StopCoroutine(CommandCoroutine);
            skillCammand += 'R';
            CommandCoroutine = StartCoroutine(ClearCommand());
            UseSkill();
            if (!isAttack)
            {
                sword.Use(stat.right_atk_speed, Damage());
                anim.SetTrigger("RightAttack");
                attackDelay = 0;
            }
        }
    }
    // 공격 끝났는지 확인하는 코루틴 => 삭제 가능성 높음
    IEnumerator AttackEnd(float attackTime, string animationBool)
    {
        anim.SetTrigger(animationBool);
        yield return new WaitForSeconds(attackTime/2);
        isAttack = false;
    }
    // 플레이어의 스킬 커맨드 초기화 코루틴
    IEnumerator ClearCommand()
    {
        yield return new WaitForSeconds(3);
        skillCammand = " ";
    }

    // 플레이어의 데미지 설정
    public int Damage()
    {
        int baseDamage = Random.Range(stat.min_atk, stat.max_atk+1);

        bool isCritical = Random.value < stat.crit_chance;

        //크리티컬 확률로 크리티컬 확인 후 데미지 적용
        if (isCritical)
        {
            int criticalDamage = Mathf.RoundToInt(baseDamage * stat.critical);

            return criticalDamage;
        }
        else
        {
            return baseDamage;
        }
    }


    /*/ 피격
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("monsterAttack"))
        {
            if (!isDamage)
            {
                if (other.CompareTag("Monster")) // 몬스터 GameObject에 "Monster" 태그를 지정한 경우
                {
                    Monster monster = other.GetComponent<Monster>(); // 몬스터 GameObject의 Monster 스크립트를 가져옴
                    if (monster != null)
                    {
                        int damage = monster.Attack.Damage(); // 몬스터의 Damage() 메서드 호출
                                                      
                        int finalDamage = Mathf.RoundToInt(damage * (1 - stat.defense)); // 피해 감소 적용
                        stat.cur_hp = Mathf.Max(0, stat.cur_hp - finalDamage);
                        if(other.GetComponent<Rigidbody>() != null)
                        {
                            Destroy(other.gameObject);
                        }

                        StartCoroutine(TakeDamage());
                        Debug.Log("플레이어가 받은 피해 :" + finalDamage);
                    }
                }
            }
        }
    }
    */

    // 피격 메서드
    public void GetHit()
    {
        anim.SetTrigger("GetHit");
        Flash();
        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null) { playerAttack.DisableSwordCollider(); }
        else { Debug.Log("isattack변경실패"); }
    }

    //몬스터의 공격에 의한 데미지를 방어력 계산을 통해 최종 데미지 산출
    public void TakeDamage(int damage)
    {
        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            // 최종 데미지 = 플레이어의 공격데미지 * (1 - 방어력%)
            int finalDamage = Mathf.RoundToInt(damage * (1 - Defense));
            Cur_HP -= finalDamage;
            lastDamagedTime = Time.time;

        }

        //if (stat.Cur_HP <= 0) { Die() }
    }

    // 피격시 점멸상태(무적 상태)
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }
    // 점멸 코루틴
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

    // 자동 회복 시스템
    void Recover()
    {
        // 체력 자동 회복
        if (Cur_HP > 0 && Cur_HP < Max_HP)
        {
            Cur_HP += Mathf.RoundToInt(HP_Recover * Time.deltaTime);
            Cur_HP = Mathf.Clamp(Cur_HP, 0, Max_HP);

            /* float deltaTimeValue = Time.deltaTime;
            Debug.Log("DeltaTime Value: " + deltaTimeValue);
            float recoveredAmount = Mathf.RoundToInt(stat.hp_recover * Time.deltaTime);
            Debug.Log("Recovered Amount: " + recoveredAmount); */
        }

        // 스테미나 자동 회복
        if (Cur_Stamina < Max_Stamina)
        {
            Cur_Stamina += Mathf.RoundToInt(Stamina_Recover * Time.deltaTime);
            Cur_Stamina = Mathf.Clamp(Cur_Stamina, 0, Max_Stamina);

        }
    }

    // 플레이어 대쉬
    void Dash()
    {
        //if (unlockSkills.Count > 0)
        //{
            attackDelay += Time.deltaTime;
            isAttackReady = stat.atk_speed < attackDelay;

            if (shiftDown && isAttackReady && !isAttack && !isDash)
            {

                if (CommandCoroutine != null)
                    StopCoroutine(CommandCoroutine);
                skillCammand += 'S';
                CommandCoroutine = StartCoroutine(ClearCommand());
                UseSkill();
                if (!isAttack)
                {
                    sword.Use(stat.Dash_speed, Damage());
                    anim.SetTrigger("Dash");
                    attackDelay = 0;
                }
            }
        //}
    }

    // 스킬 사용 메서드
    private void UseSkill()
    {
        for (int i = unlockSkills.Count - 1; i >= 0; i--)
        {
            if (skillCammand.EndsWith(unlockSkills[i].Command))
            {
                if (unlockSkills[i].Level > 0)
                {
                    print(unlockSkills[i].AnimationTrigger);
                    if (CommandCoroutine != null)
                        StopCoroutine(CommandCoroutine);
                    isAttack = true;
                    float floatSkillDamage = allSkills[i].damagePercent * Damage();
                    int intSkillDamage = Mathf.RoundToInt(floatSkillDamage);
                    float floatLevelDamage = allSkills[i].Level * allSkills[i].addDmg;
                    int intLevelDamage = Mathf.RoundToInt(floatLevelDamage);
                    int skilldamage = intSkillDamage + intLevelDamage;
                    sword.Use(allSkills[i].AnimationTime, skilldamage);
                    StartCoroutine(AttackEnd(allSkills[i].AnimationTime, allSkills[i].AnimationTrigger));
                    skillCammand = " ";
                    break;
                }
            }
        }
    }

    // 플레이어 상호작용 메서드
    void Interaction()
    {
        // 플레이어와 상호작용할 수 있는 모든 Collider를 가져옴
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);

        foreach (var collider in colliders)
        {
            Portal portal = collider.GetComponent<Portal>();
            if (portal != null)
            {
                portal.TeleportPlayer(transform);
                break; // 여러 포탈이 있을 경우 첫 번째 포탈만 사용
            }

            HealSpace healSpace = collider.GetComponent<HealSpace>();
            if (healSpace != null)
            {
                healSpace.HealPlayer(gameObject); // 플레이어 게임 오브젝트를 전달
                break;
            }
            /*
            LevelUpSkill levelUpSkill = collider.GetComponent<LevelUpSkill>();
            if (levelUpSkill != null)
            {
                levelUpSkill.levelUpPlayer(gameObject);
                break;
            }

            UnlockSkill unlockSkill = collider.GetComponent<UnlockSkill>();
            if(unlockSkill != null)
            {
                unlockSkill.unlockPlayer(gameObject);
                break;
            }
            */
        }
    }

}
