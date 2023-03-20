using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbi : Ability
{
    public float speed = 8.0f;
    private Vector3 movement;
    private float spawn_time;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 angleVector = -Library.VectorFromAngle(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
        movement = (new Vector3(angleVector.x, angleVector.y, 0)).normalized;
        transform.position += movement * 1.5f;

        spawn_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (movement * speed * Time.deltaTime);

        if ((Time.time - spawn_time) > 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {      
        //crab melee attacks destroy incoming projectiles (otherwise, crab is not viable)   
        //check if its a projectile
        if (col.GetComponent<Ability>() != null)
        {
            Destroy(col.gameObject);
            print("kljfalk");
        }

        if (!this.CompareTag(col.tag))
        {
            if (col.GetComponent<Ability>() == null)
            {
                ability.Use(col.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
