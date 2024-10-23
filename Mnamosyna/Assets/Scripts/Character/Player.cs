using NUnit.Framework.Interfaces;
//using skill;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static SkillData;


public class Player : PlayerStat
{
    public GameObject gameOverPanel;

    float hAxis;
    float vAxis;

    bool leftDown;
    bool rightDown;
    bool spaceDown;
    bool isBorder;
    bool isDamage;
    public bool isDead = false;
    public bool isAttack = false;
    public bool isDash = false;
    private float attackDelay = 0.0f;
    private float attackThreshold = 1.5f; // someThreshold�� attackThreshold�� ����

    private Coroutine CommandCoroutine = null;
    private Queue<string> inputBuffer = new Queue<string>();
    int hashAttackCount = Animator.StringToHash("AttackCount");
    public bool isGameOver = false;


    public Camera followCamera;

    public float invincibleTime = 1.0f; // ���� ���� �ð�
    private bool isInvincible = false;

    // ���� ������ ���� �ð� (��)
    public float invincibleDuration = 2.0f;
    private float lastDamagedTime;

    //���ݹ����� �����̴� �뵵
    private float flashDuration = 0.1f;
    private int flashCount = 10;
    private SkillManager skillManager;
    private List<Renderer> renderers;


    Vector3 moveVec;
    public Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;

    // �� �Ӽ��� �ش��ϴ� BaseMesh
    GameObject initialBaseMesh = null;
    public GameObject BaseMesh_Fire;
    public GameObject BaseMesh_Water;
    public GameObject BaseMesh_Air;
    public GameObject BaseMesh_Earth;
    // ���� ������ BaseMesh
    private GameObject currentBaseMesh;


    public Collider waterACollider;
    public TrailRenderer waterAEffect;
    public Collider fireACollider;
    public TrailRenderer fireAEffect;
    public Collider airACollider;
    public TrailRenderer airAEffect1, airAEffect2;
    public Collider earthACollider;
    public TrailRenderer earthAEffect1, earthAEffect2;


    // ��ų ���� �ڵ�
    //public int maxSkillSlots = 30;
    //public SkillData[] skillSlots;

    // ��ü ��ų
    [SerializeField]
    private List<SkillData> allSkills = new List<SkillData>();

    [SerializeField]
    private List<SkillData> stanbySkills = new List<SkillData>();
    public List<SkillData> StanbySkills
    {
        get { return stanbySkills; }
        set { stanbySkills = value; }
    }
    public List<SkillData> unlockedSkills
    {
        get { return UnlockSkills; }
        set { UnlockSkills = value; }
    }

    [SerializeField]
    private string skillCommand;
    public string SkillCommand
    {
        get { return skillCommand; }
        set { skillCommand = value; }
    }


    void Awake()
    {
        DontDestroyOnLoad(this);
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    void Start()
    {
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        Cur_HP = Max_HP;
        Cur_Stamina = Max_Stamina;
        foreach (SkillData skillData in allSkills)
        {
            skillData.Level = 0;
            if (skillData.isUnlock)
            {
                stanbySkills.Add(skillData);
            }
        }

        skillManager = FindObjectOfType<SkillManager>(); // scene���� SkillManager ������Ʈ�� ã�� �Ҵ�
        if (skillManager == null)
        {
            Debug.LogError("SkillManager�� ã�� �� �����ϴ�.");
        }

        currentBaseMesh = BaseMesh_Water;
        gameOverPanel.SetActive(false);
        StartCoroutine(RegenerateStats());

        DisableAllColliders();  // ���� �� ��� �浹ü�� ��Ȱ��ȭ
        if (waterACollider == null)
        {
            waterACollider = transform.Find("Sword1").GetComponent<Collider>();
        }
        if (fireACollider == null)
        {
            fireACollider = transform.Find("Sword2").GetComponent<Collider>();
        }
        if (airACollider == null)
        {
            airACollider = transform.Find("Sword3").GetComponent<Collider>();
        }
        if (earthACollider == null)
        {
            earthACollider = transform.Find("Sword4").GetComponent<Collider>();
        }

        StartCoroutine(ClearCommand());
    }



    // Update is called once per frame
    void Update()
    {
        GetInput();
        ChangeWeapon();
        ApplySkills();
        if (!isAttack)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }

        if (Cur_HP <= 0)
        {
            Die();
        }


        HandleInput();
        if (!isAttack && inputBuffer.Count > 0)
        {
            ProcessNextInput();
        }

        if (isAttack)
        {
            attackDelay += Time.deltaTime;

            if (attackDelay >= attackThreshold)
            {
                isAttack = false;
                attackDelay = 0.0f;
            }
        }

        if (isGameOver)
        {
            Destroy(this.gameObject); // ���� ���� �� ������Ʈ �ı�
        }

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Fire1"))
        {
            inputBuffer.Enqueue("L");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            inputBuffer.Enqueue("R");
        }
        if (Input.GetButtonDown("Jump"))
        {
            inputBuffer.Enqueue("S");
        }
    }

