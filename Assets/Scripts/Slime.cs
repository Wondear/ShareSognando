using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FixMonster
{
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    Vector2 boxColliderOffset;
    Vector2 boxColliderJumpOffset;
    public float runTime, deltime;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 3f;
        jumpPower = 15f;
        currentHP = 6;
        atkCoolTime = 0.5f;
        fullHP = 6;
        atkPower = 1;
        skill_Cool = 3;
        DetectRan = 10f;
        AtkRan = 5;
        haveSkill = true;
        //boxColliderOffset = boxCollider.offset;
        //boxColliderJumpOffset = new Vector2(Moveoffset.transform.position.x, 1f);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        GroundCheck();
        if (!isHit && isGround)
        {
            //boxCollider.offset = boxColliderOffset;

        }

        if (currentState == State.Idle)
        {
            deltime += Time.deltaTime;
        }
    }

    protected override IEnumerator Idle()
    {
        while (!Player) {
            yield return Delay500;
            //currentState = State.Idle;
            runTime = GetLan(2f, 4f);
            bool rundir = GetLan(); //true == another,false == same, 
            if (rundir==true) {
                MonsterFlip();
            }
           
            deltime = 0f;
            
            while (runTime >= deltime & currentState== State.Idle)
            {
                rb.velocity = new Vector2((-transform.localScale.x * moveSpeed), rb.velocity.y);

                if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask)) {
                    MonsterFlip();
                
                }
               
            }
           
        }
    } 

    protected override IEnumerator Attack()
    {
            if (isGround && canAtk)
            {
            while (canAtk&&Player) { 
                timer = 0;
                canAtk = ToggleBool(canAtk);
                rb.velocity = new Vector2(-transform.localScale.x * 10f, jumpPower / 1.25f);
            yield return Delay500;
            currentState = State.Idle;
            }
        }
        yield return null;
    }

    protected override IEnumerator Skill()
    {
        if (isGround && canSkill) {
            while (canSkill) { 
                sktimer = 0;
                canSkill = ToggleBool(canSkill);
                rb.velocity = new Vector2(-transform.localScale.x * 6f, jumpPower / 1.3f);
            yield return Delay500;
            currentState = State.Idle;

            yield return null;
            }
            
        }
    }

}
