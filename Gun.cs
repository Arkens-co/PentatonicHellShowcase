using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour 
{
	public int SoundIndex;
	public static float Bulletspeed = 3.5f;
	public static float OutsideSpeedModifier = 1f;
	public static float VolumeDamper = 1f;
	private Vector3 pos;
	private bool CurrentlyFiring = false;

	public bool IgnoresDistance;

	private static int Healcount;
	public AudioClip[] Tonemades;
	public GameObject Rotator;
	public Transform OriginPoint;
	public Light Winder;
	public ParticleSystem Warning;
	public bool NotTouchingWall = false;
	public bool ConstantlyRotating;
	GameObject Shot;
	//private bool Split = false;
	private float rotateamount = 0;
	private Material bulletskin;

	private Bullet bulletholder = null;

	//public static bool Perpendicular = false;
	public float PerpendicularAngle;

	public AudioSource Toneplayer;

	void Awake ()
	{
		if (Toneplayer == null)
			Toneplayer = gameObject.GetComponent<AudioSource>();
		if (Winder == null)
			Winder = gameObject.GetComponentInChildren<Light>();
		//Bulletspeed = 150f;
		if (Warning == null)
			Warning = gameObject.GetComponentInChildren<ParticleSystem>();
	}

	// Use this for initialization
	void Start () 
	{
		if (ConstantlyRotating)
			StartCoroutine(RotateToShip());
		Winder.enabled = false;
		this.enabled = false;
	}

	public void FireWeapon(int type, bool targeted, float extrarotation, bool playsound)
	{
		FireWeapon(type, targeted, extrarotation, playsound, 1);
	}

	public void FireWeapon(int type, bool targeted, float extrarotation, bool playsound, float speedmultiplier)
	{
		//CurrentlyFiring = true;
		if (OriginPoint != null)
		{
			pos.x = OriginPoint.position.x;
			pos.z = OriginPoint.position.z;
			pos.y = 1;
		}
		else
		{
			pos.x = transform.position.x;
			pos.z = transform.position.z;
			pos.y = 1;
		}

		//Vector3 VectorToTarget = (Main.Player.pos - pos).normalized;
		float targetedangle = Mathf.Atan2(Main.Player.pos.x - pos.x, Main.Player.pos.z - pos.z) * Mathf.Rad2Deg;

		if (Vector3.Distance(Main.Player.pos, pos) > Main.Levelstats.MaxDistanceForGuns && !IgnoresDistance)
		{
			return;
		}

		//conditional modifiers
		float Force = 1;
		float volume = .7f;
		bool Split = false;
		//TempWarning.startColor = Color.red;
		//Toneplayer.volume = .7f;

		rotateamount = extrarotation;
		switch(type)
		{

		//Normal
		case 1:

			Force = FinalSpeed(1*speedmultiplier);
			AssignBullet(0, new Vector2(0f,.75f), Color.red, false);
			break;
		
		//Slow
		case 2:

			Force = FinalSpeed(0.5f*speedmultiplier);
			AssignBullet(1, new Vector2(0,0f), Color.blue, false);

			volume = 0.5f;
			break;
		
		//Jester
		case 3:

			Force = FinalSpeed(1.1f*speedmultiplier);
			AssignBullet(2, new Vector2(.25f,0f), Color.yellow, false);

			bulletholder.bounces = 0;
			break;
		
		//Split
		case 4:

			Force = FinalSpeed(1.6f*speedmultiplier);
			AssignBullet(3, new Vector2(0f,.25f), Color.magenta, false);

			Split = true;
			rotateamount += 10;
			break;

		//Death		
		case 5:
		
			float temp = 2f;
			if ((Vector3.Distance(Main.Player.pos, pos) < 3))
			{
				Debug.Log("Super Close");
				temp = 1.5f;
			}
			else if ((Vector3.Distance(Main.Player.pos, pos) > 20))
			{
				Debug.Log("Really far away!");
				temp = 4;
			}				
			else if ((Vector3.Distance(Main.Player.pos, pos) > 10))
			{
				Debug.Log("Decently far away");
				temp = 3f;

			}

			Force = FinalSpeed(temp*speedmultiplier);
			AssignBullet(4, new Vector2(.25f,.5f), Color.white, false);

			Vector3 targetVelocity = Main.Player.Playrgd.velocity;
			//Force = 8;
			float a = (targetVelocity.x * targetVelocity.x) + (targetVelocity.z * targetVelocity.z) - (Force * Force);
			float b = 2 * (targetVelocity.x * (Main.Player.pos.x - pos.x) + targetVelocity.z * (Main.Player.pos.z - pos.z));
			float c = ((Main.Player.pos.x - pos.x) * (Main.Player.pos.x - pos.x)) + ((Main.Player.pos.z - pos.z) * (Main.Player.pos.z - pos.z));

			float disc = b*b - (4 * a * c);
			if (disc < 0)
			{
				Debug.Log("No Possible hit");
				//Force *= 1.5f;
				break;
			}
				float t = Mathf.Max ((-1 * b + Mathf.Sqrt(disc)) / (2 * a), (-1 * b - Mathf.Sqrt(disc)) / (2 * a));

				Vector2 aim = new Vector2((targetVelocity.x * t) + Main.Player.pos.x, (targetVelocity.z * t) + Main.Player.pos.z);

				//VectorToTarget = new Vector3(aimX, 1, aimZ);
				//Debug.Log("velocity, " + targetVelocity + " force, " + Force + " A, " + a +  " B, " + b + " C, " + c + " disc, " + disc + " t1, " + t1 + " t2, " + t2 + " t, " + t);
				//Debug.Log("Final Target, " + VectorToTarget);
				temp = targetedangle;
				float moa = 90; //Margin of Adjustment
				targetedangle = Mathf.Atan2(aim.x - pos.x, aim.y - pos.z) * Mathf.Rad2Deg;

				if (temp+moa < targetedangle)
				{
					Debug.Log("Too high an adjustment");
					targetedangle = temp+moa;
				}
				else if (temp-moa > targetedangle)
				{
					Debug.Log("Too low an adjustment");
					targetedangle = temp - moa;
				}
			
			//Force = FinalSpeed(2*speedmultiplier);
			break;

		//Grinder
		case 6:

			Force = FinalSpeed(1.5f*speedmultiplier);
			AssignBullet(5, new Vector2(.25f,.25f), Color.cyan, true);

			Shot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
			break;

		//Stealth
		case 7:
	
			Force = FinalSpeed(0.9f*speedmultiplier);
			AssignBullet(6, new Vector2(0f,0f), Color.gray, false);

			if (Main.Fields[8])
				bulletholder.mesh.enabled = false;
			break;

		//Lover
		case 8:

			Force = FinalSpeed(0.85f*speedmultiplier);
			AssignBullet(7, new Vector2(0f,0.5f), new Color (1, .8f, .8f), true);
			break;

		//Explosive
		case 9:

			Force = FinalSpeed(0.4f*speedmultiplier);
			AssignBullet(8, new Vector2(.5f,.5f), new Color (1, .6f, .6f), false);

			break;

		//Curving Normal
		case 10:

			Force = FinalSpeed(1f*speedmultiplier);
			AssignBullet(13, new Vector2(0f,0.75f), new Color (1, .2f, 0), true);
			break;

		//Curving Snake
		case 11:

			Force = FinalSpeed(1f*speedmultiplier);
			AssignBullet(12, new Vector2(0f,0.75f), new Color (1, .1f, .1f), false);
			//bulletholder.anim.SetFloat("Speed", Force / Bulletspeed);
			//return;
			break;

		//Curving Normal
		case 12:

			Force = FinalSpeed(1f*speedmultiplier);
			AssignBullet(14, new Vector2(.0f,0.75f), new Color (1, 0, .2f), true);
			break;

		//Healing
		case -1:

			if ((Healcount < 3 || Main.Player.CurrentHP == Main.Player.MaxHP))
			{
				Healcount++;
				//Shot.gameObject.SetActive(false);
				return;
			}
			else
				Healcount = 0;

			Force = FinalSpeed(0.8f*speedmultiplier);
			AssignBullet(10, new Vector2(.75f,.75f), Color.green, false);
			break;

		//Key
		case -2:
			Force = FinalSpeed(0.75f*speedmultiplier);
			AssignBullet(11, new Vector2(.5f,0.75f), new Color (.5f, 1, .5f), false);

			targeted = false;
			break;

		//Player, from Sonic Shooter
		case -3:

			Force = FinalSpeed(4f*speedmultiplier);
			AssignBullet(9, new Vector2(.25f,0.75f), new Color(0,0,0,0), false);

			break;

		//Healing, Ignores Healcount
		case -4:

			Force = FinalSpeed(0.8f*speedmultiplier);
			AssignBullet(10, new Vector2(.75f,0.75f), Color.green, false);

			break;

		//Second Split Bullet
		case -5:
			
			Force = FinalSpeed(1.6f*speedmultiplier);
			AssignBullet(3, new Vector2(0f,0.25f), new Color(0,0,0,0), false);

			rotateamount -= 10;
			break;

		default:
			Debug.Log("Error, bullet type is currently nothing");
			break;
		}

		if (playsound)
		{
			volume *= VolumeDamper;
			if (type > 0)
				GameControl.control.PlaySound(Toneplayer,true,Tonemades[type-1], false, volume);
			else
				GameControl.control.PlaySound(Toneplayer,true,Tonemades[type * -1 + 9], false, volume);
			//Toneplayer.PlayOneShot(Tonemades[type-1],1);
		}
		//Debug.DrawLine(pos, VectorToTarget, Color.white, 2.5f);

		if (Main.Fields[10] || !targeted)
		{
			targetedangle = PerpendicularAngle;
		}

		Shot.transform.rotation = Quaternion.identity;
		Shot.transform.Rotate(0, targetedangle + rotateamount, 0);
		bulletholder.rgb.rotation = Shot.transform.rotation;

		//bulletholder.rgb.velocity = Vector3.zero;
		Shot.SetActive(true);
		Shot.transform.position = pos;
		//Debug.Log(Shot.transform.position);
		//bullet.rgb.AddRelativeForce(Vector3.forward * Force);
		bulletholder.ThisBulletSpeed = Force;
		if (type != 11)
			bulletholder.rgb.velocity = Shot.transform.forward * Force;
		else
			bulletholder.enabled = true;

		if (Split)
		{
			this.FireWeapon(-5, targeted, extrarotation, false, speedmultiplier);
		}
				
	}

	private float FinalSpeed(float basespeed)
	{
		//If Slowdown applied
			if (Main.Fields[11])
				basespeed /= 1.5f;

			//if Snail applied
			if (Main.Fields[12])
				basespeed /= 4;

			basespeed *= Bulletspeed * OutsideSpeedModifier *  GameControl.control.PL.options.Bulletspeedmultiplier;

			return basespeed;			
	}

	private void AssignBullet(int identity, Vector2 offset, Color assign, bool enable)
	{
		Shot = ObjectPooler.SharedPooler.GetPooledObject(identity);
		bulletholder = Shot.GetComponent<Bullet>();
		bulletholder.mesh.material.mainTextureOffset = offset;

		if (NotTouchingWall)
			bulletholder.OnScreen = true;
		else
			bulletholder.OnScreen = false;

		if (!bulletholder.SetSkin)
		{
			bulletholder.mesh.material = Shot.GetComponent<MeshRenderer>().material;
			bulletholder.SetSkin = true;
		}
			//bulletholder.glow.enabled = false;

		//if Stealth applied
		if (Main.Fields[8])
		{
			bulletholder.glow.enabled = false;
			bulletholder.mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

		}
		else if (identity != 6)
			bulletholder.mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		if(enable)
			bulletholder.enabled = true;

		ParticleSystem.MainModule TempWarning = Warning.main;

		TempWarning.startColor = assign;
		//TempWarning.Play();
		Warning.Play();
	}

	public IEnumerator RotateToShip()
	{
		yield return new WaitForSeconds(0.03f);
		if (!Main.Fields[10] && Rotator != null && !CurrentlyFiring)
		{
			Rotator.transform.rotation = Quaternion.LookRotation((Main.Player.pos - Rotator.transform.position).normalized);
			//Rotator.transform.Rotate(0,-90,0);	
		}
		else if (Rotator != null && !CurrentlyFiring)
		{
			//transform.
			Rotator.transform.localRotation = Quaternion.Euler(0,PerpendicularAngle,0);
		}
		StartCoroutine(RotateToShip());
	}

	public IEnumerator WindUp(int type)
	{
		CurrentlyFiring = true;
		Winder.enabled = true;
		switch (type)
		{

		case 1: 
			Winder.color = Color.red;
			break;

		case 2: 
			Winder.color = Color.blue;
			break;

		case 3: 
			Winder.color = Color.yellow;
			break;

		case 4: 
			Winder.color = Color.magenta;
			break;

		case 5: 
			Winder.color = Color.black;
			break;

		case 6: 
			Winder.color = Color.cyan;
			break;

		case 7: 
			Winder.color = Color.white;
			break;

		case 8: 
			Winder.color = new Color(1,.8f,.8f);
			break;

		case 9: 
			Winder.color = new Color(1,.8f,.8f);
			break;

		case 10: 
			Winder.color = new Color (1,.6f,0);
			break;

		case -1:
			Winder.color = Color.green;
			break;

		case -2:
			Winder.color = new Color (.5f, 1, .5f);
			break;

		}

		yield return new WaitForSeconds(0.125f / Main.Levelstats.CadenceSpeed / GameControl.control.PL.options.Cadencespeedmultiplier);

		Winder.enabled = false;
		CurrentlyFiring = false;
	}
		
}
