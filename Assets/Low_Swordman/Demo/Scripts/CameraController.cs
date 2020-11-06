using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public static CameraController Instance;

    public GameObject Target;
    public int Smoothvalue =2;
    public float PosY = 1;


    // Use this for initialization
    public Coroutine my_co;

    void Start()
    {
     
    }


    void Update()
    {
        float PosX = Target.transform.position.x;
        if(PosX>91.47f)
            PosX = 91.47f;
        if(PosX<-2.46f)
            PosX = -2.46f;
        Vector3 Targetpos = new Vector3(PosX, PosY, -100);
        transform.position = Vector3.Lerp(transform.position, Targetpos, Time.deltaTime * Smoothvalue);

    }



}