    #region �÷��̾� �̵�
    void Move()
    {
        // �÷��̾� �Է��� ȭ�� �������� ��ȯ
        Vector3 inputDirection = new Vector3(hAxis, 0, vAxis);
        inputDirection = Camera.main.transform.TransformDirection(inputDirection);
        inputDirection.y = 0; // y�� ��ȯ�� ���� (�÷��̾�� �ٴ��� ���� ������)

        // �̵� ���� ����ȭ
        moveVec = inputDirection.normalized;

        if (!isBorder)
        {
            // �÷��̾ �̵� �������� �̵�
            transform.position += moveVec * Move_Speed * Time.deltaTime;
        }

        // �̵� ���ο� ���� �ִϸ��̼� ����
        anim.SetBool("isRun", moveVec != Vector3.zero);

        // �̵� ���⿡ ���� ȸ��
        if (moveVec != Vector3.zero)
        {
            // ���콺 ��ġ�� ����Ͽ� ȸ��
            Vector3 targetDirection = moveVec;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 mouseDirection = hitInfo.point - transform.position;
                mouseDirection.y = 0; // y���� ����
                targetDirection = Vector3.Lerp(moveVec, mouseDirection.normalized, 0.5f); // �̵� ����� ���콺 ������ ȥ��
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 10f);
        }
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
    #endregion
    public int AttackCount
    {
        get => anim.GetInteger(hashAttackCount);
        set => anim.SetInteger(hashAttackCount, value);
    }

    void LeftAttack()
    {
        skillCommand += 'L';
        bool skillUsed = UseSkill(); // ��ų�� ���Ǿ����� Ȯ��

        // ��ų�� ���� ���
        if (skillUsed)
        {
            isAttack = true; // ��ų ��� �� ���� ���� ����
            StartCoroutine(ActivateColliderForDuration(2f)); // �ݶ��̴� Ȱ��ȭ �� 2�� �� ��Ȱ��ȭ
        }

        else
        {
            isAttack = true;

            // �������� swordNumber ���� (��: �÷��̾��� ���� �Ӽ��� ����)
            int swordNumber = GetCurrentSwordNumber(); // �� �޼��带 �����ؼ� ������ �� ��ȣ ��ȯ

            SetActiveSword(swordNumber); // �ùٸ� �� �ݶ��̴� Ȱ��ȭ
            FaceMouseDirection();
            anim.SetTrigger("LeftAttack");
            Debug.Log("LeftAttack Triggered");

            attackDelay = 0;
            StartCoroutine(AttackEnd("LeftAttack"));
            if (skillCommand == "LLL")
            {
                skillCommand = "";        // Ŀ�ǵ� �ʱ�ȭ
                inputBuffer.Clear();      // �Է� ���� �ʱ�ȭ
            }
        }
    }


