using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Equipment
{
	public bool isActive;
	public int EquipSlot;
	protected float timeleft;
}

public class PlayerShip : MonoBehaviour 
{
	public Vector3 pos;
	public float HorInput;
	public float VerInput;
	public float Focus = 1;
	public float Shipspeed = 6;
	public Rigidbody Playrgd;
	//public Boundary boundary;

	public int MaxHP = 12;
	public int CurrentHP;
	public int KeyStrikes = 0;

	public int DecoyHealth;
	public bool Paused;

	public bool CollidingWithWall = false;

	public bool MovingCamera = false;

	public bool Invincible = false;
	public float MercyUpgrade;

	public float[] PowerupCooldowns = new float[8];
	public Equipment[] EquippedPowerups = new Equipment[4];
	public GameObject DecoyHologram;

	//public Image Healthbar;
	//public Image Temphealthbar;

	public AudioSource PlayerAudioSource;
	public AudioClip[] Playerclips;
	//0: Death
	//1: Healing
	//2: Damage
	//3: Shield
	//4: Slowdown
	//5: Speedup
	//6: Freeze
	//7: Pause
	//8: FireShot

	public GameObject Explosion;
	public MeshRenderer mesh;
	public BoxCollider hitbox;

	//private bool zooming = false;
	public Light zoomlight;

	public Gun SonicShooter;

	private Vector3 v;
	private Vector3 targetDir = new Vector3 (0,0,1);

	//public float BiggerMapSize = 1;
	//public float MapHeight = 1;
	public Cameramovement moveCamera;

	private bool freezing;
	private bool frozen;
	//private float timeAtLastShot;
	//private float minTimeBetween2Shots = 1f;
	// Use this for initialization
	void Awake ()
	{
		Invincible = false;
		CurrentHP = MaxHP;
		//Healthbar = Temphealthbar;
		//Heallingsound = Temphealingsound;
	}

	void Start () 
	{
		//Playrgd = GetComponent<Rigidbody>();
		//mesh = GetComponent<MeshRenderer>();
		//
		//pos = transform.position;
		//tr = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
			//Nodes 0
			//Health 1 
			//Mercy 2
			//Evasion 3
			//Decoy 4
			//Speedup 5 
			//Bullet Clear 6
			//Shield 7
			//Reflector 8
			//Slowdown 9 
			//Teleport 10
			//Sonic Shooter 11
		//prevpos = pos;
		//pos = transform.position;

		//Reverse Field
		if (!Main.Fields[1])
		{
			HorInput = Input.GetAxis("Horizontal");
			VerInput = Input.GetAxis("Vertical");
		}
		else
		{
			HorInput = -Input.GetAxis("Horizontal");
			VerInput = -Input.GetAxis("Vertical");
		}

		if(Input.GetButton("Focus") && (Input.GetAxis("Focus") > 0))
		{
			Focus = 0.5f;
		}
		else
			Focus = 1f;


		if (Input.GetButtonDown("Command 1"))
		{
			StartCoroutine(InitiatePowerup(0));
		}
		else if (Input.GetButtonDown("Command 2"))
		{
			StartCoroutine(InitiatePowerup(1));
		}
		else if (Input.GetButtonDown("Command 3"))
		{
			StartCoroutine(InitiatePowerup(2));
		}

		if (Input.GetButton("Fire") && GameControl.control.PL.upgrades[11] > 0)
		{
			StartCoroutine(InitiatePowerup(3));
		}
		/*
		//Speedup
		if (Input.GetButtonDown("Focus") && Input.GetAxis("Focus") < 0 && GameControl.control.PL.upgrades[5] > 0)
		{
			if (!zooming)
			{
				GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[5]);
				zooming = true;
				Shipspeed *= 1.25f + (GameControl.control.PL.upgrades[5] / 4);
				zoomlight.enabled = true;
			}
			else
			{
				GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[4]);
				zooming = false;
				Shipspeed = 6;
				zoomlight.enabled = false;
			}
		}

		if (Input.GetButtonUp("Pause"))
		{
			if (Time.timeScale != 0)
			{
				GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[7]);
				Time.timeScale = 0;
				Paused = true;
			}
			else
			{
				GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[7]);
				Time.timeScale = 1;	
				Paused = false;
			}
		}

		//Slowdown
		if (Input.GetButton("Slowdown") && !Paused && GameControl.control.PL.upgrades[9] > 0)
		{
			StartCoroutine(TimeSlowdown());
		}

		//Shield
		if (Input.GetButton("Shield") && !Invincible && GameControl.control.PL.upgrades[7] > 0)
		{

			GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[4]);
			Invincible = true;
			mesh.material.color = Color.red;
			mesh.sharedMaterial.SetColor("_EmissionColor",Color.red);
			StartCoroutine(InvincibilityCooldown());

			//GameControl.control.PL.options.Mercy /=2 ;
		}

		//Sonic Shooter
		if (Input.GetButton("Fire") && GameControl.control.PL.upgrades[11] > 0 && (Time.time - timeAtLastShot) >= minTimeBetween2Shots)
		{
            Debug.Log("Fire");
			timeAtLastShot = Time.time;

			GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[8]);
			//GameObject Shot = Main.Bullets[11];
			//Shot = Instantiate(Shot, transform.position, Quaternion.identity);
			//Shot.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, Playrgd.rotation.eulerAngles.y - 90, 0);
			//Shot.transform.Rotate(new Vector3(0,Playrgd.rotation.eulerAngles.y - 90,0));
			//Shot.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 600);
		}
		*/
	}

