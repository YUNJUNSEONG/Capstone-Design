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
    public bool isAttackReady = true;
    private float attackDelay = 0.0f;
    private float attackThreshold = 1.5f; // someThreshold를 attackThreshold로 정의
    public bool isDash = false;
    private Coroutine CommandCoroutine = null;
    private Queue<string> inputBuffer = new Queue<string>();

    public Camera followCamera;

    public float invincibleTime = 1.0f; // 무적 지속 시간
    private bool isInvincible = false;

    // 무적 상태의 지속 시간 (초)
    public float invincibleDuration = 2.0f;
    private float lastDamagedTime;

    //공격받을때 깜빡이는 용도
    private float flashDuration = 0.1f;
    private int flashCount = 5;
    private SkillManager skillManager;
    private List<Renderer> renderers;


    Vector3 moveVec;
    Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;
    Sword sword;
    // 각 속성에 해당하는 BaseMesh
    GameObject initialBaseMesh = null;
    public GameObject BaseMesh_Fire;
    public GameObject BaseMesh_Water;
    public GameObject BaseMesh_Air;
    public GameObject BaseMesh_Earth;
    // 현재 설정된 BaseMesh
    private GameObject currentBaseMesh;


    // 스킬 관련 코드
    //public int maxSkillSlots = 30;
    //public SkillData[] skillSlots;

    // 전체 스킬
    [SerializeField]
    private List<SkillData> allSkills = new List<SkillData>();
    // 플레이어가 보유한 스킬 ID를 저장하는 리스트
    //private List<int> unlockSkillIds = new List<int>();
    // 플레이어가 보유한 스킬 데이터를 저장하는 리스트
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
    private string skillCammand;
    public string SkillCammand
    {
        get { return skillCammand; }
        set { skillCammand = value; }
    }


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        sword = GetComponentInChildren<Sword>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    protected override void Start()
    {
        base.Start();
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

        skillManager = FindObjectOfType<SkillManager>(); // scene에서 SkillManager 오브젝트를 찾아 할당
        if (skillManager == null)
        {
            Debug.LogError("SkillManager를 찾을 수 없습니다.");
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
        leftDown = Input.GetButtonDown("Fire1");
        rightDown = Input.GetButtonDown("Fire2");
        spaceDown = Input.GetButtonDown("Dash");
    }


    void Move()
    {
        // 플레이어 입력을 화면 공간으로 변환
        Vector3 inputDirection = new Vector3(hAxis, 0, vAxis);
        inputDirection = Camera.main.transform.TransformDirection(inputDirection);
        inputDirection.y = 0; // y축 변환은 무시 (플레이어는 바닥을 따라 움직임)

        // 이동 벡터 정규화
        moveVec = inputDirection.normalized;

        if (!isBorder)
        {
            // 플레이어를 이동 방향으로 이동
            transform.position += moveVec * Move_Speed * Time.deltaTime;
        }

        // 이동 여부에 따라 애니메이션 설정
        anim.SetBool("isRun", moveVec != Vector3.zero);

        // 플레이어가 이동하는 방향을 바라보도록 함
        if (moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
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

    void LeftAttack()
    {
        if (isAttackReady)// && !isDash)
        {
            UseSkill();

            if (!isAttack)
            {
                isAttack = true;
                var playerAttack = GetComponent<PlayerAttack>();
                if (playerAttack != null)
                {
                    playerAttack.EnableSwordCollider();
                }
                anim.SetTrigger("LeftAttack");
                attackDelay = 0;
                StartCoroutine(AttackEnd("LeftAttack"));
            }
        }
    }

    void RightAttack()
    {
        if (isAttackReady)// && !isDash)
        {
            UseSkill();

            if (!isAttack)
            {
                isAttack = true;
                var playerAttack = GetComponent<PlayerAttack>();
                if (playerAttack != null)
                {
                    playerAttack.EnableSwordCollider();
                }
                anim.SetTrigger("RightAttack");
                attackDelay = 0;
                StartCoroutine(AttackEnd("RightAttack"));
            }
        }
    }
    // 플레이어 대쉬
    void Dash()
    {
        if (!isDash)
        {
            UseSkill();
            if (isAttackReady && !isAttack)
            {
                isDash = true;
                anim.SetTrigger("Dash");
                StartCoroutine(DashEnd("Dash"));
            }
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

        // 애니메이션 속도를 고려한 실제 재생 시간 계산
        float animationLength = stateInfo.length / anim.speed;
        yield return new WaitForSeconds(animationLength / 2);

        isDash = false;
        isAttackReady = true;

        if (inputBuffer.Count > 0)
        {
            ProcessNextInput();
        }
    }

    // 스킬 사용 메서드
    void UseSkill()
    {
        for (int i = unlockedSkills.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(unlockedSkills[i].Command) && skillCammand.EndsWith(unlockedSkills[i].Command))
            {
                if (unlockedSkills[i].Level > 0 && Cur_Stamina >= unlockedSkills[i].useStamina)
                {
                    isAttack = true;
                    isAttackReady = false;
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
                    StartCoroutine(ResetAttack());

                    Cur_Stamina -= unlockedSkills[i].useStamina;
                    skillCammand = "";
                    inputBuffer.Clear();
                    break;
                }
                else
                {
                    Debug.Log("스테미너가 부족합니다.");
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

        // 애니메이션 길이의 절반만큼 대기
        float animationLength = stateInfo.length / 2;
        yield return new WaitForSeconds(animationLength);

        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.DisableSwordCollider();
        }

        isAttack = false;
        isAttackReady = true;

        if (inputBuffer.Count > 0)
        {
            ProcessNextInput();
        }
    }
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f); // 또는 스킬 지속 시간
        isAttack = false;
        isAttackReady = true;
        Debug.Log("Attack reset in Coroutine");
    }
    //입력 처리 및 다음 버퍼 추가
    void HandleInput()
    {
        attackDelay += Time.deltaTime;
        isAttackReady = ATK_Speed <= attackDelay;

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

    // 플레이어의 스킬 커맨드 초기화 코루틴
    IEnumerator ClearCommand()
    {
        yield return new WaitForSeconds(3);
        skillCammand = " ";
    }

    //커맨드 버퍼에 넣기
    void AddInputToBuffer(string input)
    {
        inputBuffer.Enqueue(input);
        if (CommandCoroutine != null)
        {
            StopCoroutine(CommandCoroutine);
        }
        skillCammand += input;
        CommandCoroutine = StartCoroutine(ClearCommand());
    }
    //입력 버퍼에서 다음 입력을 꺼내어 처리
    void ProcessNextInput()
    {
        if (inputBuffer.Count > 0 && !isAttack)
        {
            string nextInput = inputBuffer.Peek(); // 큐의 맨 위 요소를 확인

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
                    inputBuffer.Dequeue(); // 대쉬 입력은 항상 하나만 처리
                    break;
            }
        }
    }

    void HandleLeftClick()
    {
        int leftClickCount = 0;
        while (inputBuffer.Count > 0 && inputBuffer.Peek() == "L" && leftClickCount < 3)
        {
            inputBuffer.Dequeue();
            leftClickCount++;
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


    // 플레이어의 데미지 설정
    public int Damage()
    {
        int baseDamage = Random.Range(MIN_ATK, MAX_ATK+1);

        bool isCritical = Random.value < Crit_Chance;

        //크리티컬 확률로 크리티컬 확인 후 데미지 적용
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


    // 피격 메서드
    public void GetHit()
    {
        anim.SetTrigger("GetHit");
        Flash();
        //var playerAttack = GetComponent<PlayerAttack>();
        //if (playerAttack != null) { playerAttack.DisableSwordCollider(); }
        //else { Debug.Log("isattacking변경실패"); }
    }

    //몬스터의 공격에 의한 데미지를 방어력 계산을 통해 최종 데미지 산출
    public void TakeDamage(int damage)
    {
        // 플레이어가 공격 중이거나 대쉬중일 때는 데미지를 무시
        if (isAttack || isDash) return;

        if (Time.time >= lastDamagedTime + invincibleDuration)
        {
            // 최종 데미지 = 플레이어의 공격 데미지 * (1 - 방어력%)
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
    IEnumerator RegenerateStats()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // 1초마다 실행

            // 체력 회복
            if (Cur_HP > 0 && Cur_HP < Max_HP)
            {
                int hpToRecover = Mathf.RoundToInt(HP_Recover);
                Cur_HP += hpToRecover;
                Cur_HP = Mathf.Clamp(Cur_HP, 0, Max_HP);
            }

            // 스테미나 회복
            if (cur_stamina < Max_Stamina)
            {
                int staminaToRecover = Mathf.RoundToInt(Stamina_Recover);
                cur_stamina += staminaToRecover;
                cur_stamina = Mathf.Clamp(cur_stamina, 0, Max_Stamina);
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

    void ChangeWeapon()
    {
        // 처음에 랜덤 BaseMesh를 설정하고 저장
        if (initialBaseMesh == null)
        {
            initialBaseMesh = GetRandomBaseMesh();
            ApplyBaseMesh(initialBaseMesh);
        }
        else
        {
            // UnlockSkills 리스트가 비어 있는지 확인
            if (UnlockSkills != null && UnlockSkills.Count > 0)
            {
                // 리스트의 첫 번째 요소를 가져옴
                SkillData firstSkill = UnlockSkills[0];

                if (firstSkill != null)
                {
                    ChangeBaseMesh(firstSkill.element);
                }
            }
            // UnlockSkills 리스트가 비어 있거나 첫 번째 요소가 null인 경우, 처음 설정된 랜덤 무기로 고정
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
            // 지정된 속성이 없는 경우, 랜덤 BaseMesh로 설정
            newBaseMesh = GetRandomBaseMesh();
        }

        ApplyBaseMesh(newBaseMesh); // 새로운 BaseMesh 적용
    }

    // 새로운 BaseMesh를 적용하는 함수
    private void ApplyBaseMesh(GameObject newBaseMesh)
    {
        if (newBaseMesh == null)
        {
            // 새로운 BaseMesh가 없으면 아무것도 하지 않음
            return;
        }

        // 이전에 설정된 모든 GameObject를 비활성화
        foreach (Transform childTransform in transform)
        {
            if (childTransform.gameObject != newBaseMesh)
            {
                childTransform.gameObject.SetActive(false);
            }
        }

        // 현재 애니메이터를 가져옴
        Animator currentAnimator = GetComponent<Animator>();
        if (currentAnimator != null)
        {
            // 새로운 BaseMesh에 있는 애니메이터를 가져옴
            Animator newMeshAnimator = newBaseMesh.GetComponent<Animator>();
            if (newMeshAnimator != null)
            {
                // 애니메이션 컨트롤러를 교체
                currentAnimator.runtimeAnimatorController = newMeshAnimator.runtimeAnimatorController;

                // 아바타를 교체
                currentAnimator.avatar = newMeshAnimator.avatar;
            }
        }

        // 새로운 BaseMesh를 활성화
        newBaseMesh.SetActive(true);
    }

    GameObject GetRandomBaseMesh()
    {
        GameObject[] baseMeshes = { BaseMesh_Fire, BaseMesh_Water, BaseMesh_Air, BaseMesh_Earth };
        int randomIndex = UnityEngine.Random.Range(0, baseMeshes.Length);
        return baseMeshes[randomIndex];
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");
        gameOverPanel.SetActive(true); // 게임오버 패널 활성화
        Invoke("StopTime", 0.6f); // 2초 뒤에 StopTime 메서드 실행
    }

    private void StopTime()
    {
        Time.timeScale = 0f;
    }
}