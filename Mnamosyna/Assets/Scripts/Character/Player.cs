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

    public float invincibleTime = 1.0f; // ���� ���� �ð�
    private float lastDamagedTime;

    //���ݹ����� �����̴� �뵵
    private float flashDuration = 0.1f;
    private int flashCount = 4;
    private List<Renderer> renderers;


    Vector3 moveVec;
    Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;
    Sword sword;

    // ��ų ���� �ڵ�
    public int maxSkillSlots = 30;
    public SkillData[] skillSlots;

    // ��ü ��ų
    private List<SkillData> allSkills = new List<SkillData>();
    // �÷��̾ ������ ��ų ID�� �����ϴ� ����Ʈ
    private List<int> unlockSkillIds = new List<int>();
    // �÷��̾ ������ ��ų �����͸� �����ϴ� ����Ʈ
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
        // �÷��̾� �Է��� ȭ�� �������� ��ȯ
        Vector3 inputDirection = new Vector3(hAxis, 0, vAxis);
        inputDirection = Camera.main.transform.TransformDirection(inputDirection);
        inputDirection.y = 0; // y�� ��ȯ�� ���� (�÷��̾�� �ٴ��� ���� ������)

        // �̵� ���� ����ȭ
        moveVec = inputDirection.normalized;

        if (!isAttackReady)
        {
            moveVec = Vector3.zero;
        }

        if (!isBorder)
        {
            // �÷��̾ �̵� �������� �̵�
            transform.position += moveVec * stat.move_speed * Time.deltaTime;
        }

        // �̵� ���ο� ���� �ִϸ��̼� ����
        anim.SetBool("isRun", moveVec != Vector3.zero);

        // �÷��̾ �̵��ϴ� ������ �ٶ󺸵��� ��
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


        //Ű���� ȸ��
        transform.LookAt(transform.position + moveVec);
        // ���콺 ȸ��
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
    // ���� �������� Ȯ���ϴ� �ڷ�ƾ => ���� ���ɼ� ����
    IEnumerator AttackEnd(float attackTime, string animationBool)
    {
        anim.SetTrigger(animationBool);
        yield return new WaitForSeconds(attackTime/2);
        isAttack = false;
    }
    // �÷��̾��� ��ų Ŀ�ǵ� �ʱ�ȭ �ڷ�ƾ
    IEnumerator ClearCommand()
    {
        yield return new WaitForSeconds(3);
        skillCammand = " ";
    }

    // �÷��̾��� ������ ����
    public int Damage()
    {
        int baseDamage = Random.Range(stat.min_atk, stat.max_atk+1);

        bool isCritical = Random.value < stat.crit_chance;

        //ũ��Ƽ�� Ȯ���� ũ��Ƽ�� Ȯ�� �� ������ ����
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


    /*/ �ǰ�
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("monsterAttack"))
        {
            if (!isDamage)
            {
                if (other.CompareTag("Monster")) // ���� GameObject�� "Monster" �±׸� ������ ���
                {
                    Monster monster = other.GetComponent<Monster>(); // ���� GameObject�� Monster ��ũ��Ʈ�� ������
                    if (monster != null)
                    {
                        int damage = monster.Attack.Damage(); // ������ Damage() �޼��� ȣ��
                                                      
                        int finalDamage = Mathf.RoundToInt(damage * (1 - stat.defense)); // ���� ���� ����
                        stat.cur_hp = Mathf.Max(0, stat.cur_hp - finalDamage);
                        if(other.GetComponent<Rigidbody>() != null)
                        {
                            Destroy(other.gameObject);
                        }

                        StartCoroutine(TakeDamage());
                        Debug.Log("�÷��̾ ���� ���� :" + finalDamage);
                    }
                }
            }
        }
    }
    */

    // �ǰ� �޼���
    public void GetHit()
    {
        anim.SetTrigger("GetHit");
        Flash();
        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null) { playerAttack.DisableSwordCollider(); }
        else { Debug.Log("isattack�������"); }
    }

    //������ ���ݿ� ���� �������� ���� ����� ���� ���� ������ ����
    public void TakeDamage(int damage)
    {
        if (Time.time >= lastDamagedTime + invincibleTime)
        {
            // ���� ������ = �÷��̾��� ���ݵ����� * (1 - ����%)
            int finalDamage = Mathf.RoundToInt(damage * (1 - Defense));
            Cur_HP -= finalDamage;
            lastDamagedTime = Time.time;

        }

        //if (stat.Cur_HP <= 0) { Die() }
    }

    // �ǰݽ� �������(���� ����)
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }
    // ���� �ڷ�ƾ
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

    // �ڵ� ȸ�� �ý���
    void Recover()
    {
        // ü�� �ڵ� ȸ��
        if (Cur_HP > 0 && Cur_HP < Max_HP)
        {
            Cur_HP += Mathf.RoundToInt(HP_Recover * Time.deltaTime);
            Cur_HP = Mathf.Clamp(Cur_HP, 0, Max_HP);

            /* float deltaTimeValue = Time.deltaTime;
            Debug.Log("DeltaTime Value: " + deltaTimeValue);
            float recoveredAmount = Mathf.RoundToInt(stat.hp_recover * Time.deltaTime);
            Debug.Log("Recovered Amount: " + recoveredAmount); */
        }

        // ���׹̳� �ڵ� ȸ��
        if (Cur_Stamina < Max_Stamina)
        {
            Cur_Stamina += Mathf.RoundToInt(Stamina_Recover * Time.deltaTime);
            Cur_Stamina = Mathf.Clamp(Cur_Stamina, 0, Max_Stamina);

        }
    }

    // �÷��̾� �뽬
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

    // ��ų ��� �޼���
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

    // �÷��̾� ��ȣ�ۿ� �޼���
    void Interaction()
    {
        // �÷��̾�� ��ȣ�ۿ��� �� �ִ� ��� Collider�� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);

        foreach (var collider in colliders)
        {
            Portal portal = collider.GetComponent<Portal>();
            if (portal != null)
            {
                portal.TeleportPlayer(transform);
                break; // ���� ��Ż�� ���� ��� ù ��° ��Ż�� ���
            }

            HealSpace healSpace = collider.GetComponent<HealSpace>();
            if (healSpace != null)
            {
                healSpace.HealPlayer(gameObject); // �÷��̾� ���� ������Ʈ�� ����
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
