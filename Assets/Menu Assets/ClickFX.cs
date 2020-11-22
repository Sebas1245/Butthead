using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFX : MonoBehaviour
{
    public AudioSource MyFx;
    public AudioClip hoverFx;
    public AudioClip clickFx;

    public void HoverSound()
    {
        MyFx.PlayOneShot(hoverFx);
    }
    public void Clicksound()
    {
        MyFx.PlayOneShot(clickFx);
    }
}
