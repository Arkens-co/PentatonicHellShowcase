using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyFace : Minion
{

    public OrdinalJoy Controller;

    // Start is called before the first frame update
    public void JoyTargeting(int i)
	{
		Controller.TargetingPlayer(i);
	}
}
