using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeOrExit : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void Home()
    {
        SceneManager.LoadScene("StartUI");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
