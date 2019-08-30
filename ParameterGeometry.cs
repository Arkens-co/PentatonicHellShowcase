using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterGeometry : Boss
{
    int lastattack = 0;
    float movespeed = 0.5f;
    
    public override IEnumerator Intro ()
    {
        GameControl.control.PlaySound(Audio, false, Clips[1], false);
		
        Body.Play("Intro");
        yield return new WaitForSeconds (0.1f);
		  
        Introdone = true;
        lastroutine = GeoPattern();
        StartCoroutine(lastroutine);
	}

    public IEnumerator GeoPattern()
    {
        yield return new WaitForSeconds (2f);
        movespeed = 0.5f;
        charging = false;
        int attack = (int)(Random.value * 4);
        if (lastattack == attack)
            attack = 0;
        else
            lastattack = attack;

        switch (attack)
        {
            case 0:
            if(charging)
                break;
            Debug.Log("Charge Attack");
            Body.Play("Charge");
            GameControl.control.PlaySound(Audio, false, Clips[3]);
            yield return new WaitForSeconds (1f);
            charging = true;
            movespeed = 4f + (2 / CurrentHP);
            yield return new WaitForSeconds (1f);
            break;

            case 1:
            Debug.Log("Projectiles!");
            GameControl.control.PlaySound(Weapons[0].Toneplayer, true, Clips[2]);
            for (int f = 0; f < 13; f++)
            {
                Weapons[0].FireWeapon(1, true, 70 - (f * 10f), false, 1);
            }
            yield return new WaitForSeconds (1.5f);
            break;

            case 2:
            Debug.Log("Different Projectiles");
            for (int f = 0; f < 3; f++)
            {
                Weapons[0].FireWeapon(1, true, 30, false, 1);
                Weapons[0].FireWeapon(1, true, 10, false, 1);
                Weapons[0].FireWeapon(1, true, 0, false, 1);
                Weapons[0].FireWeapon(1, true, -10, false, 1);
                Weapons[0].FireWeapon(1, true, -30, false, 1);
                GameControl.control.PlaySound(Weapons[0].Toneplayer, true, Clips[2]);
                yield return new WaitForSeconds (0.5f);
            }
            break;

            case 3:
            Debug.Log("Third Projectiles");
            if (CurrentHP < 3)
            {
                GameControl.control.PlaySound(Weapons[0].Toneplayer, true, Clips[1]);
                 for (int f = 0; f < 8; f++)
                {
                    Weapons[0].FireWeapon(1, true, 10 + 10 * f, false, 1);
                    Weapons[0].FireWeapon(1, true, 5 + 5 * f , false, 1);
                    Weapons[0].FireWeapon(1, true, 3 + 3 * f, false, 1);
                    Weapons[0].FireWeapon(1, true, 0, false, 1);
                    Weapons[0].FireWeapon(1, true, -3 - 3 * f, false, 1);
                    Weapons[0].FireWeapon(1, true, -5  -5 * f, false, 1);
                    Weapons[0].FireWeapon(1, true, -10 -10 * f, false, 1);
                    yield return new WaitForSeconds (0.2f);
                }
            }
            else
            {
                if(charging)
                break;
                Debug.Log("Charge Attack");
                Body.Play("Charge");
                GameControl.control.PlaySound(Audio, false, Clips[3]);
                yield return new WaitForSeconds (1f);
                charging = true;
                movespeed = 4f + (2 / CurrentHP);
                yield return new WaitForSeconds (1f);
            }

            break;
        }
        lastroutine = GeoPattern();
        StartCoroutine(lastroutine);
    }

    new public IEnumerator TakeDamage()
    {
        CurrentHP--;
		Invincible = true;  
        StopCoroutine(lastroutine);

        if (CurrentHP <= 0)
		{
            GameControl.control.PlaySound(Audio, false, Clips[0], false);
			Explosion.SetActive(true);
            Body.Play("Death");
			yield return new WaitForSeconds(4f);
			Invincible = false;
			Main.SegmentFlag = true;

			Destroy(gameObject);
			yield break;
		}
		
		else
		{
            Main.SegmentFlag = true;
            GameControl.control.PlaySound(Audio, false, Clips[1], false);

			yield return new WaitForSeconds(2f);
            charging = false;
            movespeed = 1f;
			Invincible = false;
			
			StartCoroutine(lastroutine);
		}
    }

    void LateUpdate()
    {
        transform.position = pos;
		Vector3 temp = Main.Player.pos;
		//temp.y++;
		if (!charging)
		{
			tr.rotation = Quaternion.LookRotation((temp - transform.position).normalized);
			//transform.Rotate(90,0,-90);
		}
 		pos += transform.forward * Time.deltaTime * movespeed;
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if(!Main.Player.Invincible)
				Main.Player.UpdateHealth(-1);
		}
        else if (other.gameObject.name == "Pillar")
        {
            Body.Play("Stunned");
            movespeed = -1f;
            if (charging)
            {
                other.gameObject.SetActive(false);
                StartCoroutine(TakeDamage());
            }
        }

        else if (other.gameObject.tag == "Wall")
        {
            Body.Play("Stunned");
            charging = false;
            movespeed = 0;
        }
	}

}
