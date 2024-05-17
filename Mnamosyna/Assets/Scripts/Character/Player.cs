using NUnit.Framework.Interfaces;
//using skill;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static SkillData;


public class Player : PlayerStat
{

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

    public float invincibleTime = 3.0f; // 무적 지속 시간
    private bool isInvincible = false;

    // 무적 상태의 지속 시간 (초)
    public float invincibleDuration = 2.0f;
    private float lastDamagedTime;

    //공격받을때 깜빡이는 용도
    private float flashDuration = 0.1f;
    private int flashCount = 4;
    private SkillManager skillManager;
    private List<Renderer> renderers;


    Vector3 moveVec;
    Animator anim;
    SkinnedMeshRenderer[] meshs;
    Rigidbody rigid;
    Sword sword;
    // 각 속성에 해당하는 BaseMesh
    public GameObject BaseMesh_Fire;
    public GameObject BaseMesh_Water;
    public GameObject BaseMesh_Air;
    public GameObject BaseMesh_Earth;
    //public GameObject BaseMesh_Earth; => 나중에 추가하기
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
 

    float attackDelay;

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

        skillManager = FindObjectOfType<SkillManager>(); // scene에서 SkillManager 오브젝트를 찾아 할당
        if (skillManager == null)
        {
            Debug.LogError("SkillManager를 찾을 수 없습니다.");
        }

        currentBaseMesh = BaseMesh_Water;

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
        ChangeWeapon();
        //ttackInvincible();

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
            transform.position += moveVec * Move_Speed * Time.deltaTime;
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
        isAttackReady = ATK_Speed < attackDelay;

        if (leftDown && isAttackReady && !isAttack && !isDash)
        {
            if (CommandCoroutine != null)
                StopCoroutine(CommandCoroutine);
            skillCammand += 'L';
            CommandCoroutine = StartCoroutine(ClearCommand());
            UseSkill();

            if (!isAttack)
            {
                var playerAttack = GetComponent<PlayerAttack>();
                if (playerAttack != null) { playerAttack.EnableSwordCollider(); }
                //sword.Use(Left_ATK_Speed, Damage());
                anim.SetTrigger("LeftAttack");
                attackDelay = 0;
                Invoke("attackend", 2.0f);
            }

        }
    }

   void RightAttack()
    {
        attackDelay += Time.deltaTime;
        isAttackReady = ATK_Speed < attackDelay;

        if(rightDown && isAttackReady && !isAttack && !isDash)
        {
            if (CommandCoroutine != null)
                StopCoroutine(CommandCoroutine);
            skillCammand += 'R';
            CommandCoroutine = StartCoroutine(ClearCommand());
            UseSkill();
            if (!isAttack)
            {
                var playerAttack = GetComponent<PlayerAttack>();
                if (playerAttack != null) { playerAttack.EnableSwordCollider(); }
                //sword.Use(Right_ATK_Speed, Damage());
                anim.SetTrigger("RightAttack");
                attackDelay = 0;
                Invoke("attackend", 2.0f);
            }

        }
    }

    void attackend()
    {
        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null) { playerAttack.DisableSwordCollider(); }
    }
    // 공격 끝났는지 확인하는 코루틴 => 삭제 가능성 높음
    IEnumerator AttackEnd(float attackTime, string animationBool)
    {
        anim.SetTrigger(animationBool);
        yield return new WaitForSeconds(attackTime/2);
        isAttack = false;
        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null) { playerAttack.DisableSwordCollider(); }
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
        //var playerAttack = GetComponent<PlayerAttack>();
        //if (playerAttack != null) { playerAttack.DisableSwordCollider(); }
        //else { Debug.Log("isattacking변경실패"); }
    }

    //몬스터의 공격에 의한 데미지를 방어력 계산을 통해 최종 데미지 산출
    public void TakeDamage(int damage)
    {
        if (Time.time >= lastDamagedTime + invincibleDuration)
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
        if (unlockSkills.Count > 0)
        {
            attackDelay += Time.deltaTime;
            isAttackReady = ATK_Speed < attackDelay;

            if (shiftDown && isAttackReady && !isAttack && !isDash)
            {

                if (CommandCoroutine != null)
                    StopCoroutine(CommandCoroutine);
                skillCammand += 'S';
                CommandCoroutine = StartCoroutine(ClearCommand());
                UseSkill();
                if (!isAttack)
                {
                    var playerAttack = GetComponent<PlayerAttack>();
                    if (playerAttack != null) { playerAttack.EnableSwordCollider(); }
                    //sword.Use(Dash_speed, Damage());
                    anim.SetTrigger("Dash");
                    attackDelay = 0;
                    Invoke("attackend", 2.0f);
                }
            }
        }
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
                    //sword.Use(allSkills[i].AnimationTime, skilldamage);
                    StartCoroutine(AttackEnd(allSkills[i].AnimationTime, allSkills[i].AnimationTrigger));
                    var playerAttack = GetComponent<PlayerAttack>();
                    if (playerAttack != null) { playerAttack.EnableSwordCollider(); }

                    cur_stamina -= (unlockSkills[i].useStamina);
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
            
            LevelUpSkill levelUpSkill = collider.GetComponent<LevelUpSkill>();
            if (levelUpSkill != null)
            {
                levelUpSkill.OpenLevelUpUI(skillManager);
                break;
            }

            UnlockSkill unlockSkill = collider.GetComponent<UnlockSkill>();
            if(unlockSkill != null)
            {
                unlockSkill.OpenUnlockUpUI(skillManager);
                break;
            }
            
        }
    }

    void ChangeWeapon()
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
            else
            {
                // 만약 첫 번째 스킬 데이터가 null인 경우, 기본 BaseMesh로 설정
                ApplyBaseMesh(BaseMesh_Water); // 기본 BaseMesh로 설정
            }
        }
        else
        {
            // UnlockSkills 리스트가 비어 있거나 첫 번째 요소가 null인 경우, 기본 BaseMesh로 설정
            ApplyBaseMesh(BaseMesh_Water); // 기본 BaseMesh로 설정
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
            // 지정된 속성이 없는 경우, 기본 BaseMesh로 설정
            newBaseMesh = BaseMesh_Water;
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

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible)
        {
            // 플레이어가 무적 상태인 경우에는 몬스터 레이어와의 충돌을 무시
            if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                Physics.IgnoreCollision(other, GetComponent<Collider>());
            }
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            // 플레이어가 몬스터와 충돌한 경우
            PenetrateMonster(other);
        }
    }
    private Collider currentMonsterCollider;

    public void PenetrateMonster(Collider monsterCollider)
    {
        // 플레이어를 무적 상태로 전환
        isInvincible = true;
        currentMonsterCollider = monsterCollider;

        // GetHit 메서드 호출
        //GetHit();

        StartCoroutine(BecomeInvincible());
    }

    private IEnumerator BecomeInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);

        // 무적 상태가 끝나면 다시 몬스터와의 충돌을 감지하도록 설정
        isInvincible = false;
        if (currentMonsterCollider != null)
        {
            Physics.IgnoreCollision(currentMonsterCollider, GetComponent<Collider>(), false);
        }
    }


    void AttackInvincible()
    {
        if(isAttack == true)
        {
            isInvincible = true;

            StartCoroutine(BecomeInvincible());
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"), true);
        }
    }*/

}
