using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AblShootNormal : Ability
{
    public float speed = 1.0f;
    private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 angleVector = -Library.VectorFromAngle(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
        movement = (new Vector3(angleVector.x, angleVector.y, 0)).normalized;
        transform.position += movement;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (movement * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        ability.Use(col.gameObject);
        if (col.tag != this.tag)
        {
            Destroy(gameObject);
        }
    }
}
