using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerStat stat;


    float hAxis;
    float vAxis;
    bool leftDown;
    bool rightDown;
    bool shiftDown;
    bool isBorder;
    bool isDamage;
    public static bool isAttack;
    public static bool isDash;

    public Camera followCamera;
    public bool isAttackReady = true;

    public float invincibilityTime = 1.5f; // ���� ���� �ð�
    private bool isInvincible = false; // ���� ���� ����
    private float invincibilityTimer = 0f; // ���� ���� �ð��� ī��Ʈ�ϱ� ���� Ÿ�̸�


    Vector3 moveVec;
    Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;
    Sword sword;

    // ��ų ���� �ڵ�
    [SerializeField]
    private List<SkillData> skills = new List<SkillData>();
    [SerializeField]
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
    //

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
    void Update()
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
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }

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

    IEnumerator AttackEnd(float attackTime, string animationBool)
    {
        anim.SetTrigger(animationBool);
        yield return new WaitForSeconds(attackTime/2);
        isAttack = false;
    }
    IEnumerator ClearCommand()
    {
        yield return new WaitForSeconds(3);
        skillCammand = " ";
    }

    // ������ ����
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


    // �ǰ�
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "monsterAttack")
        {
            if (!isDamage)
            {
                if(Monster.instance != null)
                {
                    int damage = Monster.instance.Damage();

                    // ���� ó��
                    int finalDamage = Mathf.RoundToInt(damage * (1 - stat.defense)); // ���� ���� ����
                    stat.cur_hp = Mathf.Max(0, stat.cur_hp - finalDamage);

                    StartCoroutine(TakeDamage());
                    Debug.Log("�÷��̾ ���� ���� :" + finalDamage);

                }
            }
        }
    }

    IEnumerator TakeDamage()
    {
        if(!isInvincible)
        {
            isDamage = true;
            foreach (SkinnedMeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.red;
            }

            yield return new WaitForSeconds(1f);
            isDamage = false;

            foreach (SkinnedMeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.white;
            }

            // ���� ���·� ����
            isInvincible = true;
            invincibilityTimer = invincibilityTime;
        }

    }

    // �ڵ� ȸ�� �ý���
    void Recover()
    {
        // ü�� �ڵ� ȸ��
        if (stat.cur_hp > 0 && stat.cur_hp < stat.max_hp)
        {
            stat.cur_hp += Mathf.RoundToInt(stat.hp_recover * Time.deltaTime);
            stat.cur_hp = Mathf.Clamp(stat.cur_hp, 0, stat.max_hp);

            /* float deltaTimeValue = Time.deltaTime;
            Debug.Log("DeltaTime Value: " + deltaTimeValue);
            float recoveredAmount = Mathf.RoundToInt(stat.hp_recover * Time.deltaTime);
            Debug.Log("Recovered Amount: " + recoveredAmount); */
        }

        // ���׹̳� �ڵ� ȸ��
        if (stat.cur_stamina < stat.max_stamina)
        {
            stat.cur_stamina += Mathf.RoundToInt(stat.stamina_recover * Time.deltaTime);
            stat.cur_stamina = Mathf.Clamp(stat.cur_stamina, 0, stat.max_stamina);

        }
    }

    void Dash()
    {
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
    }

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
                    float floatSkillDamage = skills[i].damagePercent * Damage();
                    int intSkillDamage = Mathf.RoundToInt(floatSkillDamage);
                    float floatLevelDamage = skills[i].Level * skills[i].addDmg;
                    int intLevelDamage = Mathf.RoundToInt(floatLevelDamage);
                    int skilldamage = intSkillDamage + intLevelDamage;
                    sword.Use(skills[i].AnimationTime, skilldamage);
                    StartCoroutine(AttackEnd(skills[i].AnimationTime, skills[i].AnimationTrigger));
                    skillCammand = " ";
                    break;
                }
            }
        }
    }

    void Interaction()
    {
        // �÷��̾�� ��ȣ�ۿ��� �� �ִ� ��� Collider�� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);

        // ������ Collider�� �� Potal ��ũ��Ʈ�� �ִ��� Ȯ���ϰ� ������ �ڷ���Ʈ
        foreach (var collider in colliders)
        {
            Portal portal = collider.GetComponent<Portal>();
            if (portal != null)
            {
                portal.TeleportPlayer(transform);
                break; // ���� ��Ż�� ���� ��� ù ��° ��Ż�� ���
            }
        }

        foreach (var collider in colliders)
        {
            HealSpace healSpace = collider.GetComponent<HealSpace>();
            if (healSpace != null)
            {
                healSpace.HealPlayer();
                break;
            }
        }
    }

}
