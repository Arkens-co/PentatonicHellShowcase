using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpentArm : Minion
{
    public HarpentMonic Controller;

    void MonicAttack(int x)
    {
        StartCoroutine(Controller.MonicAttack(x));
    }

    void RotateHarpent(string x)
    {
        StartCoroutine(Controller.RotateHarpent(x));
    }

    void Death()
    {
        StartCoroutine(Controller.Death());
    }

    void Reset()
    {
        Controller.Reset();
    }

    void Rampage()
    {
        StartCoroutine(Controller.Rampage());
    }

    void DamageStart()
    {
        StopAllCoroutines();
        Controller.DamageStart();
    }
}
