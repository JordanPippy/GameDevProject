using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public AbilityBase ability;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        ability.Use(col.gameObject);
        Destroy(gameObject);
    }
}
