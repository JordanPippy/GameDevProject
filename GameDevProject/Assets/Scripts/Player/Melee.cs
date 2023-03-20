using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability
{
    public float speed = 1.0f;
    private Vector3 movement;
    private float time_spawned;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 angleVector = -Library.VectorFromAngle(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
        movement = (new Vector3(angleVector.x, angleVector.y, 0)).normalized;
        transform.position += movement;
        time_spawned = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (movement * speed * Time.deltaTime);

        if (Time.time - time_spawned > 3f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
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
