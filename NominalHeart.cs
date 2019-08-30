using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NominalHeart : Boss
{

    private bool TouchingPlayer = false;

	private float Speed = .75f;
    public override IEnumerator Intro()
	{
        GameControl.control.PlaySound(Audio, false, Clips[1], false);
		//Audio.Play();
			Invincible = true;
			pos = new Vector3 (0,40,0);

			for (int i = 0; i < 200; i++)
			{
				if (i < 80)
				{
					pos.y -= 0.25f;
				}

				else if (i < 160) 
				{
					pos.y -= .1875f;
					tr.Rotate(new Vector3(0,6,0));
				}

				else
				{
					pos.y -= 0.1f;
					tr.Rotate(new Vector3(0,6,0));
				}

				yield return new WaitForSeconds(0.01f);
			}
			yield return new WaitForSeconds (1f);

			Introdone = true;
			Weapons[1] = Main.ArenaStats.Turrets[0];
			Weapons[2] = Main.ArenaStats.Turrets[1];
			Weapons[3] = Main.ArenaStats.Turrets[2];
			Weapons[4] = Main.ArenaStats.Turrets[3];
			Weapons[5] = Main.ArenaStats.Turrets[4];
			Weapons[6] = Main.ArenaStats.Turrets[5];
			Weapons[7] = Main.ArenaStats.Turrets[6];
			Weapons[8] = Main.ArenaStats.Turrets[7];
			
			Invincible = false;
			lastroutine = HeartPattern();
			StartCoroutine(lastroutine);
	}

    public IEnumerator HeartPattern()
	{
		for (int j = 0; j < 8; j++)
		{
			yield return new WaitForSeconds(.5f / Speed);
			if (j != 7)
			{
				Weapons[0].FireWeapon(1, true, 0, true);

				if(CurrentHP != MaxHP && j % CurrentHP == 0)
					Weapons[(int)(Random.value * 8)+1].FireWeapon(8, true, 0, true);
			}

			else if (j == 7 && !TouchingPlayer)
			{
				if (CurrentHP <= 6)
				{
					switch ((int)(Random.value * 4))
					{
						case 0:
						for (int h = 0; h < 4; h++)
						{
							GameControl.control.PlaySound(Audio, false, Clips[6], false);
							Weapons[0].FireWeapon(1, true, 15, false);
							Weapons[0].FireWeapon(1, true, 0, false);
							Weapons[0].FireWeapon(1, true, -15, false);
							yield return new WaitForSeconds(.5f);

							Weapons[0].FireWeapon(8, true, -7, false, 1);
							Weapons[0].FireWeapon(8, true, 7, false, 1);
							yield return new WaitForSeconds(.5f);
						}
						break;

						case 1:
						for (int h = 0; h < 4; h++)
						{
							GameControl.control.PlaySound(Audio, true, Clips[5], false);
							Weapons[1].FireWeapon(1, true, 0, false);
							Weapons[2].FireWeapon(1, true, 0, false);
							Weapons[3].FireWeapon(1, true, 0, false);
							Weapons[4].FireWeapon(1, true, 0, false);
							Weapons[5].FireWeapon(1, true, 0, false);
							Weapons[6].FireWeapon(1, true, 0, false);
							Weapons[7].FireWeapon(1, true, 0, false);
							Weapons[8].FireWeapon(1, true, 0, false);
							yield return new WaitForSeconds(.25f);
						}
						break;
							
						case 2:
						GameControl.control.PlaySound(Audio, true, Clips[4], false);
						for (int h = 0; h < 15; h++)
						{
							Weapons[0].FireWeapon(8, true, -70 + h*10, false, 1);
							yield return new WaitForSeconds(.075f);
						}
						break;

						case 3:
						float spread = 0;
						for (int h = 0; h < 64; h++)
						{
							spread = Random.value * 90 - 45;
							Weapons[0].FireWeapon(1, true, spread, true);
							yield return new WaitForSeconds(.0625f);
						}
						break;
					}
				}
				if (CurrentHP == 1 && Variancecounter < 3)
					Variancecounter++;
				else
				{
					Weapons[0].FireWeapon(3, true, 0, true);
					Variancecounter = 0;
				}
					
			}
		}
		//StoredCoroutine = HeartPattern();
		lastroutine = HeartPattern();
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
				
			//Main.PlayLoop = new int[16,16];

			yield return new WaitForSeconds(2f);
			Invincible = false;
			Main.SegmentFlag = true;

			Destroy(gameObject);
			yield break;
		}

		else
		{
            Main.SegmentFlag = true;

			if (CurrentHP == 6)
				Speed = 1f;
			else if (CurrentHP == 4)
				Speed = 1.25f;
			else if (CurrentHP == 2)
				Speed = 1.5f;

            GameControl.control.PlaySound(Audio, false, Clips[2], false);
			GetComponent<MeshRenderer>().material.color = new Color(.25f,.25f,.25f,.25f);
			GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor",Color.black);

			yield return new WaitForSeconds(2f);
			Invincible = false;
			float percleft = (float)CurrentHP/MaxHP;
			GetComponent<MeshRenderer>().material.color = new Color (percleft/2, percleft, 1);
			GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor",new Color (percleft/2, percleft, 1));

			lastroutine = HeartPattern();
			StartCoroutine(lastroutine);
		}
    }


    void LateUpdate()
    {
        transform.position = pos;
			if (Introdone && CurrentHP > 0 && Main.Player.CurrentHP > 0 && !TouchingPlayer)
			{
				tr.rotation = Quaternion.LookRotation((Main.Player.pos - transform.position).normalized);
				transform.Rotate(0,-90,0);
				pos += transform.right * Time.deltaTime * Speed;
			}
			else if (Introdone && CurrentHP > 0 && Main.Player.CurrentHP > 0)
			{
				tr.rotation = Quaternion.LookRotation((Main.Player.pos - transform.position).normalized);
				transform.Rotate(0,-90,0);
				pos -= transform.right * Time.deltaTime * Speed;
			}
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
		{
			if(!Main.Player.Invincible)
				Main.Player.UpdateHealth(-1);
		}

		else if (other.gameObject.tag == "Bullet")
		{
			//Alternatively, ask for TypeIdentifier 3
			if (other.gameObject.name.Contains("Jester") && !Invincible)
				StartCoroutine(TakeDamage());
			other.gameObject.SetActive(false);
		}

		else if (other.gameObject.tag == "Boss" && type == "RevengeHeart2")
		{
			TouchingPlayer = true;
		}
    }
    
    void OnTriggerStay (Collider other)
	{
		//Debug.Log("Stop Touching the boss");
		if (other.gameObject.tag == "Player")
		{
			if(!Main.Player.Invincible)
				Main.Player.UpdateHealth(-1);
			TouchingPlayer = true;
		}

		else if (other.gameObject.tag == "Boss" && type == "RevengeHeart2")
		{
			TouchingPlayer = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player")
			TouchingPlayer = false;

		else if (other.gameObject.tag == "Boss" && type == "RevengeHeart2")
		{
			StartCoroutine(RevengeWait());
		}
	}

    private IEnumerator RevengeWait()
	{
		yield return new WaitForSeconds(.5f);
		TouchingPlayer = false;
	}

}
