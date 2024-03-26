using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Monster : MonoBehaviour
{

    public MobStat mobStat;
    public Transform player;

    protected const float WAIT_TIME = 0.2f;

    protected bool isChase = false;
    protected bool isAttack = false;
    protected bool isDamage = false;
    protected bool isDead = false;

    public Transform target;
    public BoxCollider attackArea;
    public BoxCollider skillArea;

    protected float skillCool; 

    protected Rigidbody rigid;
    protected BoxCollider boxCollider;
    protected Material mat;
    protected NavMeshAgent nav;
    protected Animator anim;


    // 추가: 코루틴 정지를 위한 변수
    private Coroutine stateCoroutine;


    protected void Start()
    {
        // 변경: 코루틴 시작을 메서드로 호출
        StartStateCoroutines();

        skillCool = mobStat.skill_colltime; // 초기 스킬 쿨다운 설정
        StartCoroutine(SkillCooldownTimer());
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        mobStat = GetComponent<MobStat>();
        player = GameObject.FindWithTag("Player").transform;

        Invoke("ChaseStart", 1.0f);
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(player.position);
            nav.isStopped = !isChase;
        }

    }

    // 변경: 코루틴을 시작하는 메서드
    void StartStateCoroutines()
    {
        stateCoroutine = StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    // 변경: 코루틴 정지하는 메서드
    void StopStateCoroutines()
    {
        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);
    }

    protected virtual IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(WAIT_TIME);

            float dist = Vector3.Distance(player.position, transform.position);
        }
    }

    protected virtual IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            yield return null;
        }
    }

    //플레이어 추적 시작
    void ChaseStart()
    {
        SetNavSpeed(mobStat.move_speed);
        isChase = true;
        anim.SetBool("isChase", true);
    }

    void SetNavSpeed(float speed)
    {
        if (nav != null)
        {
            // NavMeshAgent의 속도를 몬스터의 이동 속도로 설정합니다.
            nav.speed = speed;
        }
    }

    IEnumerator SkillCooldownTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초마다 스킬 쿨다운을 감소시킴
            skillCool = Mathf.Max(0, skillCool - 1);
        }
    }

    //몬스터 피격 확인 및 데미지 부여
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            if (!isDamage)
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

    public int Damage()
    {
        int damage = mobStat.attack;
        return damage;

    }
    public int SkillDamage()
    {
        int skilldamage = mobStat.skill_attack;
        return skilldamage;
    }

    #region OnDamage
    // 무적 시간 상수 정의
    private const float INVINCIBILITY_TIME = 1f;
    IEnumerator OnDamage(Vector3 reactVec)
    {
        SetColor(Color.black); // 피격 시 색상을 변경

        yield return new WaitForSeconds(WAIT_TIME);

        if (mobStat.cur_hp > 0)
        {
            Hit(reactVec); // 피해를 처리하는 함수 호출
        }
        else
        {
            Death(reactVec); // 사망 처리하는 함수 호출
        }

        yield return new WaitForSeconds(INVINCIBILITY_TIME);
        SetColor(Color.white); // 원래 색상으로 변경
    }
    // 피격 시 색상 변경 함수
    void SetColor(Color color)
    {
        mat.color = color;
    }

    // 무적 상태 설정

    // 피해를 처리하는 함수
    void Hit(Vector3 reactVec)
    {
        isDamage = true;
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 1.5f, ForceMode.Impulse);
    }

    // 사망을 처리하는 함수
    public virtual void Death(Vector3 reactVec)
    {
        isDead = true;
        anim.SetTrigger("isDead");
        gameObject.layer = 11; // 사망한 몬스터의 레이어 변경
        isChase = false; // 추적 중지
        nav.enabled = false; // 네비게이션 비활성화
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 5, ForceMode.Impulse);

        Destroy(gameObject, 2); // 일정 시간 후 게임 오브젝트 삭제
        OnDestroy();
    }
    #endregion

    // 변경: 게임 오브젝트가 파괴될 때 코루틴 정지
    protected void OnDestroy()
    {
        StopStateCoroutines();
    }

}
