using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Coral : Trap

{
    public Rigidbody2D Player_rb;
    public System.Random rand = new System.Random();
    int X_power;
    void Awake() {
        atkDelay = 0.5f;
        Player_rb = GameObject.Find("Player (Fixed)").GetComponent<Rigidbody2D>();
        hitBoxCollider.SetActive(false);
        StartCoroutine("FSM");
    }



    // Update is called once per frame
    void Update()
    {

    }

    protected override IEnumerator Attack() {
        Anim.SetTrigger("Player");
        yield return new WaitForSeconds(atkDelay);

        if (player == true)
        {   
            hitBoxCollider.SetActive(true);
            Player_rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            yield return new WaitForSeconds(1.5f);
            Player_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            X_power = rand.Next(-10,10);
            Player_rb.AddForce (new Vector2(X_power,1)*10, ForceMode2D.Impulse);
        }

        AttackOver();
        yield return new WaitForSeconds(2f);
        StartCoroutine("Idle");
        yield return null;
    }

}
