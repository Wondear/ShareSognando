using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FixMonster
{
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    Vector2 boxColliderOffset;
    Vector2 boxColliderJumpOffset;
    public float runTime =0, deltime=0;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 3f;
        jumpPower = 15f;
        currentHP = 6;
        atkCoolTime = 0.4f;
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

        if (runTime>deltime)
        {
            deltime += Time.deltaTime;
        }
    }
    protected override IEnumerator Idle() {
        base.Idle();
        //Anim.speed = 0.5f;
        yield return Delay500;
        StartCoroutine(Move());
        yield return null;
    }

    protected override IEnumerator Move()
    {
        base.Move();

        if (Player) {
            while (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask) || !Player) {
                IsPlayerDir();
                rb.velocity = new Vector2((-transform.localScale.x) *(moveSpeed +0.5f) ,0);
                yield return Delay100;
            }
        }

        else
        {
            runTime = GetLan(2f, 4f);
            bool rundir = GetLan(0.5f); //true == another,false == same, 
            deltime = 0f;
            if (rundir == true)
            {
                MonsterFlip();
            }
            while (runTime >= deltime)
            {
                rb.velocity = new Vector2((-transform.localScale.x * moveSpeed), rb.velocity.y);

                if (Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask))
                    MonsterFlip();

                if (Player)
                    break;

                yield return null;
            }
        }

        Corouting = ToggleBool(Corouting);
    } 

    protected override IEnumerator Attack()
    {
        base.Attack();
        if (isGround && canAtk)
        {
            rb.velocity = new Vector2(-transform.localScale.x * 6f, jumpPower / 1.25f);
            yield return new WaitForSeconds(1.25f);
            timer = 0;
            canAtk = ToggleBool(canAtk);
        }

        yield return new WaitForSeconds(0.2f);

        Corouting = false;
        currentState = State.Idle;

    }

    protected override IEnumerator Skill()
    {
        base.Skill();
        if (isGround && canSkill) {
            yield return Delay500;
            rb.velocity = new Vector2(-transform.localScale.x * 10f, jumpPower / 1.3f);
            yield return new WaitForSeconds(1.3f);
            sktimer = 0;
            canSkill = ToggleBool(canSkill);

        }
        yield return new WaitForSeconds(0.5f);
        Corouting = false;
        currentState = State.Idle;

        yield return null;
    }

}
