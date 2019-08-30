using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	public bool OnScreen = false;
	public Rigidbody rgb;
	public MeshRenderer mesh;
	public Light glow;

	public Animator anim;
	private bool TouchingWall;

	public int TypeIdentifier;

	private bool reversed = false;
	public int bounces = 0;
	public ParticleSystem Explosive;

	public Material[] swapskins;
	private Vector3 newscale = new Vector3(0.15f,0.15f,0.15f);
	public float Growthfactor = 1f;
	public float ThisBulletSpeed = 1;

	private int counter = 0;

	public bool SetSkin = false;

	void OnEnable ()
	{
		//Grinder Bullet
		if (TypeIdentifier == 6 || TypeIdentifier == 66)
		{
			newscale = transform.localScale;
			reversed = false;
			//Growthfactor = 230;
			StartCoroutine(Growing());
			this.enabled = false;
		}
		
		//Lover Bullet
		else if (TypeIdentifier == 8)
		{
			counter = 0;
			StartCoroutine(Loving());
			this.enabled = false;
		}

		//Curving Bullets
		else if (TypeIdentifier == 10 || TypeIdentifier == 12)
		{
			//Debug.Log("Curve along");
			StartCoroutine(Curving(TypeIdentifier - 11));
			this.enabled = false;
		}

		else if (TypeIdentifier == 11)
		{
			Debug.Log("Snakes!");
			rgb.velocity = Vector3.zero;
			anim.SetFloat("Speed", ThisBulletSpeed / Gun.Bulletspeed);
			//StartCoroutine(Spiral());
			this.enabled = false;
		}

		else
		{
			Debug.Log("Why did you enable this bullet?");
			this.enabled = false;
		}
	}

	IEnumerator Growing()
	{

		if (Main.Player.Paused)
		{
			Debug.Log("Paused");
			yield return new WaitUntil(() => !Main.Player.Paused);	
		}
		else
		{
			rgb.AddRelativeForce(Vector3.back * ThisBulletSpeed);
				
			newscale.x *= 1 + (.02f * ThisBulletSpeed * Growthfactor);
			newscale.y *= 1 + (.005f * ThisBulletSpeed * Growthfactor);
			newscale.z *= 1 + (.01f * ThisBulletSpeed * Growthfactor);
			transform.localScale = newscale;
		}
		yield return new WaitForSeconds (0.05f);
		StartCoroutine (Growing());
	}

	IEnumerator Curving(int leftorright)
	{
		if (Main.Player.Paused)
		{
			Debug.Log("Paused");
			yield return new WaitUntil(() => !Main.Player.Paused);	
		}
		//Debug.Log("Curve along at " + leftorright);
		float temp = 10 * leftorright;

		rgb.AddRelativeForce(new Vector3(temp,0,-5) * ThisBulletSpeed);
		yield return new WaitForSeconds (0.5f);
		StartCoroutine(Curving(leftorright));
	}

	IEnumerator Snake()
	{
		if (Main.Player.Paused)
		{
			Debug.Log("Paused");
			yield return new WaitUntil(() => !Main.Player.Paused);	
		}
		rgb.velocity = Vector3.zero;

		//Debug.Log("Curve along at " + leftorright);
		
		//transform.Rotate(0,20,0);
		//rgb.velocity = transform.forward * ThisBulletSpeed;
		yield return new WaitForSeconds (0.5f);
		StartCoroutine(Snake());
	}

	IEnumerator Loving()
	{
		yield return new WaitForSeconds (0.2f);
		if (Main.Player.Paused)
		{
			Debug.Log("Paused");
			yield return new WaitUntil(() => !Main.Player.Paused);	
		}
		else
		{
			counter++;
			if (counter > 10)
				ThisBulletSpeed *= 1.05f;
			//ThisBulletSpeed *= 1.1f;
			float temprot = Mathf.Atan2(Main.Player.pos.x - transform.position.x, Main.Player.pos.z - transform.position.z) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, temprot, 0), Time.deltaTime * 1000f);
			rgb.velocity = transform.forward * ThisBulletSpeed;
		}
		StartCoroutine (Loving());
	}

	void ExplosiveShot()
	{
		Explosive.Play();

		transform.rotation = Quaternion.LookRotation((Main.Player.pos - transform.position).normalized);
		//rgb.rotation = Quaternion.LookRotation((Main.Player.pos - transform.position).normalized);
		rgb.velocity = transform.forward * ThisBulletSpeed * 7;
		//rgb.AddRelativeForce(Vector3.forward * Gun.Bulletspeed * 3f * Speedadjust);
	}

	void OnCollisionEnter (Collision other)
	{
		Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.collider);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Goal" && other.isTrigger)
			OnScreen = true;
		else if (other.gameObject.tag == "Wall" && other.isTrigger)
		{
			//The Bullet hasn't left the wall yet
			if (!OnScreen)
				return;

			//Jester
			else if (TypeIdentifier == 3 || TypeIdentifier == 33 || TypeIdentifier == 66)
			{
				//Jesterfield
				if (TypeIdentifier != 33)
					bounces++;
				else if (!Main.Fields[14])
					gameObject.SetActive(false);

				if (bounces > 2)
					gameObject.SetActive(false);

				//if (bounces == 1)
				//	mesh.material = swapskins[1];
				//else if (bounces == 2)
				//	mesh.material = swapskins[2];
				mesh.material.mainTextureOffset += new Vector2 (.25f, 0);

                transform.rotation = Quaternion.LookRotation((Main.Player.pos - transform.position).normalized);

                rgb.velocity = transform.forward * ThisBulletSpeed;
			}

			//Key
			else if (TypeIdentifier == -2)
			{
				Main.Player.KeyStrikes = 0;
				UserInterface.UI.UpdateProgress(0);
				gameObject.SetActive(false);
			}

			//For everything else
			else
				gameObject.SetActive(false);
		}

		else if (other.gameObject.tag == "Player")
		{
			//Player Bullet, Sonic Shooter
			if (TypeIdentifier == -3)
			{
				return;
			}

			//Healing
			else if(TypeIdentifier == -1)
			{
				Main.Player.UpdateHealth(1);
				gameObject.SetActive(false);
				return;
			}
			
			//Key, required for Unlock Levels
			else if (TypeIdentifier == -2)
			{
				Main.Player.KeyStrikes++;
				UserInterface.UI.UpdateProgress((float)Main.Player.KeyStrikes/4);
				if (Main.Player.KeyStrikes == 4)
					Main.SegmentFlag = true;
				gameObject.SetActive(false);
				return;
			}

			if(!Main.Player.Invincible)
			{
				Main.Player.UpdateHealth(-1);
			}

			gameObject.SetActive(false);
		}

		else if (other.gameObject.tag == "Decoy")
		{
			//Any non enemy bullet
			if (TypeIdentifier == -1 || TypeIdentifier == -2 || TypeIdentifier == -3)
				return;
			Debug.Log("Decoy hit");
			Main.Player.DecoyHealth--;
			if (Main.Player.DecoyHealth <= 0)
				Main.Player.DecoyHologram.SetActive(false);
			gameObject.SetActive(false);
		}
			
	}

	//void OnTriggerStay(Collider other)
	//{
	//	if (other.gameObject.tag == "Wall" && other.isTrigger)
	//	{
	//		OnScreen = false;
	//	}
	//}

	//void OnTriggerExit(Collider other)
	//{
	//	if (other.gameObject.tag == "Wall" && other.isTrigger && !OnScreen)
	//	{
	//		OnScreen = true;
	//	}
	//}

}
