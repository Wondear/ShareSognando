using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp2 : FixMonster
{

    // Start is called before the first frame update
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);
    WaitForSeconds Delay1500 = new WaitForSeconds(1.5f);
    WaitForSeconds Delay100 = new WaitForSeconds(0.1f);
    protected override void Awake() {
        base.Awake();
        fullHP = 1;
        atkPower = 2;
        moveSpeed = 0.5f;
        atkCoolTime = 1;
        skill_Cool = 100;
        DetectRanX = 2;
        DetectRanY = 2;
        AtkRanX = 2;
        AtkRanY = 2;
        canSkill = false;
    }

    // Update is called once per frame

    protected override void Update() {
        base.Update();
        ToPlayer= new Vector2((PlayerData.Instance.Player.transform.position.x - transform.position.x),(PlayerData.Instance.Player.transform.position.y - transform.position.y)).normalized * moveSpeed; ;
    }

    protected override IEnumerator Idle()
    {   base.Idle();
        while (!Player)
        {
            rb.velocity = new Vector2(0, transform.localScale.y * 40);
            yield return Delay1500;
            rb.velocity = new Vector2(0, -transform.localScale.y * 30);
            yield return Delay1500;
            rb.velocity = Stop;
            yield return Delay100;
        }
        yield return null;
    }

    protected override IEnumerator Attack()
    {
        base.Attack();
        currentState = State.Idle;
        yield return null;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.transform.CompareTag("Player")) {
            TakeDamage(1);
        }
        base.OnTriggerEnter2D(collision);   
    }

    protected override IEnumerator Skill()
    {
        base.Skill();
        while (Player) { 
        rb.velocity = ToPlayer;
        yield return new WaitForSeconds(0.1f);
        }
        currentState = State.Idle;

    }


}
