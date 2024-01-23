using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public MobStat mobStat;
    public Transform player;
    public static Monster instance;

    public bool isChase;
    public bool isAttack;
    public bool isSkill;
    bool isSkillCool;
    bool isSkillCoolInProgress;
    bool isDamage;

    public Transform target;
    public BoxCollider attackArea;
    public BoxCollider skillArea;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        mobStat = GetComponent<MobStat>();
        instance = this;
        player = GameObject.FindWithTag("Player").transform;

        Invoke("ChaseStart",4);
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(player.position);
            nav.isStopped = !isChase;
        }
        
    }


    //플레이어 추적 시작
    void ChaseStart()
    {
        SetNavSpeed(mobStat.move_speed);
        isChase = true;
        anim.SetBool("isRun", true);
    }

    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            // NavMeshAgent의 속도를 몬스터의 이동 속도로 설정합니다.
            nav.speed = speed;
        }
    }

    //몬스터 피격 확인 및 데미지 부여
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword")
        {
            if(!isDamage)
            {
                if (Player.instance != null)
                {
                    int damage = Player.instance.Damage();

                    int finalDamage = Mathf.RoundToInt(damage * (1 - mobStat.defense));
                    mobStat.cur_hp = Mathf.Max(0, mobStat.cur_hp - finalDamage);

                    Debug.Log("몬스터가 받은 피해 :" + finalDamage);

                }

                Vector3 reactVec = transform.position - other.transform.position;

                StartCoroutine(OnDamage(reactVec));
            }
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
        Targeting();
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    // 몬스터의 플레이어 타게팅
    void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,LayerMask.GetMask("Player"));
        
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());

            //몬스터가 플레이어를 마주친 이후 부터 스킬 쿨타임 활성화
            StartCoroutine(SkillCool());
        }
    }

    IEnumerator Attack()
    {

        //스킬이 쿨타임일 경우
        if (isSkillCool)
        {
            isChase = false;
            isAttack = true;
            anim.SetBool("isAttack", true);

            yield return new WaitForSeconds(0.2f);
            attackArea.enabled = true;


            yield return new WaitForSeconds(0.3f);
            attackArea.enabled = false;

            yield return new WaitForSeconds(mobStat.atk_speed);
            isChase = true;
            isAttack = false;
            anim.SetBool("isAttack", false);
        }
        //스킬이 쿨타임이 아닐 경우
        else
        {
            //스킬 사용
            StartCoroutine(Skill());
            //스킬 쿨타임 시작
            StartCoroutine(SkillCool());
        }

    }

    IEnumerator Skill()
    {
        isChase = false;
        isAttack = true;
        isSkill = true;
        anim.SetBool("isSkill", true);

        yield return new WaitForSeconds(0.2f);
        skillArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        skillArea.enabled = false;

        yield return new WaitForSeconds(mobStat.atk_speed);
        isChase = true;
        isAttack = false;
        isSkill = false;
        anim.SetBool("isSkill", false);
    }

    IEnumerator SkillCool() 
    {
        // 스킬 쿨타임이 초기화되지 않도록 조정
        if (!isSkillCoolInProgress)
        {
            isSkillCoolInProgress = true;
            isSkillCool = true;
            yield return new WaitForSeconds(mobStat.skill_colltime);
            isSkillCool = false;
            isSkillCoolInProgress = false;  // 추가: Coroutine이 끝날 때 변수 초기화
        }
    }

    public int Damage()
    {
        int damage = mobStat.attack;
        int skillDamage = mobStat.skill_attack;

        if(isSkill)
        {
            return skillDamage;
        }
        else
        {
            return damage;
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        isDamage = true; //몬스터 무적시간
        // 피해를 받았을 때 잠시 색상을 반투명하게 변경
        ChangeMaterialTransparency(0.5f);

        yield return new WaitForSeconds(1f); // 무적시간 1초

        if (mobStat.cur_hp > 0)
        {
            ChangeMaterialTransparency(1.0f);
            anim.SetTrigger("getHit");

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 1.5f, ForceMode.Impulse);
        }
        else
        {
            ChangeMaterialTransparency(1.0f);
            gameObject.layer = 11;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("Die");

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            Destroy(gameObject,3);
        }

        isDamage = false;
    }

    void ChangeMaterialTransparency(float alphaValue)
    {
        // 머티리얼이 지정되어 있지 않으면 종료
        if (mat == null)
        {
            Debug.LogError("Material is not assigned.");
            return;
        }

        // 머티리얼의 색상을 가져오고 알파 값을 설정
        Color color = mat.color;
        color.a = alphaValue;
        mat.color = color;
    }

}