    void RightAttack()
    {
        skillCommand += 'R';
        bool skillUsed = UseSkill(); // ��ų�� ���Ǿ����� Ȯ��

        // ��ų�� ���� ���
        if (skillUsed)
        {
            isAttack = true; // ��ų ��� �� ���� ���� ����
            StartCoroutine(ActivateColliderForDuration(2f)); // �ݶ��̴� Ȱ��ȭ �� 2�� �� ��Ȱ��ȭ
        }
        else // ��ų�� ������ ���� ���
        {
            // ���� ���¸� true�� ����
            isAttack = true;

            // �������� swordNumber ���� (��: �÷��̾��� ���� �Ӽ��� ����)
            int swordNumber = GetCurrentSwordNumber(); // ���� �Ӽ��� ���� ������ �� ��ȣ ��ȯ

            SetActiveSword(swordNumber); // �ùٸ� �� �ݶ��̴� Ȱ��ȭ
            FaceMouseDirection(); // ���콺 �������� ĳ���� ȸ��

            anim.SetTrigger("RightAttack"); // ������ ���� �ִϸ��̼� Ʈ����
            Debug.Log("RightAttack Triggered"); // ����� �α׷� Ʈ���� Ȯ��

            attackDelay = 0; // ���� ���� �ʱ�ȭ
            StartCoroutine(AttackEnd("RightAttack")); // ���� ���� ���
        }
    }


