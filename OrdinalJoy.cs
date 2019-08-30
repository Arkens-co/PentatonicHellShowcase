using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdinalJoy : Boss
{

   	public int[] UsedAttacks = new int[8];
	public int count;
    public int Randattack;
	public bool returning = true;

	public Texture[] background = new Texture[2];
	public MeshRenderer MainBackground;

	public JoyFace MyFace;


    public override IEnumerator Intro ()
    {
         GameControl.control.PlaySound(Audio, false, Audio.clip, false);
		
        pos = new Vector3 (0,1,0);
		CentralRotation.transform.position = pos;
		Main.ArenaStats.DefendGoals[0].GetComponent<Animator>().Play("Start");
            
		yield return new WaitForSeconds (8f);

        Body.Play("JoyIntro");
		for (int j = 0; j < 8; j++)
		{
            UsedAttacks[j] = 0;
			Weapons[4+j] = Main.ArenaStats.Turrets[j];
		}
        JoyRandomize();
          
        Introdone = true;
        yield return new WaitForSeconds(6f);
        StartCoroutine(JoyPattern());
	}


    public IEnumerator JoyPattern()
	{
		yield return new WaitForSeconds(0.5f);

		//yield return new WaitUntil(() => Main.Currentshot.x == 15 && Main.Currentshot.y == 15);

		switch (Randattack)
		{
		case 0:

			    //Main.Fields[10] = true;
			    Debug.Log("Attack 1: Let's Go");

                GameControl.control.PlaySound(Audio, false, Clips[9], false);
			    for(int a = 0; a < 10; a++)
			    {

                    Weapons[1].FireWeapon(4, false, ConstantAngle - 30 + a * 5, false);
                    Weapons[1].FireWeapon(4, false, ConstantAngle + 30 - a * 5, false);
                    Weapons[2].FireWeapon(4, false, ConstantAngle - 30 + a * 5, false);
                    Weapons[2].FireWeapon(4, false, ConstantAngle + 30 - a * 5, false);
					GameControl.control.PlaySound(Audio, false, Clips[11], false);
                    yield return new WaitForSeconds (0.1f);
			    }

                Body.SetFloat("Speed", UsedAttacks[0]);
                Body.Play("Joycharge");
			    returning = false;
			    yield return new WaitForSeconds(1.5f/ UsedAttacks[0]);
                if (UsedAttacks[0] > 1)
                {
                    GameControl.control.PlaySound(Audio, false, Clips[9], false);
                    returning = true;
                    yield return new WaitForSeconds(0.75f);
                    returning = false;
                    Weapons[1].FireWeapon(3, false, ConstantAngle - 15, false);
                    Weapons[1].FireWeapon(3, false, ConstantAngle + 15, false);
                    Weapons[2].FireWeapon(3, false, ConstantAngle - 15, false);
                    Weapons[2].FireWeapon(3, false, ConstantAngle + 15, false);
					GameControl.control.PlaySound(Audio, false, Clips[11], false);
                    Body.Play("Joycharge");
                }
                yield return new WaitForSeconds (1f);
                returning = true;
			    break;

		case 1:

			    Debug.Log("Attack 2: Jester Jump");

                GameControl.control.PlaySound(Audio, false, Clips[1], false);
                Body.Play("JoyHop");
                Body.SetFloat("Speed", 1f);
                Variancecounter = 0;

		    	returning = false;
                for (int h = 0; h < 4; h++)
                {
                    int randomshot = (int)(Random.value * 8);
                    Weapons[4+randomshot].FireWeapon(3, true, 0, true, 0.5f);
                    yield return new WaitForSeconds(0.5f);
                }
			    yield return new WaitForSeconds(1f);
			    returning = true;
			
			    break;

		case 2:
			    Debug.Log("Attack 3: Slow Labyrinth");
			    //Perpendicular bullets
			    Main.Fields[10] = true;
                GameControl.control.PlaySound(Audio, false, Clips[10], false);
			    for (int a = 0; a < 2 * UsedAttacks[2]; a++)
			    {
				    for (int z = 0; z < 16; z++)
				    {
						float arenaangle = Main.ArenaStats.DefendGoals[0].gameObject.transform.rotation.eulerAngles.y;
                        if (z % 4 == 0)
                            Weapons[z%3].FireWeapon(9, false, 0, false, 0.5f);
                        int randomshot = (int)(Random.value  * 8);
                        Weapons[4 + randomshot].FireWeapon(2, false, arenaangle, false);
                    yield return new WaitForSeconds (.125f / UsedAttacks[2]);
                    }
                }

			    Main.Fields[10] = false;

			    break;

		case 3:

			    Debug.Log("Attack 4: Stealth Flip");
                
				GameControl.control.PlaySound(Audio, false, Clips[13], false);
				yield return new WaitForSeconds(1f);
				Body.Play("JoyFlip");
				int temp = 2 * UsedAttacks[3];
			    for (int a = 0; a < 2; a++)
			    {
					returning = false;
                    for (int k = 0; k < 12; k++)
				    {
						
						GameControl.control.PlaySound(Audio, false, Clips[13], false);
						yield return new WaitForSeconds (0.1f);
					    Weapons[0].FireWeapon(7, true, -1, false, temp);
					    Weapons[0].FireWeapon(7, true, 3, false, temp);
                        Weapons[3].FireWeapon(7, true, -3, false, temp);
                        Weapons[3].FireWeapon(7, true, 1, false, temp);
                        
				    }
					returning = true;
					yield return new WaitForSeconds (0.2f);
			    }
			    returning = true;
                break;

		case 4:

			    Debug.Log("Attack 5: Death Minigun");
                Body.SetFloat("Speed", 1);
                Body.Play("JoySwipe");
                Variancecounter = 15;
                returning = false;
			    for (int a = 0; a < UsedAttacks[4]; a++)
			    {
                    for (int k = 0; k < 8; k++)
				    {
						GameControl.control.PlaySound(Audio, false, Clips[15], false);
					    //transform.Rotate(0,0,15);
					    Weapons[0].FireWeapon(5, true, 0, false, 0.9f);
					    yield return new WaitForSeconds (0.25f / UsedAttacks[4]);
				    }
			    }
			    returning = true;
			    break;

		case 5:

			    Debug.Log("Attack 6: Jester Barrage");
				returning = false;
				GameControl.control.PlaySound(Audio, false, Clips[12], false);
                Body.SetFloat("Speed", 1);
                Body.Play("JoyCrash");
				Variancecounter = 5;
				yield return new WaitForSeconds(8f);
				returning = true;
			    break;

		case 6:

			Debug.Log("Attack 7: Red Wedding");

			Main.Fields[10] = true;
			returning = false;
            Body.SetFloat("Speed", 1);
            Body.Play("JoyRotate");

            for (int a = 0; a < 12 * UsedAttacks[6]; a++)
			{
				float arenaangle = Main.ArenaStats.DefendGoals[0].gameObject.transform.rotation.eulerAngles.y;

				GameControl.control.PlaySound(Audio, false, Clips[11], false);

				if (a % 2 == 0)
				{
					Weapons[11].FireWeapon(11, false, arenaangle, false, 6f);
					Weapons[9].FireWeapon(11, false, arenaangle, false, 6f);
					Weapons[7].FireWeapon(11, false, arenaangle, false, 6f);
					Weapons[5].FireWeapon(11, false, arenaangle, false, 6f);
				}
				else
				{
					Weapons[10].FireWeapon(11, false, arenaangle, false, 2f);
					Weapons[8].FireWeapon(11, false, arenaangle, false, 2f);
					Weapons[6].FireWeapon(11, false, arenaangle, false, 2f);
					Weapons[4].FireWeapon(11, false, arenaangle, false, 2f);
				}

				yield return new WaitForSeconds (0.25f / UsedAttacks[6]);
			}
			returning = true;
			Main.Fields[10] = false;

			break;

		case 7:

    			Debug.Log("Attack 8: Hop of DEATH");

                GameControl.control.PlaySound(Audio, false, Clips[2], false);
                returning = false;
				Main.Fields[10] = true;
	    		yield return new WaitForSeconds(0.5f);
        
                Body.SetFloat("Speed", 1);
                Variancecounter = 10;
                Body.Play("JoyHop");
                for (int d = 0; d < 4; d++)
                {
					float arenaangle = Main.ArenaStats.DefendGoals[0].gameObject.transform.rotation.eulerAngles.y;
					Weapons[d*2+4].FireWeapon(4, false, arenaangle, true);
					Weapons[11-d*2].FireWeapon(4, false, arenaangle, false);

                    yield return new WaitForSeconds(0.8f);
                }

                returning = true;
				Main.Fields[10] = false;

			break;
		}

		StartCoroutine(TakeDamage());
	}

    new public IEnumerator TakeDamage()
    {
        CurrentHP--;
		Invincible = true;
        //Main.PlayLoop = new int[16,16];

		if (CurrentHP <= 0)
		{
            GameControl.control.PlaySound(Audio, false, Clips[0], false);

			Explosion.SetActive(true);

			yield return new WaitForSeconds(2f);
			Invincible = false;
			Main.SegmentFlag = true;

			Destroy(gameObject);
			yield break;
		}		

		else
		{
			//Phase 2
			if (CurrentHP == 8)
			{
				//Turn on a Reverse Field
				GameControl.control.PlaySound(Audio, false, Clips[12], false);
				Main.Fields[1] = true;
				Main.ArenaStats.Floor.material.mainTexture = Main.Levelstats.SwitchScreens[0];
			    //MainBackground.material.mainTexture = background[1];
			    yield return new WaitForSeconds (2f);
			}
            Main.SegmentFlag = true;
            JoyRandomize();
            GameControl.control.PlaySound(Audio, false, Clips[2 + Randattack], false);	

			yield return new WaitForSeconds (1f);
			Invincible = false;
			StartCoroutine(JoyPattern());
		}
    }

    void LateUpdate()
    {
        if (Introdone)
		{
            ConstantAngle = CentralRotation.transform.localRotation.eulerAngles.y - 90;
			if (returning)
			{
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (CentralRotation.transform.position != Vector3.up)
				{
					Debug.Log("Not in center right now");
					if ((CentralRotation.transform.position.x < 0.25 && CentralRotation.transform.position.x > -0.25 ) 
                        && (CentralRotation.transform.position.z < 0.25 && CentralRotation.transform.position.z > -0.25))
    					{
                        CentralRotation.transform.position = Vector3.up;
	    				}							
					else
					{
			    		CentralRotation.transform.rotation = Quaternion.LookRotation((new Vector3(0, 1, 0) - CentralRotation.transform.position).normalized);
						CentralRotation.transform.Rotate(-90,0,0);
						CentralRotation.transform.position = Vector3.MoveTowards(CentralRotation.transform.position, new Vector3(0,1,0), 10 * Time.deltaTime);
					}
							
				}
				else if (CentralRotation.transform.position == Vector3.up)
				{
					//Debug.Log("Look at me!");
					CentralRotation.transform.rotation = Quaternion.LookRotation(Main.Player.pos - CentralRotation.transform.position);
                    CentralRotation.transform.Rotate(-90,0,0);
                    
					//rgd.MoveRotation(Quaternion.Euler(90,0,90));
				}
			}
			else
			{
                //CentralRotation.transform.position = new Vector3
				//	(
				//		Mathf.Clamp (CentralRotation.transform.position.x, Main.Player.boundary.xMin, Main.Player.boundary.xMax), 
				//		Mathf.Clamp (CentralRotation.transform.position.y, 1, 1), 
				//		Mathf.Clamp (CentralRotation.transform.position.z, Main.Player.boundary.zMin, Main.Player.boundary.zMax)
				//	);
			}
		}
	}


    private void JoyRandomize()
    {
        //Main.PlayLoop = new int[,]
        Randattack = (int)(Random.value * 8);

       // Randattack = 6;

        if (UsedAttacks[Randattack] >= 2)
        {
            count++;
            Debug.Log("Attack isn't usable Ordinal Joy, try again for the " + count + " time");

            JoyRandomize();
        }
        else
        {
            UsedAttacks[Randattack]++;
            //Debug.Log("Usedattacks at " + Randattack + " Equals " + UsedAttacks[Randattack]);
        }
    }
    
    //Used for several different animation events
    public void TargetingPlayer (int i)
    {
        if (i == 1 && Variancecounter == 10)
            CentralRotation.transform.position = Main.Player.pos;
      

        else if (i == 2)
        {
            CentralRotation.transform.rotation = Quaternion.LookRotation(Main.Player.pos - CentralRotation.transform.position);
            CentralRotation.transform.Rotate(-90, 0, 0);
        }

        else if (i == 3 && Variancecounter == 0)
        {
            Weapons[0].FireWeapon(3, false, 15, false);
            Weapons[0].FireWeapon(3, false, -15, false);
            Weapons[1].FireWeapon(3, false, 15, false);
            Weapons[1].FireWeapon(3, false, -15 , false);
            Weapons[2].FireWeapon(3, false, 15, false);
            Weapons[2].FireWeapon(3, false, -15, false);
            Weapons[3].FireWeapon(3, false, 15, false);
            Weapons[3].FireWeapon(3, false, -15, false);
			GameControl.control.PlaySound(Audio, false, Clips[11], false);
        }

		else if (i == 3 && Variancecounter == 10)
		{
			float arenaangle = Main.ArenaStats.DefendGoals[0].gameObject.transform.rotation.eulerAngles.y;
			Weapons[4].FireWeapon(4, false, arenaangle, true);
            Weapons[6].FireWeapon(4, false, arenaangle, true);
            Weapons[8].FireWeapon(4, false, arenaangle, true);
            Weapons[10].FireWeapon(4, false, arenaangle, true);
			//GameControl.control.PlaySound(Audio, false, Clips[11], false);
		}

		else if (i == 4)
		{
			GameControl.control.PlaySound(Audio, false, Clips[17], false);
			if (UsedAttacks[5] > 1)
				Weapons[0].FireWeapon(1, true, 0, false);
		}
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if(!Main.Player.Invincible)
				Main.Player.UpdateHealth(-1);
		}
	}

}
