using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    AudioSource source;
    AudioClip sound;
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        sound = source.clip;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enters collision 2D");
        Debug.Log(collision);
        if(collision.gameObject.tag == "Player")
        {
            //StartCoroutine(PlaySound());
            AudioSource.PlayClipAtPoint(sound, transform.position);
            Destroy(gameObject);
        }
    }
    /*IEnumerator PlaySound(){
        source.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    */
}