    void FaceMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // Y�� ȸ�� ����
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10); // �ε巯�� ȸ��
        }
    }

    // �÷��̾� �뽬
    void Dash()
    {
        // ��ų�� �ߵ��Ǿ����� Ȯ��
        bool skillUsed = UseSkill(); // ��ų�� ���Ǿ����� Ȯ��

        // ��ų�� ���� ���
        if (skillUsed)
        {
            isAttack = true; // ��ų ��� �� ���� ���� ����
            StartCoroutine(ActivateColliderForDuration(2f)); // �ݶ��̴� Ȱ��ȭ �� 2�� �� ��Ȱ��ȭ
        }

        if (!isDash)
        {
            skillCommand += 'S'; // Ŀ�ǵ忡 'S' �߰�
            FaceMouseDirection(); // �뽬 ���� ����

            isDash = true;
            anim.SetTrigger("Dash");
            StartCoroutine(DashEnd("Dash"));
        }
    }

    IEnumerator DashEnd(string animationTrigger)
    {
        AnimatorStateInfo stateInfo;
        do
        {
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            yield return null;
        } while (!stateInfo.IsName(animationTrigger));

        // �ִϸ��̼� �ӵ��� ����� ���� ��� �ð� ���
        float animationLength = stateInfo.length / anim.speed;
        yield return new WaitForSeconds(animationLength / 2);

        isDash = false;

        if (inputBuffer.Count > 0)
        {
            ProcessNextInput();
        }
    }

    bool UseSkill()
    {
        bool skillActivated = Skill();
        if (skillActivated)
        {
            isAttack = true; // ��ų ��� �� ���� ���� ����
        }
        return skillActivated;
    }

    bool Skill()
    {
        for (int i = unlockedSkills.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(unlockedSkills[i].Command) && skillCommand.EndsWith(unlockedSkills[i].Command))
            {
                if (unlockedSkills[i].Level > 0 && Cur_Stamina >= unlockedSkills[i].useStamina)
                {
                    Debug.Log(unlockedSkills[i].AnimationTrigger);

                    float floatSkillDamage = unlockedSkills[i].damagePercent * Damage();
                    int intSkillDamage = Mathf.RoundToInt(floatSkillDamage);
                    float floatLevelDamage = unlockedSkills[i].Level * unlockedSkills[i].addDmg;
                    int intLevelDamage = Mathf.RoundToInt(floatLevelDamage);
                    int skillDamage = intSkillDamage + intLevelDamage;

                    anim.SetTrigger(unlockedSkills[i].AnimationTrigger);

                    Cur_Stamina -= unlockedSkills[i].useStamina; // ���¹̳� �Ҹ�
                    skillCommand = "";  // Ŀ�ǵ� ����
                    inputBuffer.Clear(); // �Է� ���� ����

                    return true; // ��ų�� ���������� ����
                }
                else
                {
                    Debug.Log("���¹̳ʰ� �����մϴ�.");
                    return false; // ���¹̳� ����
                }
            }
        }
        return false; // ��ġ�ϴ� ��ų ����
    }

    IEnumerator ActivateColliderForDuration(float duration)
    {
        // �ݶ��̴� Ȱ��ȭ
        int swordNumber = GetCurrentSwordNumber(); // ���� �Ӽ��� ���� ������ �� ��ȣ ��ȯ
        SetActiveSword(swordNumber); // �ùٸ� �� �ݶ��̴� Ȱ��ȭ

        // ������ �ð� ���
        yield return new WaitForSeconds(duration);

        // �ݶ��̴� ��Ȱ��ȭ
        SetActiveSword(-1); // -1 �Ǵ� ������ ���� ����Ͽ� ��� �ݶ��̴� ��Ȱ��ȭ
    }


    void ActivateSkillCollider()
    {
        // �������� swordNumber ���� (��: �÷��̾��� ���� �Ӽ��� ����)
        int swordNumber = GetCurrentSwordNumber(); // ���� �Ӽ��� ���� ������ �� ��ȣ ��ȯ
        SetActiveSword(swordNumber); // �ùٸ� �� �ݶ��̴� Ȱ��ȭ
    }

    /*

    // ��ų�� �ߵ��Ǿ����� Ȯ���ϴ� �޼���
    bool CheckIfSkillUsed()
    {
        // ��ų�� �ߵ��Ǿ��ٸ� UseSkill()���� true�� ��ȯ�ϰ� ��
        return UseSkill();
    }*/


    IEnumerator AttackEnd(string animationTrigger)
    {
        // �ִϸ������� ���� ���� ���� ��������
        AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // �ִϸ��̼� ���̿� �ӵ��� �ݿ��Ͽ� ���� ��� �ð��� ���
        float animationLength = animStateInfo.length / animStateInfo.speed;

        // �ִϸ��̼��� ���� ������ ���
        yield return new WaitForSeconds(animationLength);

        // ���� ���õ� ������ �浹ü ��Ȱ��ȭ
        DisableSwordCollider();

        // �Է� ���ۿ� ���� ���� ���, ���� �Է� ó��
        if (inputBuffer.Count > 0)
        {
            ProcessNextInput();
        }

        Debug.Log("Attack ended and isAttack set to false");
    }


    //�Է� ó�� �� ���� ���� �߰�
    void HandleInput()
    {
        attackDelay += Time.deltaTime;
        //isAttackReady = ATK_Speed <= attackDelay;

        if (leftDown)
        {
            AddInputToBuffer("L");
        }
        if (rightDown)
        {
            AddInputToBuffer("R");
        }
        if (spaceDown)
        {
            AddInputToBuffer("S");
        }
    }

    // �÷��̾��� ��ų Ŀ�ǵ� �ʱ�ȭ �ڷ�ƾ
    IEnumerator ClearCommand()
    {
        while (true)
        {
            yield return new WaitForSeconds(7);
            skillCommand = " ";
            //CommandCoroutine = null;
        }
    }

    void AddInputToBuffer(string input)
    {
        inputBuffer.Enqueue(input); // ť�� �߰��ϴ� ���, �Է� ��� ó��
        skillCommand += input;

        // ��ų�� ��� Ȯ���ϰ� �ߵ�
        UseSkill();

        if (CommandCoroutine != null)
        {
            StopCoroutine(CommandCoroutine);
        }
        CommandCoroutine = StartCoroutine(ClearCommand());

    }

    void ProcessNextInput()
    {
        if (inputBuffer.Count > 0)
        {
            string nextInput = inputBuffer.Peek(); // ť�� �� �� ��Ҹ� Ȯ��
            Debug.Log("Next input: " + nextInput);  // Log next input

            switch (nextInput)
            {
                case "L":
                    HandleLeftClick();
                    //inputBuffer.Dequeue();
                    break;
                case "R":
                    HandleRightClick();
                    //inputBuffer.Dequeue();
                    break;
                case "S":
                    Dash();
                    inputBuffer.Dequeue();
                    break;
            }
        }
    }

    void HandleLeftClick()
    {
        //int leftClickCount = 0;
        while (inputBuffer.Count > 0 && inputBuffer.Peek() == "L")//&& leftClickCount < 3)
        {
            inputBuffer.Dequeue();
            //leftClickCount++;
        }
        LeftAttack();
    }

    void HandleRightClick()
    {
        while (inputBuffer.Count > 0 && inputBuffer.Peek() == "R")
        {
            inputBuffer.Dequeue();
        }
        RightAttack();
    }


    // �÷��̾��� ������ ����
    public int Damage()
    {
        int baseDamage = Random.Range(MIN_ATK, MAX_ATK+1);

        bool isCritical = Random.value < Crit_Chance;

        //ũ��Ƽ�� Ȯ���� ũ��Ƽ�� Ȯ�� �� ������ ����
        if (isCritical)
        {
            int criticalDamage = Mathf.RoundToInt(baseDamage * Critical);

            return criticalDamage;
        }
        else
        {
            return baseDamage;
        }
    }

    #region �÷��̾� �ǰ�
    // �ǰ� �޼���
    public void GetHit()
    {
        anim.SetTrigger("GetHit");
        Flash();
        //var playerAttack = GetComponent<PlayerAttack>();
        //if (playerAttack != null) { playerAttack.DisableSwordCollider(); }
        //else { Debug.Log("isattacking�������"); }
    }

    //������ ���ݿ� ���� �������� ���� ����� ���� ���� ������ ����
    public void TakeDamage(int damage)
    {
        // �÷��̾ ���� ���̰ų� �뽬���� ���� �������� ����
        if (isAttack || isDash || UseSkill()) return;

        if (Time.time >= lastDamagedTime + invincibleDuration)
        {
            // ���� ������ = �÷��̾��� ���� ������ * (1 - ����%)
            int finalDamage = Mathf.RoundToInt(damage * (1 - Defense));
            Cur_HP -= finalDamage;
            GetHit();
            lastDamagedTime = Time.time;

            if (Cur_HP <= 0)
            {
                Die();
            }
        }
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
    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");
        gameOverPanel.SetActive(true); // ���ӿ��� �г� Ȱ��ȭ
        Invoke("StopTime", 0.6f); // 2�� �ڿ� StopTime �޼��� ����
        isGameOver = true;
    }

    private void StopTime()
    {
        Time.timeScale = 0f;
    }
    #endregion

    #region �ڵ� ȸ�� �� ��ų ����
    // �ڵ� ȸ�� �ý���
    IEnumerator RegenerateStats()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // 1�ʸ��� ����

            // ü�� ȸ��
            if (Cur_HP > 0 && Cur_HP < Max_HP)
            {
                int hpToRecover = Mathf.RoundToInt(HP_Recover);
                Cur_HP += hpToRecover;
                Cur_HP = Mathf.Clamp(Cur_HP, 0, Max_HP);
            }

            // ���׹̳� ȸ��
            if (cur_stamina < Max_Stamina)
            {
                int staminaToRecover = Mathf.RoundToInt(Stamina_Recover);
                cur_stamina += staminaToRecover;
                cur_stamina = Mathf.Clamp(cur_stamina, 0, Max_Stamina);
            }
        }
    }
    private void ApplySkills()
    {
        foreach (SkillData skill in StanbySkills)
        {
            if (skill.isUnlock)
            {
                // ��ų�� �ɷ�ġ�� �÷��̾�� ����
                skill.Apply(this);
            }
        }
    }
    #endregion

    #region �÷��̾� ��ȣ�ۿ�
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

            LevelUpSkill levelUpSkill = collider.GetComponent<LevelUpSkill>();
            if (levelUpSkill != null)
            {
                levelUpSkill.OpenLevelUpUI(skillManager);
                break;
            }

            UnlockSkill unlockSkill = collider.GetComponent<UnlockSkill>();
            if (unlockSkill != null)
            {
                unlockSkill.OpenUnlockUpUI(skillManager);
                break;
            }

            BossSkill bossSkill = collider.GetComponent<BossSkill>();
            if (bossSkill != null)
            {
                bossSkill.BossMemory(gameObject);
                break;
            }

        }
    }
    #endregion

    #region �÷��̾� ��ų ���� �� ���� ����
    void ChangeWeapon()
    {
        // ó���� ���� BaseMesh�� �����ϰ� ����
        if (initialBaseMesh == null)
        {
            initialBaseMesh = GetRandomBaseMesh();
            ApplyBaseMesh(initialBaseMesh);
        }
        else
        {
            // UnlockSkills ����Ʈ�� ��� �ִ��� Ȯ��
            if (UnlockSkills != null && UnlockSkills.Count > 0)
            {
                // ����Ʈ�� ù ��° ��Ҹ� ������
                SkillData firstSkill = UnlockSkills[0];

                if (firstSkill != null)
                {
                    ChangeBaseMesh(firstSkill.element);
                }
            }
            // UnlockSkills ����Ʈ�� ��� �ְų� ù ��° ��Ұ� null�� ���, ó�� ������ ���� ����� ����
        }
    }

    void ChangeBaseMesh(Element element)
    {
        GameObject newBaseMesh = null;

        if (element == Element.Fire)
        {
            newBaseMesh = BaseMesh_Fire;
        }
        else if (element == Element.Water)
        {
            newBaseMesh = BaseMesh_Water;
        }
        else if (element == Element.Air)
        {
            newBaseMesh = BaseMesh_Air;
        }
        else if (element == Element.Earth)
        {
            newBaseMesh = BaseMesh_Earth;
        }
        else
        {
            // ������ �Ӽ��� ���� ���, ���� BaseMesh�� ����
            newBaseMesh = GetRandomBaseMesh();
        }

        if (newBaseMesh != currentBaseMesh)
        {
            ApplyBaseMesh(newBaseMesh);  // BaseMesh ����
            currentBaseMesh = newBaseMesh; // ���� ���� ���̽� �޽� ������Ʈ
        }
    }

    // ���ο� BaseMesh�� �����ϴ� �Լ�
    private void ApplyBaseMesh(GameObject newBaseMesh)
    {
        if (newBaseMesh == null)
        {
            return;
        }

        // ������ ������ ��� GameObject�� ��Ȱ��ȭ
        foreach (Transform childTransform in transform)
        {
            if (childTransform.gameObject != newBaseMesh)
            {
                childTransform.gameObject.SetActive(false);
            }
        }

        // ���� �ִϸ����͸� ������
        Animator currentAnimator = GetComponent<Animator>();
        if (currentAnimator != null)
        {
            // ���ο� BaseMesh�� �ִ� �ִϸ����͸� ������
            Animator newMeshAnimator = newBaseMesh.GetComponent<Animator>();
            if (newMeshAnimator != null)
            {
                currentAnimator.runtimeAnimatorController = newMeshAnimator.runtimeAnimatorController;
                currentAnimator.avatar = newMeshAnimator.avatar;
            }
        }

        // ���ο� BaseMesh Ȱ��ȭ
        newBaseMesh.SetActive(true);
        UpdateBasemesh(newBaseMesh);
        Debug.Log("Activated new base mesh: " + newBaseMesh.name);
        // **�߰�: �� �޽��� �´� ���� Ȱ��ȭ**
        SetActiveSword(GetCurrentSwordNumber());  // GetCurrentSwordNumber �޼ҵ尡 ������ �� ��ȣ ��ȯ
    }


    GameObject GetRandomBaseMesh()
    {
        GameObject[] baseMeshes = { BaseMesh_Fire, BaseMesh_Water, BaseMesh_Air, BaseMesh_Earth };
        int randomIndex = UnityEngine.Random.Range(0, baseMeshes.Length);
        return baseMeshes[randomIndex];
    }
    #endregion


    int GetCurrentSwordNumber()
    {
        if (currentBaseMesh == null)
        {
            Debug.LogWarning("currentBaseMesh is null");
            return 1; // �⺻��
        }

        // currentBaseMesh�� �̸��� �������� �� ��ȣ ��ȯ
        switch (currentBaseMesh.name)
        {
            case "BaseMesh_Water":
                return 1;  // �� �Ӽ��� ��� 1�� ��
            case "BaseMesh_Fire":
                return 2;  // �� �Ӽ��� ��� 2�� ��
            case "BaseMesh_Air":
                return 3;  // ���� �Ӽ��� ��� 3�� ��
            case "BaseMesh_Earth":
                return 4;  // �� �Ӽ��� ��� 4�� ��
            default:
                Debug.LogWarning("Unknown base mesh name: " + currentBaseMesh.name);
                return 1;  // �⺻���� 1�� ��
        }
    }


    void UpdateBasemesh(GameObject newBaseMesh)
    {
        currentBaseMesh = newBaseMesh;  // �Ӽ� ������Ʈ
        SetActiveSword(GetCurrentSwordNumber());  // �Ӽ��� �´� �� Ȱ��ȭ
    }



    // �浹ü�� ����Ʈ(TrailRenderer) Ȱ��ȭ/��Ȱ��ȭ�� ó���ϴ� �޼��� �߰�
    private void SetColliderAndEffectEnabled(Collider collider, TrailRenderer[] trailRenderers, bool isEnabled)
    {
        if (collider != null)
        {
            collider.enabled = isEnabled;
        }
        if (trailRenderers != null)
        {
            foreach (var trailRenderer in trailRenderers)
            {
                if (trailRenderer != null)
                {
                    trailRenderer.enabled = isEnabled;
                }
            }
        }
    }

    private void DisableAllColliders()
    {
        SetColliderAndEffectEnabled(waterACollider, new TrailRenderer[] { waterAEffect }, false);
        SetColliderAndEffectEnabled(fireACollider, new TrailRenderer[] { fireAEffect }, false);
        SetColliderAndEffectEnabled(airACollider, new TrailRenderer[] { airAEffect1, airAEffect2 }, false);
        SetColliderAndEffectEnabled(earthACollider, new TrailRenderer[] { earthAEffect1, earthAEffect2 }, false);
    }

    public void SetActiveSword(int swordNumber)
    {
        // ��� ���� �浹ü ��Ȱ��ȭ
        DisableAllColliders();

        // ���� ��� ���� ������ �浹ü ���� �� Ȱ��ȭ
        switch (swordNumber)
        {
            case 1:
                SetColliderAndEffectEnabled(waterACollider, new TrailRenderer[] { waterAEffect }, true);
                break;
            case 2:
                SetColliderAndEffectEnabled(fireACollider, new TrailRenderer[] { fireAEffect }, true);
                break;
            case 3:
                SetColliderAndEffectEnabled(airACollider, new TrailRenderer[] { airAEffect1, airAEffect2 }, true);
                break;
            case 4:
                SetColliderAndEffectEnabled(earthACollider, new TrailRenderer[] { earthAEffect1, earthAEffect2 }, true);
                break;
            default:
                Debug.LogWarning("Invalid sword number");
                break;
        }
    }

    public void EnableSwordCollider()
    {
        isAttack = true;

        // ���� ���õ� ������ �浹ü Ȱ��ȭ
        SetActiveSword(GetCurrentSwordNumber());
    }

    public void DisableSwordCollider()
    {
        isAttack = false;

        // ���� ���õ� ������ �浹ü ��Ȱ��ȭ
        DisableAllColliders();
    }


    // ���Ϳ��� �浹�� �����ϴ� �޼���
    private void OnTriggerEnter(Collider other)
    {
        if (isAttack && other.CompareTag("Monster"))
        {
            // Try to get the BaseMonster component first
            BaseMonster monster = other.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.TakeDamage(Damage());  // Apply damage to BaseMonster
                return; // Exit after dealing damage to the BaseMonster
            }
            /*
            // If BaseMonster is not found, try to get the Boss component
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(Damage());  // Apply damage to Boss
            }*/
        }
    }

}
