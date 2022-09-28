using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform character;
    void Update()
    {
        if (character != null)
        {
            this.transform.position = new Vector3(character.position.x, character.position.y + 0.45f, character.position.z - 0.9f);
            this.transform.rotation = Quaternion.Euler(15, 0, 0);
        }
        
    }
}
