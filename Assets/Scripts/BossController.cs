using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform Player;
    CameraController CameraScript;
    public bool Vision, ClosenessLimit;
    public LayerMask PlayerLayer;
    public float VisionRadio = 2f, ClosenessRadio = 1f, Speed = 0.1f;
    public float scaleX = 0.4f;
    public float scaleY = 0.4f;
    protected Animator m_Anim;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();   
        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraScript = MainCamera.GetComponent<CameraController>();   
        m_Anim = this.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update() {
        if(CameraScript.InBossArea){
            // StartCoroutine(PathFinding());
            Vision = Physics2D.OverlapCircle(transform.position,VisionRadio, PlayerLayer);
            Vector3 Direction = Player.position - transform.position;
            if(Direction.x > 0) {
                Flip(true);
            }
            else {
                Flip(false);
            }
            m_Anim.Play("Run_02");
            transform.position += Direction * Speed * Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        
    }

    protected void Flip(bool bLeft)
    {

        transform.localScale = new Vector3(bLeft ? -scaleX : scaleX, scaleY, 0);
    }

    IEnumerator PathFinding()
    {
        yield return new WaitForSeconds(1);
        Vision = Physics2D.OverlapCircle(transform.position,VisionRadio, PlayerLayer);
        Vector3 Direction = Player.position - transform.position;
        if(Direction.x > 0) {
            Flip(true);
        }
        else {
            Flip(false);
        }
        m_Anim.Play("Run_02");
        transform.position += Direction*0.001f;
        // transform.position += transform.forward * Direction.x * Speed * Time.deltaTime;
    }
}
