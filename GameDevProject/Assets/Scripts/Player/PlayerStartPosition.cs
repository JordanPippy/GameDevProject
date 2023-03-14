using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerController p = FindObjectOfType<PlayerController>();
        if (p != null)
        {
            p.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }
        // Destroy itself?
        //Destroy(this.gameObject);
    }
}
