using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject redPanel1;
    public GameObject redPanel2;
    public GameObject redPanel3;
    public void Hat()
    {
        if (PlayerPrefs.GetInt("Gold") >= 10)
        {
            PlayerPrefs.SetInt("item", 1);
            button1.SetActive(false);
            button2.SetActive(true);
            button3.SetActive(true);

            redPanel1.SetActive(true);
            redPanel2.SetActive(false);
            redPanel3.SetActive(false);
            Debug.Log("1");
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - 10);
        }
    }public void Helmet()
    {
        if (PlayerPrefs.GetInt("Gold") >= 20)
        {
            PlayerPrefs.SetInt("item", 2);
            button1.SetActive(true);
            button2.SetActive(false);
            button3.SetActive(true);

            redPanel1.SetActive(false);
            redPanel2.SetActive(true);
            redPanel3.SetActive(false);
            Debug.Log("2");
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - 20);
        }
            
    }
    public void Horn()
    {
        if (PlayerPrefs.GetInt("Gold") >= 30)
        {
            PlayerPrefs.SetInt("item", 3);
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(false);

            redPanel1.SetActive(false);
            redPanel2.SetActive(false);
            redPanel3.SetActive(true);
            Debug.Log("3");
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - 30);
        }
            
    }
}
