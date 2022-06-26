using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterForm : FixMonster
{
    // ���� ��ũ��Ʈ �ۼ� ���

    protected override void Awake()
    {
        base.Awake();
        fullHP = 1;
        atkPower = 1;
        moveSpeed = 1;
        atkCoolTime = 1;
        skill_Cool = 1;
        DetectRanX = 1;
        DetectRanY = 1;
        AtkRanX = 1;
        AtkRanY = 1;

        //StartCoroutine(SkillCalc(skill_Cool)); //��ų �ִ� �ֵ鸸 Ȱ��ȭ

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
