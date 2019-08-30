using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    // Start is called before the first frame update

    public Gun[] Turrets;

    public MeshRenderer Floor;

    public string Effect;
    public bool LockedCamera;

    public float HeightofCamera = 11;

    public Vector3 PlayerStart;

    public GameObject ExitLocation;

    public Goal[] DefendGoals;

    public Collider Walls;

    //public Material[] SwitchScreens;

    //public float CadenceSpeed;
    //For CadenceSpeed

    void Start()
    {
        
    }
}
