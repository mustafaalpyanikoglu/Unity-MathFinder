using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sonucManager : MonoBehaviour
{
    public void oyunuYenidenBaslat()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void anaMenuyeDon()
    {
        SceneManager.LoadScene("MenuLevel");
    }

}
