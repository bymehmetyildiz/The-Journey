using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }    

    public void ContinueGame()
    {
        //SceneManager.LoadScene(x);
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void About()
    {
        SceneManager.LoadScene("About");
    }

    public void Quit()
    {
        Application.Quit();   
    }



}
