using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : FixMonster   
{
    float runTime=0, deltime = 0; 
    
    //토하는 공격?을 날릴 예정, 후공몹 > 피가 떨어지면 공격 시작
    public new enum State{
        Idle,
        AttackIdle,// move
        Walk,
        MeleeAttack,
        RangeAttack
    };

    public CapsuleCollider2D capsuleCollider;
    WaitForSeconds Delay = new WaitForSeconds(0.2f);
    Vector2 boxColliderOffset;
    Vector2 boxColliderJumpOffset;
    public GameObject Vomit;

    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 1f;
        atkPower = 2;
        jumpPower = 0f;
        fullHP = 5;
        atkCoolTime = 0.4f;
        skill_Cool = 3;
        DetectRan = 0f;//10f;
        AtkRan = 0; //5f;

        atkCoolTime = 3f;
        haveSkill = true;
        //atkCoolTimeCalc = atkCoolTime;
        //StartCoroutine(FSM());
        
    }

    /*IEnumerator Idle(){
      yield return Delay;
       int vomitPercentage = Random.Range(0, 5);
       Debug.Log(vomitPercentage);
       if(vomitPercentage == 3){
           //currentState = State.RangeAttack;
       }
       else{
           //currentState = State.Walk;
       }
       base.Idle();
       Corouting = ToggleBool(Corouting);
       yield return null;
   }*/


    protected override IEnumerator Idle(){
        base.Idle();

            yield return null;
        }


    protected override IEnumerator Move()
    {
        base.Move();


    yield return null;
    runTime = GetLan(2f, 4f);
    bool rundir = GetLan(0.2f);

    if (rundir == true)
    {
        MonsterFlip();
    }

        while (runTime >= deltime)
        {
            rb.velocity = new Vector2((-transform.localScale.x * moveSpeed), rb.velocity.y);

            if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
                MonsterFlip();

            if (currentHP < fullHP)
            {
                break;
                /*if(IsPlayerDir() && isGround && canAtk){
                    if(Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 5f && canAtk == true){
                        currentState = State.AttackIdle;
                        canAtk = false;
                        break;
                    }
                }*/
            }
        }
            //currentState = State.Idle;


            /* 어택 move
            if(canAtk == true){
                if(Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) > 1.7f){
                    //currentState = State.RangeAttack;
                }
                else{
                    //currentState = State.MeleeAttack;
                }
            }
            if(!Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask) && Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 7f){

                rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);
            }*/

        Corouting = ToggleBool(Corouting);

        yield return null;
    }

    IEnumerator RangeAttack(){
        Debug.Log("Vomit");
        yield return new WaitForSeconds(0.3f);
        Vomit.GetComponent<BezierShooter>().Shot();
        //currentState = State.Idle;
        yield return null;
    }
    IEnumerator MeleeAttack(){
        yield return null;
    }

    protected override void Update()
    {
        base.Update();
        if (runTime > deltime)
        {
            deltime += Time.deltaTime;
        }

    }
    /*if(!isHit && isGround && !IsPlayingAnim("Walk")){
        MyAnimSetTrigger("Idle");
    }*/

}
