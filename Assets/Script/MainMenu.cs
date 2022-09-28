using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        PlayerPrefs.SetInt("item", 0);
        PlayerPrefs.SetInt("Gold", 0);
        Application.Quit();
    }
    public void ClearPrefs()
    {
        PlayerPrefs.SetInt("item", 0);
        PlayerPrefs.SetInt("Gold", 0);
    }
}
