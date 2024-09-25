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
    public bool isSkillActive = false;
    private float attackDelay = 0.0f;
    private float attackThreshold = 1.5f; // someThreshold�� attackThreshold�� ����

    private Coroutine CommandCoroutine = null;
    private Queue<string> inputBuffer = new Queue<string>();
    int hashAttackCount = Animator.StringToHash("AttackCount");

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
    Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;
    Sword sword;
    // �� �Ӽ��� �ش��ϴ� BaseMesh
    GameObject initialBaseMesh = null;
    public GameObject BaseMesh_Fire;
    public GameObject BaseMesh_Water;
    public GameObject BaseMesh_Air;
    public GameObject BaseMesh_Earth;
    // ���� ������ BaseMesh
    private GameObject currentBaseMesh;


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
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        sword = GetComponentInChildren<Sword>();
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
        UseSkill();

        if (!isAttack)
        {
            isAttack = true;
            skillCommand += 'L';
            var playerAttack = GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                playerAttack.EnableSwordCollider();
            }

            // ���콺 ��ġ�� �ٶ󺸵��� ����
            FaceMouseDirection();

            // LeftAttack Ʈ���� ����
            anim.SetTrigger("LeftAttack");

            attackDelay = 0;
            StartCoroutine(AttackEnd("LeftAttack"));
        }
    }

    void RightAttack()
    {
        UseSkill();

        if (!isAttack)
        {
            skillCommand += 'R';
            isAttack = true;
            var playerAttack = GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                playerAttack.EnableSwordCollider();
            }

            // ���콺 ��ġ�� �ٶ󺸵��� ����
            FaceMouseDirection();

            anim.SetTrigger("RightAttack");
            attackDelay = 0;
            StartCoroutine(AttackEnd("RightAttack"));
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
        if (!isDash)
        {
            UseSkill();
            skillCommand += 'S';
            FaceMouseDirection();

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


    // ��ų ��� �޼���
    void UseSkill()
    {
        for (int i = unlockedSkills.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(unlockedSkills[i].Command) && skillCommand.EndsWith(unlockedSkills[i].Command))
            {
                if (unlockedSkills[i].Level > 0 && Cur_Stamina >= unlockedSkills[i].useStamina)
                {
                    isAttack = true;
                    //isAttackReady = false;
                    Debug.Log(unlockedSkills[i].AnimationTrigger);
                    if (CommandCoroutine != null)
                    {
                        StopCoroutine(CommandCoroutine);
                    }
                    float floatSkillDamage = unlockedSkills[i].damagePercent * Damage();
                    int intSkillDamage = Mathf.RoundToInt(floatSkillDamage);
                    float floatLevelDamage = unlockedSkills[i].Level * unlockedSkills[i].addDmg;
                    int intLevelDamage = Mathf.RoundToInt(floatLevelDamage);
                    int skillDamage = intSkillDamage + intLevelDamage;

                    anim.SetTrigger(unlockedSkills[i].AnimationTrigger);
                    var playerAttack = GetComponent<PlayerAttack>();
                    if (playerAttack != null) { playerAttack.EnableSwordCollider(); }
                    StartCoroutine(AttackEnd(unlockedSkills[i].AnimationTrigger));

                    Cur_Stamina -= unlockedSkills[i].useStamina;
                    skillCommand = "";
                    inputBuffer.Clear();
                    break;
                }
                else
                {
                    Debug.Log("���׹̳ʰ� �����մϴ�.");
                    break;
                }
            }
        }
    }

    IEnumerator AttackEnd(string animationTrigger)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName(animationTrigger))
        {
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }

        // �ִϸ��̼� ������ ���ݸ�ŭ ���
        float animationLength = stateInfo.length / 2;
        yield return new WaitForSeconds(animationLength);

        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.DisableSwordCollider();
        }

        isAttack = false;
        //isAttackReady = true;

        if (inputBuffer.Count > 0)
        {
            ProcessNextInput();
        }
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
        yield return new WaitForSeconds(3);
        skillCommand = " ";
    }

    //Ŀ�ǵ� ���ۿ� �ֱ�
    void AddInputToBuffer(string input)
    {
        inputBuffer.Enqueue(input);
        if (CommandCoroutine != null)
        {
            StopCoroutine(CommandCoroutine);
        }
        skillCommand += input;
        CommandCoroutine = StartCoroutine(ClearCommand());
    }

    //�Է� ���ۿ��� ���� �Է��� ������ ó��
    void ProcessNextInput()
    {
        if (inputBuffer.Count > 0)//&& !isAttack)
        {
            string nextInput = inputBuffer.Peek(); // ť�� �� �� ��Ҹ� Ȯ��

            switch (nextInput)
            {
                case "L":
                    HandleLeftClick();
                    break;
                case "R":
                    HandleRightClick();
                    break;
                case "S":
                    Dash();
                    inputBuffer.Dequeue(); // �뽬 �Է��� �׻� �ϳ��� ó��
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
        if (isAttack || isDash) return;

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

        ApplyBaseMesh(newBaseMesh); // ���ο� BaseMesh ����
    }

    // ���ο� BaseMesh�� �����ϴ� �Լ�
    private void ApplyBaseMesh(GameObject newBaseMesh)
    {
        if (newBaseMesh == null)
        {
            // ���ο� BaseMesh�� ������ �ƹ��͵� ���� ����
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
                // �ִϸ��̼� ��Ʈ�ѷ��� ��ü
                currentAnimator.runtimeAnimatorController = newMeshAnimator.runtimeAnimatorController;

                // �ƹ�Ÿ�� ��ü
                currentAnimator.avatar = newMeshAnimator.avatar;
            }
        }

        // ���ο� BaseMesh�� Ȱ��ȭ
        newBaseMesh.SetActive(true);
    }

    GameObject GetRandomBaseMesh()
    {
        GameObject[] baseMeshes = { BaseMesh_Fire, BaseMesh_Water, BaseMesh_Air, BaseMesh_Earth };
        int randomIndex = UnityEngine.Random.Range(0, baseMeshes.Length);
        return baseMeshes[randomIndex];
    }
    #endregion

}
