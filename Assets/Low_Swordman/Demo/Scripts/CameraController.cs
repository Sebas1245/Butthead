using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public static CameraController Instance;

    public GameObject Target;
    public int Smoothvalue =2;
    public float PosY = 1;
    public float PosXMax = 0;
    public float PosXMin = 0;

    // Use this for initialization
    public Coroutine my_co;

    void Start()
    {
     
    }


    void Update()
    {
        float PosX = Target.transform.position.x;
        if(PosX>PosXMax) //91.47f 
            PosX = PosXMax;
        if(PosX<PosXMin) //-2.46f 
            PosX = PosXMin;
        Vector3 Targetpos = new Vector3(PosX, PosY, -100);
        transform.position = Vector3.Lerp(transform.position, Targetpos, Time.deltaTime * Smoothvalue);

    }



}