	void FixedUpdate ()
	{
		//If Stealth applied
		if (Main.Fields[8])
		{
			mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
		}
		else
			mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		//If Slowdown applied
		if (Main.Fields[11])
			Focus /= 1.5f;

		//If Snail applied
		if (Main.Fields[12])
			Focus /= 5;		

		//If Freezing applied
		if (Main.Fields[7])
		{
			if (!freezing)
			{
				freezing = true;
				StartCoroutine(Freezingfield());
			}

			if (frozen)
				Focus = 0;
		}

		Vector3 movement = new Vector3 (-VerInput, 0f, HorInput);
		//if (VerInput == 0 && HorInput == 0)
		//	Playrgd.velocity = Vector3.zero;
		if(MovingCamera)
			moveCamera.TrackPlayer();
		Playrgd.velocity = Shipspeed * movement * Focus;

		v = new Vector3(HorInput, 0, VerInput).normalized;
		v.x = Mathf.Round(v.x);
		v.z = Mathf.Round(v.z);
		if (v.sqrMagnitude > 0.01f)
			targetDir = v.normalized;

		Playrgd.rotation = Quaternion.Slerp(Playrgd.rotation, Quaternion.LookRotation(targetDir), Time.time / 3f);
		
		if(!DecoyHologram.activeInHierarchy)
			pos = Playrgd.position;
		else
		{
			DecoyHologram.transform.position = pos;
			DecoyHologram.transform.rotation = transform.rotation;
		}
			
	}

	public IEnumerator InitiatePowerup(int x)
	{

		if (Main.Fields[4])
		{
			Debug.Log("Null Field Active, cannot use Powerups");
			yield break;
		}
		switch(EquippedPowerups[x].EquipSlot)
		{
			//Nothing
			case 0: 
			Debug.Log("That slot is unequipped");
			yield break;

			//Decoy
			case 1:
				if (GameControl.control.PL.upgrades[4] == 0)
					yield break;
				if (!EquippedPowerups[x].isActive)
				{
					EquippedPowerups[x].isActive = true;
					DecoyHealth = 3 + GameControl.control.PL.upgrades[4] * 4;
					DecoyHologram.transform.position = pos;
					DecoyHologram.SetActive(true);
					yield return new WaitForSeconds(4);
					DecoyHologram.SetActive(false);
					yield return new WaitForSeconds(PowerupCooldowns[0]);
					EquippedPowerups[x].isActive = false;
				}

				
			break;

			//Speedup
			case 2:
				if (GameControl.control.PL.upgrades[5] == 0)
					yield break;
				if (!EquippedPowerups[x].isActive)
				{
					GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[5]);
					EquippedPowerups[x].isActive = true;
					Shipspeed *= 1.25f + (GameControl.control.PL.upgrades[5] / 4);
					zoomlight.enabled = true;
				}
				else
				{
					GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[4]);
					EquippedPowerups[x].isActive = false;
					Shipspeed = 6;
					zoomlight.enabled = false;
				}
			break;

			//Silence
			case 3:
				if (GameControl.control.PL.upgrades[6] == 0)
					yield break;
				if (!EquippedPowerups[x].isActive)
				{
					EquippedPowerups[x].isActive = true;
					yield return new WaitForSeconds(PowerupCooldowns[2]);
					EquippedPowerups[x].isActive = false;
				}
			break;

