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
    public bool InBossArea = false;
    public GameObject BackgroundMusic;
    public GameObject BossMusic;
    public GameObject VictoryMusic;
    BossController BossScript;
    // Use this for initialization
    public Coroutine my_co;

    void Start()
    {
        BossMusic.SetActive(false);
        BackgroundMusic.SetActive(true);
        BossScript = GameObject.FindGameObjectWithTag("Boss1").GetComponent<BossController>();
    }

    void Update()
    {
        float PosX = Target.transform.position.x;
        if(PosX>PosXMax) { //91.47f 
            PosX = PosXMax;
            InBossArea = true;
            BackgroundMusic.SetActive(false);
            BossMusic.SetActive(true);
        }
        if(PosX<PosXMin){ //-2.46f
            PosX = PosXMin;
        }
        if(BossScript.BossDefeated) {
            BackgroundMusic.SetActive(false);
            BossMusic.SetActive(false);
            VictoryMusic.SetActive(true);
        }
        Vector3 Targetpos = new Vector3(PosX, PosY, -100);
        transform.position = Vector3.Lerp(transform.position, Targetpos, Time.deltaTime * Smoothvalue);

    }



}
