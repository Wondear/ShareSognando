using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterForm : FixMonster
{
    // 몬스터 스크립트 작성 양식

    protected override void Awake()
    {
        base.Awake();
        fullHP = 1;
        atkPower = 1;
        moveSpeed = 1;
        atkCoolTime = 1;
        skill_Cool = 1;
        DetectRan = 1;
        AtkRan = 1;
   }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    protected override IEnumerator Idle()
    {
        base.Idle();
        yield return null;
    }

    protected override IEnumerator Attack()
    {
        base.Attack();
        yield return null;
    }

    protected override IEnumerator Skill()
    {
        base.Skill();
        yield return null;
    }
}
