using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMM : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource FX;
    public AudioClip hover;
    public AudioClip click;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void HoverSound(){
        FX.PlayOneShot(hover);
    }
    public void ClickSound(){
        FX.PlayOneShot(click);
    }
}
