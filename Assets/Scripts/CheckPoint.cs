using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public AudioClip Clip;
    private bool Passed = false;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.up)* 1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 1f);
        if(hit){
            GameObject Player = hit.transform.gameObject;
            if(Player.tag == "Player" && !Passed)
            {
                Passed = true;
                AudioSource.PlayClipAtPoint(Clip, transform.position);
                StartCoroutine(textCheckPoint());
                Swordman Script = Player.GetComponent<Swordman>();
                Script.spawnPointX = transform.position.x;
                Script.spawnPointY = transform.position.y;
            }
        }
        
    }
    IEnumerator textCheckPoint(){
        text.SetActive(true);
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }
}
