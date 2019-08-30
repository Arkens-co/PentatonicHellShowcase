using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpentMonic : Boss
{
    public HarpentArm[] Minions;
    //public float ConstantAngle = 0;
    private int SegmentHP = 8;
    private int SegmentPhase = 0;
    //private int DamageMultiplier = 1;

    public GameObject TurretExplosions;
    public GameObject[] TurretBoxes = new GameObject[8];

    //public int attacktype = 0;

    public override IEnumerator Intro ()
    {	
        ConstantAngle = 0;
        Body.Play("Intro");
        gameObject.transform.localPosition = pos;
        yield return new WaitForSeconds (5f);
		  
        Introdone = true;

        for (int i = 0; i < 8; i++)
        {
            Main.PlayLoop[1,i] = 7;
            TurretBoxes[i] = Main.ArenaStats.DefendGoals[i].gameObject;
        }
	}

    public void Reset()
    {
        transform.rotation = Quaternion.identity;
    }

    public IEnumerator MonicAttack (int type)
    {
        //lastroutine = this;
        if (type == 1)
        {
            //type = 4;
            type = (int)(Random.value * 8) + 1;
        }
        switch(type)
        {
            
            //Intro Roar
            case 0:
            GameControl.control.PlaySound(Audio, false, Clips[0], false);
            for (int x = 0; x < 20; x++)
            {
                foreach (var d in Weapons)
                {
                    d.FireWeapon(1, false, 5+x*2, false, 2);
                    d.FireWeapon(1, false, 355-x*2, false, 2);
                }
                yield return new WaitForSeconds(0.1f);
            }
            break;
            //
            case 1: 
            //Debug.Log ("Explosive Nightmare");
            Body.Play("Explosive Nightmare");
            for (int x = 0; x < 8; x++)
            {
                GameControl.control.PlaySound(Audio, false, Clips[1], false);

                Weapons[0].FireWeapon(9, false, 0, false, 1);
                Weapons[3].FireWeapon(9, false, 0, false, 1);
                Weapons[4].FireWeapon(9, false, 0, false, 1);
                Weapons[7].FireWeapon(9, false, 0, false, 1);
                yield return new WaitForSeconds(0.2f);

                GameControl.control.PlaySound(Audio, false, Clips[2], false);
                Minions[0].weapon.FireWeapon(9, true, 0, false, 0.5f);
                Minions[1].weapon.FireWeapon(9, true, 0, false, 0.5f);
                Minions[2].weapon.FireWeapon(9, true, 0, false, 0.5f);
                Minions[3].weapon.FireWeapon(9, true, 0, false, 0.5f);
                yield return new WaitForSeconds(0.2f);
            }

                yield return new WaitForSeconds (0.5f);
                GameControl.control.PlaySound(Audio, false, Clips[3], false);

                Minions[0].weapon.FireWeapon(9, true, 0, false, 1.5f);
                Minions[1].weapon.FireWeapon(9, true, 0, false, 1.5f);
                Minions[2].weapon.FireWeapon(9, true, 0, false, 1.5f);
                Minions[3].weapon.FireWeapon(9, true, 0, false, 1.5f);

                Weapons[0].FireWeapon(9, false, 0, false, 1);
                Weapons[3].FireWeapon(9, false, 0, false, 1);
                Weapons[4].FireWeapon(9, false, 0, false, 1);
                Weapons[7].FireWeapon(9, false, 0, false, 1);
            break;

            case 2:
            //Debug.Log ("Death Barrage");
            Body.Play("Death Barrage");
            for (int x = 0; x < 4; x++)
            {
                GameControl.control.PlaySound(Audio, false, Clips[6], false);
                for (int d = 0; d < 12; d++)
                {
                    int temp = (int)(Random.value * 8);
                    Weapons[temp].FireWeapon(5, true, 0, false, 0.8f);
                    yield return new WaitForSeconds(0.1f);
                }
                StartCoroutine(ArmAttack((int)(Random.value * 4)+1, x));
                yield return new WaitForSeconds(0.5f);
            }
            break;

            case 3:
            //Debug.Log ("Grinder Splat");
            Body.Play("Grinder Splat");
            for (int x = 0; x < 5; x++)
            {
                GameControl.control.PlaySound(Audio, false, Clips[7], false);
                Weapons[7].FireWeapon(6, true, 180, false, 0.9f);
                yield return new WaitForSeconds(0.4f);
            }
            for (int x = 0; x < 5; x++)
            {
                GameControl.control.PlaySound(Audio, false, Clips[7], false);
                GameControl.control.PlaySound(Audio, false, Clips[7], false);
                Weapons[0].FireWeapon(6, true, 0, false, 1.2f);
                Weapons[1].FireWeapon(6, true, 30, false, 1.2f);
                Weapons[2].FireWeapon(6, true, -30, false, 1.2f);
                yield return new WaitForSeconds(0.2f);
            }
            transform.rotation = Quaternion.identity;
            break;

            case 4:
            //Debug.Log ("Bomb Toss");
            Body.Play("Bomb Toss");
            yield return new WaitForSeconds(.1f);
            for (int z = 0; z < 7; z++)
            {
                
                GameControl.control.PlaySound(Audio, false, Clips[7], false);
                Minions[0].weapon.FireWeapon(6, false, 0, false);
                Minions[1].weapon.FireWeapon(5, true, 0, false);
                Minions[3].weapon.FireWeapon(6, false, 0, false);
                
                yield return new WaitForSeconds(.4f);
            }
            GameControl.control.PlaySound(Audio, false, Clips[11], false);
            yield return new WaitForSeconds(.5f);
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Minions[2].weapon.FireWeapon(1, false, 0 +y*10, false, 1.5f);
                    Minions[2].weapon.FireWeapon(1, false, 90+y*10, false, 1.5f);
                    Minions[2].weapon.FireWeapon(1, false, 180+y*10, false, 1.5f);
                    Minions[2].weapon.FireWeapon(1, false, 270+y*10, false, 1.5f);
                }
                yield return new WaitForSeconds(.2f);
            }
            break;

            case 5:
            //Debug.Log ("Directed Roar");
            Body.Play("Directed Roar");
            yield return new WaitForSeconds(1f);
            
            GameControl.control.PlaySound(Audio, false, Clips[0], false);
            for (int x = 0; x < 20; x++)
            {
                float temp = (Random.value * 2) + 1;
                float temp2 = (Random.value * 10);
                for (int d = 0; d < 3; d++)
                {
                    Weapons[d].FireWeapon(1, true, temp2 + 30 - x*2, false, temp);
                    Weapons[d].FireWeapon(1, true, -temp2 + 30 - x*2, false, temp);
                    Weapons[d].FireWeapon(1, true, -temp2 + 30 - x*2, false, temp);
                }
                yield return new WaitForSeconds(0.1f);
            }
            transform.rotation = Quaternion.identity;
            //yield return new WaitForSeconds(1f);
            break;

            case 6:
            //Debug.Log ("Arm Stretch");
            Body.Play("Arm Stretch");
            for (int x = 0; x < 4; x++)
            {
                yield return new WaitForSeconds(0.3f);
                GameControl.control.PlaySound(Audio, false, Clips[5], false);
                Minions[0].weapon.FireWeapon(9, true, 0, false);
                Minions[1].weapon.FireWeapon(9, true, 0, false);
                Minions[2].weapon.FireWeapon(9, true, 0, false);
                Minions[3].weapon.FireWeapon(9, true, 0, false);
            }
            yield return new WaitForSeconds(1f);
            GameControl.control.PlaySound(Audio, false, Clips[4], false);
            for (int y = 0; y < 8; y++)
            {
                foreach(var z in Minions)
                {
                    if(z.weapon != null)
                        z.weapon.FireWeapon(6, true, 20+y*45, false, 1);
                }
            }
            break;

            case 7:
            //Debug.Log ("Hall of Bullets");
            Body.Play("Hall of Bullets");
            for (int x = 0; x < 8; x++)
            {   
                StartCoroutine(ArmAttack((int)(Random.value * 4)+1, 0));
                yield return new WaitForSeconds(0.25f);
                StartCoroutine(ArmAttack((int)(Random.value * 4)+1, 1));
                yield return new WaitForSeconds(0.25f);
                StartCoroutine(ArmAttack((int)(Random.value * 4)+1, 2));
                yield return new WaitForSeconds(0.25f);
                StartCoroutine(ArmAttack((int)(Random.value * 4)+1, 3));
                yield return new WaitForSeconds(0.25f);
            }
            break;

            case 8:
            //Debug.Log ("Curving Dance");
            Body.Play("Curving Dance");

            for (int x = 0; x < 10; x++)
            {
                int temp = x%2;
                GameControl.control.PlaySound(Audio, false, Clips[10], false);
                for (int y = 0; y < 6; y++)
                {
                    Weapons[0+temp].FireWeapon(10, false, -x*2, false, 1);
                    Weapons[2+temp].FireWeapon(10, false, -x*2, false, 1);
                    Weapons[4+temp].FireWeapon(10, false, -x*2, false, 1);
                    Weapons[6+temp].FireWeapon(10, false, -x*2, false, 1);
            
                    Weapons[1-temp].FireWeapon(12, false, x*2, false, 1);
                    Weapons[3-temp].FireWeapon(12, false, x*2, false, 1);
                    Weapons[5-temp].FireWeapon(12, false, x*2, false, 1);
                    Weapons[7-temp].FireWeapon(12, false, x*2, false, 1);
                    yield return new WaitForSeconds(0.1f);
                }
                StartCoroutine(ArmAttack((int)(Random.value * 4)+1, (int)(Random.value * 4)));
            }

            break;
        }

        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator ArmAttack (int type, int minion)
    {
        switch (type)
        {
            case 1:
            GameControl.control.PlaySound(Audio, false, Clips[5], false);
            for (int x = 0; x < 12; x++)
            {
                Minions[minion].weapon.FireWeapon(1, false, 10+x*30, false);
            }
            break;

            case 2:
            GameControl.control.PlaySound(Audio, false, Clips[5], false);
            for (int x = 0; x < 3; x++)
            {
                Minions[minion].weapon.FireWeapon(6, false, 60+x*120, false);
            }
            break;

            case 3:
            GameControl.control.PlaySound(Audio, false, Clips[5], false);
            for (int x = 0; x < 4; x++)
            {
                Minions[minion].weapon.FireWeapon(12, false, 20+x*60, false);
            }
            break;

            case 4:
            GameControl.control.PlaySound(Audio, false, Clips[5], false);
            for (int x = 0; x < 4; x++)
            {
                Minions[minion].weapon.FireWeapon(10, false, 40+x*90, false);
            }
            break;

            case 5:
            GameControl.control.PlaySound(Audio, false, Clips[2], false);
            Minions[minion].weapon.FireWeapon(1, true, 15, false, 2);
            Minions[minion].weapon.FireWeapon(5, true, 64, false, 1);
            Minions[minion].weapon.FireWeapon(6, true, 86, false, 1);
            Minions[minion].weapon.FireWeapon(9, true, 146, false, 1.5f);
            break;

            case 6:
            GameControl.control.PlaySound(Audio, false, Clips[12], false);
            for (int x = 0; x < 8; x++)
            {
                Minions[minion].weapon.FireWeapon(10, false, 40+x*45, false);
            }
            for (int x = 0; x < 8; x++)
            {
                Minions[minion].weapon.FireWeapon(12, false, 40+x*45, false);
            }
            break;
        }

        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator RotateHarpent (string target)
    {
        int f = int.Parse(target.Substring(target.Length - 2));
        Debug.Log(f);
        float Target = 0;
        for (int x = 0; x < f; x++)
        {
            if (target.Contains("Player"))
                Target = Quaternion.LookRotation((Main.Player.pos - CentralRotation.transform.position).normalized).eulerAngles.y;

            transform.rotation = Quaternion.Euler(0,Target,0);  
            //ConstantAngle = CentralRotation.transform.localRotation.eulerAngles.y;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DamageStart()
    {
        StopAllCoroutines();
        //Minions[5].StopAllCoroutines();
        StartCoroutine(TakeDamage());
    }

    new public IEnumerator TakeDamage()
    {
        float temp = Quaternion.LookRotation((Main.ArenaStats.Turrets[SegmentPhase].gameObject.transform.position - CentralRotation.transform.position).normalized).eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, temp, 0);
        Invincible = true;
        Main.PlayLoop[1,SegmentPhase] = 0;
        GameControl.control.PlaySound(Audio, false, Clips[8], false);
        Main.SegmentFlag = true;
        SegmentPhase++;
        SegmentHP = 8;
        Body.SetInteger("Health", 8);
        Body.SetInteger("Segment", SegmentPhase);

        if (CurrentHP <= 0)
            Body.SetTrigger("Death");
        yield return new WaitForSeconds(0.3f);

        

        yield return new WaitForSeconds(1f);
        Main.ArenaStats.Turrets[SegmentPhase-1].gameObject.SetActive(false);
        TurretExplosions.SetActive(true);
        Invincible = false;
        TurretBoxes[SegmentPhase-1].SetActive(false);

        StartCoroutine(ArmAttack(6, 1));
        
        yield return new WaitForSeconds(0.6f);
        Body.SetTrigger("Damaged");
        transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(1);
        TurretExplosions.SetActive(false);
    }

    public IEnumerator Rampage ()
    {
        Invincible = true;
        Body.SetFloat("Speed", 2);
        Main.Fields[18] = true;
        //DamageMultiplier = 2;

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ArmAttack(5, 0));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ArmAttack(5, 1));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ArmAttack(5, 2));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ArmAttack(5, 3));
        yield return new WaitForSeconds (.5f);
        GameControl.control.PlaySound(Audio, false, Clips[11], false);
        yield return new WaitForSeconds (.5f);
        for (int x = 0; x < 8; x++)
        {
            Weapons[x].FireWeapon(5, false, 0, false, 1);
            Weapons[x].FireWeapon(6, false, 2, false, 2);
            Weapons[x].FireWeapon(9, false, -4, false, 1.5f);
            Weapons[x].FireWeapon(6, false, 10, false, 2);
            Weapons[x].FireWeapon(1, false, 5, false, 2);
            Weapons[x].FireWeapon(10, false, -7, false, 1);
            Weapons[x].FireWeapon(12, false, -9, false, 1);
        }
        Invincible = false;
    }

    public IEnumerator Death()
    {
        Invincible = true;
        GameControl.control.PlaySound(Audio, false, Clips[9], false);
		Explosion.SetActive(true);
        yield return new WaitForSeconds(4f);
        Main.Fields[18] = false;

        Main.SegmentFlag = true;

		Destroy(gameObject);
        Invincible = false;
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
            Debug.Log("Monic Crush!");
			if(!Main.Player.Invincible)
				Main.Player.UpdateHealth(-1);
		}

        else if (other.gameObject.tag == "Bullet" && other.gameObject.name.Contains("Stealth") && !Invincible)
        {
			CurrentHP--;
            SegmentHP--;
            Body.SetInteger("Health",SegmentHP);
            UserInterface.UI.UpdateProgress((float)SegmentHP/8);
			other.gameObject.SetActive(false);
            Debug.Log("Monic Hit, HP is " + CurrentHP + SegmentHP);
        }
        
	}
}
