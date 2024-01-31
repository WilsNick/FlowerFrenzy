using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class StartScreen : MonoBehaviour
{

    public void PlayButton() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SettingsButton()
    {
        // Add code to open a settings menu here
    }

    public void ShopButton()
    {
        SceneManager.LoadScene("Shop");
    }
}
