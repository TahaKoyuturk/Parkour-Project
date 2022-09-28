using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 5);
            Destroy(this.gameObject);
        }
    }
   
}
