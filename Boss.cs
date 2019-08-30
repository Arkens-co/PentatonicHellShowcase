using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour 
{
	public string type;

	public Vector3 pos;
	public Transform tr;
	public Rigidbody rgd;
	public Gun[] Weapons;
	public int MaxHP;
	public int CurrentHP;
	public bool Invincible;
	public GameObject Explosion;
	public AudioSource Audio;
	public AudioClip[] Clips;


	//public Minion[] Turrets;
	public float ConstantAngle;
	public GameObject CentralRotation;
	public Animator Body;
	public bool charging;
	public int Variancecounter;

	public bool Introdone = false;

	protected IEnumerator lastroutine = null;

	// Use this for initialization
	void Awake () 
	{
		//pos = transform.position;
		//tr = transform;
		this.enabled = false;
		Explosion.SetActive(false);
		gameObject.SetActive(false);
	}

	public void TurnOn()
	{
		this.enabled = true;
		StartCoroutine(Intro());
	}

	public virtual IEnumerator Intro()
	{
        yield return new WaitForSeconds(1f);
		//Should never be activated here, and always overriden
	}

	public virtual IEnumerator TakeDamage()
	{
		yield return new WaitForSeconds(1f);
		//Also should never be activated here, and always overriden
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Collision from Boss");
		if (other.gameObject.tag == "Player")
		{
			if(!Main.Player.Invincible)
				Main.Player.UpdateHealth(-1);
		}

		else if (other.gameObject.tag == "Bullet")
		{
			other.gameObject.SetActive(false);
		}
	}

    void Confirmation()
    {
        Debug.Log("This Animation is playing");
    }

	virtual public IEnumerator RadioChargeSlam ()
	{
		yield return new WaitForSeconds(1f);
	}
	//Only here to be redirected to Melus
}

