using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp2 : FixMonster
{

    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
        fullHP = 10;
        atkPower = 2;
        moveSpeed = 0.5f;
        skill_Cool = 100;
        DetectRan = 10;
        AtkRan = 10;
        canSkill = false;
        haveSkill = false;

    }

    // Update is called once per frame

    protected override void Update() {
        base.Update();
       

    }

    protected override IEnumerator Idle(){
        base.Idle();

        StartCoroutine("Move");
        yield return null;
    }
    protected override IEnumerator Move()
    {
        while (!Player)
        {

            rb.velocity = new Vector2(0, transform.localScale.y * moveSpeed / 1.5f);
            yield return new WaitForSeconds(1.5f);
            rb.velocity = new Vector2(0, -(transform.localScale.y * moveSpeed) / 1.5f);
            yield return new WaitForSeconds(1.5f);


        }
        yield return null;
        Corouting = ToggleBool(Corouting);
    }

    protected override IEnumerator Attack()
    {
        base.Attack();
        while (Player)
        {
            rb.velocity = ToPlayer;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
        Corouting = ToggleBool(Corouting);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.transform.CompareTag("Player")) {
            TakeDamage(1);
        }
        base.OnTriggerEnter2D(collision);   
    }


}
