
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mainmenu : MonoBehaviour
{
   public void Playugame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void amonos()
    {
        Debug.Log("i QUIT!");
        Application.Quit();
    }
}

