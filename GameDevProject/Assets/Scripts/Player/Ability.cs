using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public AbilityBase ability;
    public float speed = 1.0f;
    public Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 angleVector = Library.VectorFromAngle(transform.rotation.eulerAngles.z);
        movement = (new Vector3(angleVector.x, angleVector.y, 0) - transform.position).normalized;
        transform.position += movement;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (movement * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        ability.Use(col.gameObject);
        Destroy(gameObject);
    }
}
