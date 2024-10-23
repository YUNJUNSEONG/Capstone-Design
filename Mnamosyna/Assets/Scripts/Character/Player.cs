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
    private float attackThreshold = 1.5f; // someThreshold를 attackThreshold로 정의

    private Coroutine CommandCoroutine = null;
    private Queue<string> inputBuffer = new Queue<string>();
    int hashAttackCount = Animator.StringToHash("AttackCount");
    public bool isGameOver = false;


    public Camera followCamera;

    public float invincibleTime = 1.0f; // 무적 지속 시간
    private bool isInvincible = false;

    // 무적 상태의 지속 시간 (초)
    public float invincibleDuration = 2.0f;
    private float lastDamagedTime;

    //공격받을때 깜빡이는 용도
    private float flashDuration = 0.1f;
    private int flashCount = 10;
    private SkillManager skillManager;
    private List<Renderer> renderers;


    Vector3 moveVec;
    public Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;

    // 각 속성에 해당하는 BaseMesh
    GameObject initialBaseMesh = null;
    public GameObject BaseMesh_Fire;
    public GameObject BaseMesh_Water;
    public GameObject BaseMesh_Air;
    public GameObject BaseMesh_Earth;
    // 현재 설정된 BaseMesh
    private GameObject currentBaseMesh;


    public Collider waterACollider;
    public TrailRenderer waterAEffect;
    public Collider fireACollider;
    public TrailRenderer fireAEffect;
    public Collider airACollider;
    public TrailRenderer airAEffect1, airAEffect2;
    public Collider earthACollider;
    public TrailRenderer earthAEffect1, earthAEffect2;


    // 스킬 관련 코드
    //public int maxSkillSlots = 30;
    //public SkillData[] skillSlots;

    // 전체 스킬
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

        skillManager = FindObjectOfType<SkillManager>(); // scene에서 SkillManager 오브젝트를 찾아 할당
        if (skillManager == null)
        {
            Debug.LogError("SkillManager를 찾을 수 없습니다.");
        }

        currentBaseMesh = BaseMesh_Water;
        gameOverPanel.SetActive(false);
        StartCoroutine(RegenerateStats());

        DisableAllColliders();  // 시작 시 모든 충돌체를 비활성화
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
            Destroy(this.gameObject); // 게임 오버 시 오브젝트 파괴
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

    #region 플레이어 이동
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

        // 이동 방향에 따른 회전
        if (moveVec != Vector3.zero)
        {
            // 마우스 위치를 고려하여 회전
            Vector3 targetDirection = moveVec;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 mouseDirection = hitInfo.point - transform.position;
                mouseDirection.y = 0; // y축은 무시
                targetDirection = Vector3.Lerp(moveVec, mouseDirection.normalized, 0.5f); // 이동 방향과 마우스 방향을 혼합
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
        bool skillUsed = UseSkill(); // 스킬이 사용되었는지 확인

        // 스킬이 사용된 경우
        if (skillUsed)
        {
            isAttack = true; // 스킬 사용 중 공격 상태 설정
            StartCoroutine(ActivateColliderForDuration(2f)); // 콜라이더 활성화 및 2초 후 비활성화
        }

        else
        {
            isAttack = true;

            // 동적으로 swordNumber 설정 (예: 플레이어의 현재 속성에 따라)
            int swordNumber = GetCurrentSwordNumber(); // 이 메서드를 정의해서 적절한 검 번호 반환

            SetActiveSword(swordNumber); // 올바른 검 콜라이더 활성화
            FaceMouseDirection();
            anim.SetTrigger("LeftAttack");
            Debug.Log("LeftAttack Triggered");

            attackDelay = 0;
            StartCoroutine(AttackEnd("LeftAttack"));
            if (skillCommand == "LLL")
            {
                skillCommand = "";        // 커맨드 초기화
                inputBuffer.Clear();      // 입력 버퍼 초기화
            }
        }
    }


    void RightAttack()
    {
        skillCommand += 'R';
        bool skillUsed = UseSkill(); // 스킬이 사용되었는지 확인

        // 스킬이 사용된 경우
        if (skillUsed)
        {
            isAttack = true; // 스킬 사용 중 공격 상태 설정
            StartCoroutine(ActivateColliderForDuration(2f)); // 콜라이더 활성화 및 2초 후 비활성화
        }
        else // 스킬이 사용되지 않은 경우
        {
            // 공격 상태를 true로 설정
            isAttack = true;

            // 동적으로 swordNumber 설정 (예: 플레이어의 현재 속성에 따라)
            int swordNumber = GetCurrentSwordNumber(); // 현재 속성에 따라 적절한 검 번호 반환

            SetActiveSword(swordNumber); // 올바른 검 콜라이더 활성화
            FaceMouseDirection(); // 마우스 방향으로 캐릭터 회전

            anim.SetTrigger("RightAttack"); // 오른쪽 공격 애니메이션 트리거
            Debug.Log("RightAttack Triggered"); // 디버그 로그로 트리거 확인

            attackDelay = 0; // 공격 지연 초기화
            StartCoroutine(AttackEnd("RightAttack")); // 공격 종료 대기
        }
    }


    void FaceMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // Y축 회전 방지
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10); // 부드러운 회전
        }
    }

    // 플레이어 대쉬
    void Dash()
    {
        // 스킬이 발동되었는지 확인
        bool skillUsed = UseSkill(); // 스킬이 사용되었는지 확인

        // 스킬이 사용된 경우
        if (skillUsed)
        {
            isAttack = true; // 스킬 사용 중 공격 상태 설정
            StartCoroutine(ActivateColliderForDuration(2f)); // 콜라이더 활성화 및 2초 후 비활성화
        }

        if (!isDash)
        {
            skillCommand += 'S'; // 커맨드에 'S' 추가
            FaceMouseDirection(); // 대쉬 방향 설정

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

        // 애니메이션 속도를 고려한 실제 재생 시간 계산
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
            isAttack = true; // 스킬 사용 시 공격 상태 설정
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

                    Cur_Stamina -= unlockedSkills[i].useStamina; // 스태미나 소모
                    skillCommand = "";  // 커맨드 리셋
                    inputBuffer.Clear(); // 입력 버퍼 비우기

                    return true; // 스킬이 성공적으로 사용됨
                }
                else
                {
                    Debug.Log("스태미너가 부족합니다.");
                    return false; // 스태미너 부족
                }
            }
        }
        return false; // 일치하는 스킬 없음
    }

    IEnumerator ActivateColliderForDuration(float duration)
    {
        // 콜라이더 활성화
        int swordNumber = GetCurrentSwordNumber(); // 현재 속성에 따라 적절한 검 번호 반환
        SetActiveSword(swordNumber); // 올바른 검 콜라이더 활성화

        // 지정된 시간 대기
        yield return new WaitForSeconds(duration);

        // 콜라이더 비활성화
        SetActiveSword(-1); // -1 또는 적절한 값을 사용하여 모든 콜라이더 비활성화
    }


    void ActivateSkillCollider()
    {
        // 동적으로 swordNumber 설정 (예: 플레이어의 현재 속성에 따라)
        int swordNumber = GetCurrentSwordNumber(); // 현재 속성에 따라 적절한 검 번호 반환
        SetActiveSword(swordNumber); // 올바른 검 콜라이더 활성화
    }

    /*

    // 스킬이 발동되었는지 확인하는 메서드
    bool CheckIfSkillUsed()
    {
        // 스킬이 발동되었다면 UseSkill()에서 true를 반환하게 함
        return UseSkill();
    }*/


    IEnumerator AttackEnd(string animationTrigger)
    {
        // 애니메이터의 현재 상태 정보 가져오기
        AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 애니메이션 길이와 속도를 반영하여 실제 재생 시간을 계산
        float animationLength = animStateInfo.length / animStateInfo.speed;

        // 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(animationLength);

        // 현재 선택된 무기의 충돌체 비활성화
        DisableSwordCollider();

        // 입력 버퍼에 값이 있을 경우, 다음 입력 처리
        if (inputBuffer.Count > 0)
        {
            ProcessNextInput();
        }

        Debug.Log("Attack ended and isAttack set to false");
    }


    //입력 처리 및 다음 버퍼 추가
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

    // 플레이어의 스킬 커맨드 초기화 코루틴
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
        inputBuffer.Enqueue(input); // 큐에 추가하는 대신, 입력 즉시 처리
        skillCommand += input;

        // 스킬을 즉시 확인하고 발동
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
            string nextInput = inputBuffer.Peek(); // 큐의 맨 위 요소를 확인
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

    #region 플레이어 피격
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
        if (isAttack || isDash || UseSkill()) return;

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
    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");
        gameOverPanel.SetActive(true); // 게임오버 패널 활성화
        Invoke("StopTime", 0.6f); // 2초 뒤에 StopTime 메서드 실행
        isGameOver = true;
    }

    private void StopTime()
    {
        Time.timeScale = 0f;
    }
    #endregion

    #region 자동 회복 및 스킬 적용
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
    private void ApplySkills()
    {
        foreach (SkillData skill in StanbySkills)
        {
            if (skill.isUnlock)
            {
                // 스킬의 능력치를 플레이어에게 적용
                skill.Apply(this);
            }
        }
    }
    #endregion

    #region 플레이어 상호작용
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

            BossSkill bossSkill = collider.GetComponent<BossSkill>();
            if (bossSkill != null)
            {
                bossSkill.BossMemory(gameObject);
                break;
            }

        }
    }
    #endregion

    #region 플레이어 스킬 선택 시 무기 변경
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

        if (newBaseMesh != currentBaseMesh)
        {
            ApplyBaseMesh(newBaseMesh);  // BaseMesh 적용
            currentBaseMesh = newBaseMesh; // 현재 무기 베이스 메쉬 업데이트
        }
    }

    // 새로운 BaseMesh를 적용하는 함수
    private void ApplyBaseMesh(GameObject newBaseMesh)
    {
        if (newBaseMesh == null)
        {
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
                currentAnimator.runtimeAnimatorController = newMeshAnimator.runtimeAnimatorController;
                currentAnimator.avatar = newMeshAnimator.avatar;
            }
        }

        // 새로운 BaseMesh 활성화
        newBaseMesh.SetActive(true);
        UpdateBasemesh(newBaseMesh);
        Debug.Log("Activated new base mesh: " + newBaseMesh.name);
        // **추가: 새 메쉬에 맞는 검을 활성화**
        SetActiveSword(GetCurrentSwordNumber());  // GetCurrentSwordNumber 메소드가 적절한 검 번호 반환
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
            return 1; // 기본값
        }

        // currentBaseMesh의 이름을 기준으로 검 번호 반환
        switch (currentBaseMesh.name)
        {
            case "BaseMesh_Water":
                return 1;  // 물 속성일 경우 1번 검
            case "BaseMesh_Fire":
                return 2;  // 불 속성일 경우 2번 검
            case "BaseMesh_Air":
                return 3;  // 공기 속성일 경우 3번 검
            case "BaseMesh_Earth":
                return 4;  // 땅 속성일 경우 4번 검
            default:
                Debug.LogWarning("Unknown base mesh name: " + currentBaseMesh.name);
                return 1;  // 기본값은 1번 검
        }
    }


    void UpdateBasemesh(GameObject newBaseMesh)
    {
        currentBaseMesh = newBaseMesh;  // 속성 업데이트
        SetActiveSword(GetCurrentSwordNumber());  // 속성에 맞는 검 활성화
    }



    // 충돌체와 이펙트(TrailRenderer) 활성화/비활성화를 처리하는 메서드 추가
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
        // 모든 무기 충돌체 비활성화
        DisableAllColliders();

        // 현재 사용 중인 무기의 충돌체 선택 및 활성화
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

        // 현재 선택된 무기의 충돌체 활성화
        SetActiveSword(GetCurrentSwordNumber());
    }

    public void DisableSwordCollider()
    {
        isAttack = false;

        // 현재 선택된 무기의 충돌체 비활성화
        DisableAllColliders();
    }


    // 몬스터와의 충돌을 감지하는 메서드
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