			//Shield
			case 4:
				if (GameControl.control.PL.upgrades[7] == 0)
					yield break;
				if (!EquippedPowerups[x].isActive && !Invincible)
				{
					EquippedPowerups[x].isActive = true;
					GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[4]);
					Invincible = true;
					mesh.material.color = Color.red;
					mesh.sharedMaterial.SetColor("_EmissionColor",Color.red);
					float temp = MercyUpgrade;
					MercyUpgrade = 1 + 0.5f * GameControl.control.PL.upgrades[7];
					StartCoroutine(InvincibilityCooldown());
					yield return new WaitForSeconds(0.1f);
					MercyUpgrade = temp;
					yield return new WaitForSeconds(Main.Levelstats.InvincibilityAfterHit * MercyUpgrade + PowerupCooldowns[3]);
					EquippedPowerups[x].isActive = false;
				}
			break;

			//Reflector
			case 5:
				if (GameControl.control.PL.upgrades[8] == 0)
					yield break;
				if (!EquippedPowerups[x].isActive)
				{
					EquippedPowerups[x].isActive = true;
					yield return new WaitForSeconds(PowerupCooldowns[4]);
					EquippedPowerups[x].isActive = false;
				}
			break;

			//Slowdown
			case 6:
				if (GameControl.control.PL.upgrades[9] == 0)
					yield break;
					if (!EquippedPowerups[x].isActive)
					{
						EquippedPowerups[x].isActive = true;
						GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[6]);
						Time.timeScale = 0.5f - (0.05f * GameControl.control.PL.upgrades[9]);

						yield return new WaitForSeconds (4f);
						Time.timeScale = 1;
						GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[6]);
						yield return new WaitForSeconds(PowerupCooldowns[5]);
						EquippedPowerups[x].isActive = false;
					}
			break;

			//Portal
			case 7:
				if (GameControl.control.PL.upgrades[10] == 0)
					yield break;
				if (!EquippedPowerups[x].isActive)
				{
					EquippedPowerups[x].isActive = true;
					yield return new WaitForSeconds(PowerupCooldowns[6]);
					EquippedPowerups[x].isActive = false;
				}
			break;

			//Sonic Shooter, only activated with space
			case 8:
			if (GameControl.control.PL.upgrades[10] == 0)
					yield break;
				if (!EquippedPowerups[x].isActive)
				{
					EquippedPowerups[x].isActive = true;
					GameControl.control.PlaySound(PlayerAudioSource, true, Playerclips[8]);
					SonicShooter.FireWeapon(-3, false, Playrgd.rotation.eulerAngles.y, false, 1);
					yield return new WaitForSeconds(PowerupCooldowns[7]);
					EquippedPowerups[x].isActive = false;
				}

			break;
		}
	}


	public IEnumerator TimeSlowdown()
	{
		GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[6]);
		Time.timeScale = 0.5f - (0.05f * GameControl.control.PL.upgrades[9]);

		yield return new WaitForSeconds (4f);
		Time.timeScale = 1;
		GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[6]);
	}

	public IEnumerator InvincibilityCooldown()
	{
		yield return new WaitForSeconds(Main.Levelstats.InvincibilityAfterHit * MercyUpgrade);
		Invincible = false;
		mesh.sharedMaterial.SetColor("_EmissionColor",Color.white);
		mesh.material.color = Color.white;
	}

	public void UpdateHealth(int HealthChangedAmount)
	{
		if (HealthChangedAmount > 0 && CurrentHP != MaxHP)
			GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[1], false);
			//Heallingsound.Play();
		else if (HealthChangedAmount < 0)
		{
			if (Main.Fields[18])
				HealthChangedAmount--;
			HealthChangedAmount *= Main.Levelstats.DamagePerHit;
			Invincible = true;
			if(CurrentHP + HealthChangedAmount <= 0)
			{
				StartCoroutine(PlayerDies());
			}
			else
			{
				GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[2], false);
				mesh.material.color = new Color(.5f,.5f,.5f,.5f);
				mesh.sharedMaterial.SetColor("_EmissionColor",Color.black);
				StartCoroutine(InvincibilityCooldown());
			}
		}

		CurrentHP+= HealthChangedAmount;


		if (CurrentHP >= MaxHP)
			CurrentHP = MaxHP;
		//Healthbar.fillAmount = (float)CurrentHP/MaxHP;
		UserInterface.UI.UpdateHealthbar((float)CurrentHP/MaxHP);
	}

	public IEnumerator Freezingfield()
	{
		frozen = false;
		yield return new WaitForSeconds(0.5f);
		frozen = true;
		mesh.material.color = Color.blue;
		yield return new WaitForSeconds(0.25f);
		mesh.material.color = Color.white;
		if (Main.Fields[7])
			StartCoroutine(Freezingfield());
	}

	public IEnumerator PlayerDies()
	{
		GameControl.control.PlaySound(PlayerAudioSource, false, Playerclips[0], false);
		//Death.Play();
		Explosion.SetActive(true);
		mesh.enabled = false;
		hitbox.enabled = false;
		this.enabled = false;
		yield return new WaitForSeconds(0.02f);
	}


	void OnCollisionEnter (Collision other )
	{
		if (other.gameObject.tag == "Wall")
		CollidingWithWall = true;
	}

	void OnCollisionExit (Collision other)
	{
		if (other.gameObject.tag == "Wall")
		CollidingWithWall = false;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Exit")
		{
			Debug.Log("THE END HAS COME!");
			Main.ExitReached = true;
		}
			
		
	}

	
}

