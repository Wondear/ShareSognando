using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : Monster
{
    public enum State // ���� ���, 
    {
        Idle,
        Follow,
        Attack
    };

    bool isdie;
    public State currentState = State.Idle;
    //public Transform[] wallCheck; // �� ����� �� �Ѵٸ� �浹ü�� �Բ� �ʿ� 

    void Awake()
    {
        isdie = false;
        base.Awake();
        moveSpeed = 0.5f;
        jumpPower = 0.2f;
        currentHP = 1;
        fullHP = 1;
        atkPower = 1;
        atkCoolTime = 1f;
        atkCoolTimeCalc = atkCoolTime;
        //StartCoroutine(FSM());

    }

    /*IEnumerator FSM()
    {
        if (isdie == false)
        {
            while (true)
            {
                yield return StartCoroutine(currentState.ToString());
            }
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
