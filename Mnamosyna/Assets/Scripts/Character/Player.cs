using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    float hAxis;
    float vAxis;
    bool leftDown;
    bool rightDown;
    bool isBorder;
    bool isDamage;

    public Camera followCamera;
    bool isAttackReady;

    Vector3 moveVec;
    Animator anim;
    MeshRenderer[] meshs;
    Rigidbody rigid;
    Sword sword;

    public PlayerStat stat;

    float attackDelay;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        sword = GetComponentInChildren<Sword>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        stat = new PlayerStat();
        instance = this;
    }
    void Start()
    {
        stat.cur_hp = stat.max_hp;
        stat.Cur_Stamina = stat.Max_Stamina;
    }


    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        LeftAttack();
        RightAttack();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        leftDown = Input.GetButtonDown("Fire1");
        rightDown = Input.GetButtonDown("Fire3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (leftDown || rightDown)
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

        if (leftDown && isAttackReady)
        {
            sword.Use();
            anim.SetTrigger("LeftAttack");
            attackDelay = 0;
        }
    }

   void RightAttack()
    {
        attackDelay += Time.deltaTime;
        isAttackReady = stat.atk_speed < attackDelay;

        if(rightDown && isAttackReady)
        {
            sword.Use();
            anim.SetTrigger("RightAttack");
            attackDelay = 0;
        }
    }
    public int Damage()
    {
        int damage = Random.Range(stat.min_atk, stat.max_atk+1);
        
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "monster")
        {
            Monster monster = other.GetComponent<Monster>();

            stat.cur_hp -= monster.Damage();

            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.black;
        }
        yield return new WaitForSeconds(1f);
        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
    }
    public void TakeDamage(int damage)
    {
        // 피해 처리
        int finalDamage = Mathf.RoundToInt(damage * (1 - stat.defense)); // 피해 감소 적용
        stat.cur_hp = Mathf.Max(0, stat.cur_hp - finalDamage);

        if (stat.cur_hp == 0)
        {
            //Die();
        }
    }
}
