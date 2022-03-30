using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMonster : MonoBehaviour
{
    //체력 관련은 정수형 (int로 선언. float는 공간차지가 더함)
    // 약자들은 상속, 선언, 매개 변수때 사용할 것
    public int currentHP,fullHP, atkPower;

    //이속 점프높이, 평타쿨(고정), 평타쿨계산(증감), 스킬" ,인식범위 
    protected float moveSpeed, jumpPower, atkCoolTime, skill_Cool,DetectRanX, DetectRanY;
    protected float AtkRanX, AtkRanY;
    //몬스터 상태, 기본 몬스터는 최대 평타 + 스킬 1개를 가지게함 
    public bool isHit = false;
    public bool isGround = true;
    public bool canAtk = true;
    public bool canSkill = true;
    public bool MonsterDir;
    public bool Player=false;

    //메소드 선언
    public Rigidbody2D rb;
    protected CircleCollider2D circleCollider;
    public BoxCollider2D BoxCollider;
    public GameObject healthBar;
    public Animator Anim;
    public LayerMask layerMask;
    public SpriteRenderer spriteRenderer;

    public Vector2 Stop = new Vector2(0, 0);
    public Vector2 ToPlayer;
    public Bullet bullet;

    public State currentState;
    WaitForSeconds Delay100 = new WaitForSeconds(0.1f);
    //Coroutine Idle;

    public enum State{
        Idle,
        Attack,
        Skill,
        Dead,
    }

    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider = GetComponent<BoxCollider2D>();

        //StartCoroutine(ResetCollider());
        currentState = State.Idle;
        StartCoroutine("FSM");
        StartCoroutine("damageColor");
        StartCoroutine(AtCalc(atkCoolTime, skill_Cool));
        //StartCoroutine(SkillCalc(skill_Cool));
    }

    protected void Start() { // Awake 보다 후순위, 
        currentHP = fullHP;
    }


    // Update is called once per frame
    protected virtual void Update() // :base() 상속될 스크립트에 넣기
    {
        if ((PlayerData.Instance.Player.transform.position.x - transform.position.x) < DetectRanX && (PlayerData.Instance.Player.transform.position.y - transform.position.y) < DetectRanY)
        {
            Player = true;
        }
        else Player = false;

        if ((PlayerData.Instance.Player.transform.position.x - transform.position.x) < AtkRanX && (PlayerData.Instance.Player.transform.position.y - transform.position.y) <  AtkRanY)
        {
            if (!canSkill)
            {
                currentState = State.Attack;
            }
            else
                currentState = State.Skill;
        }

    }
    



    protected IEnumerator FSM() {
        for (; ; ) { 
        State CS = currentState;
        yield return new WaitForSeconds(0.1f);
            if (currentState != CS && CS == State.Idle)
            {
                StopCoroutine("Idle");
                StartCoroutine(currentState.ToString());
                Anim.SetTrigger(currentState.ToString());
                yield return new WaitForSeconds(0.3f); //행동 딜레이
            }

        }
    }

    


   //------------------------------------------ 상태별 행동----------------------------------------
    protected virtual IEnumerator Idle()
    {
        //True pl
        yield return null;
    }

    protected virtual IEnumerator Attack()
    {
        canAtk = false;
           yield return null;
    }


    protected virtual IEnumerator Skill()
    {
        canSkill = false;
        yield return null;
    }


    protected IEnumerator Dead()
    {
        Anim.SetTrigger("Dead");
        rb.velocity = Stop;
        yield return null;
        Invoke("die", 4);
    }


    IEnumerator damageColor()
    {
        while (true) {
            if (isHit) { 
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                yield return new WaitForSeconds(0.2f);
                spriteRenderer.color = new Color(1, 1, 1, 1);
                isHit = false;
            }
            yield return null;
        }
    }

    //----------------------------------------상태와 쿨타임 계산-------------------------------------------------
    protected IEnumerator AtCalc(float AtC, float SkC) {
        while (true) {
            yield return null;
            if(!canAtk)
            {
                AtC -= Time.deltaTime;
                if (AtC <= 0)               
                    canAtk = true;
            }

            if (!canSkill)
            {
                SkC -= Time.deltaTime;
                if (SkC <= 0)
                    canSkill = true;
            }
        }
    }



    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("playerBullet"))
        { 
            bullet = collision.GetComponent< Bullet> ();
            TakeDamage(bullet.BulletDam);
            
            if (currentHP <= 0)
            {
                StopAllCoroutines();
                StartCoroutine("Dead");
            }

        }
    }

    //데미지가 들어가는 과정을 bullet에 넣는 것이 나을 것 같음 충돌 > 몬스터면 콜라이더 정보 가져옴 > 몬스터의 체력 감소 >   
    public void TakeDamage(int dam)
        {
            currentHP -= (int)dam;
            isHit = true;

            if (currentHP <= 0)
            {
                StopAllCoroutines();
                spriteRenderer.flipY = true;
                
            }
            rb.AddForce(Vector2.up *5 , ForceMode2D.Impulse);
        }
    public void die()
    {
        gameObject.SetActive(false);
    }
}
