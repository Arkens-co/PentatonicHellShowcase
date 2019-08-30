using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour 
{

	//To do, break up between bosses.
	public int MaxHP;
	public int CurrentHP;
	public Animator anim;
	public Gun weapon;
	//public Boss Controller;
	public GameObject Destroyed;

	protected Quaternion rot;

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Collision from Boss");
		if (other.gameObject.tag == "Player")
		{
			if(!Main.Player.Invincible)
				Main.Player.UpdateHealth(-1);
		}
	}

}
