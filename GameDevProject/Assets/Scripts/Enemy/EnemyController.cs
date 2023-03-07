using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 10;
    public float speed = 1;

    public GameObject spell;

    public GameObject player;

    private Vector2 targetPosition;

    private float time = 0.0f;
    private float abilityTime = 5.0f;

    private Rigidbody2D rb2D;
    private BoxCollider2D bc2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        bc2D = gameObject.GetComponent<BoxCollider2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        SetNewTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2D.position, targetPosition, Color.green, 0, false);

        time += Time.deltaTime;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position, targetPosition) >= 0.001f)
        {
            rb2D.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
        }
        else
        {
            SetNewTargetPosition();
        }


        if (time >= abilityTime)
        {
            time -= abilityTime;
            AttackPlayer();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    private void AttackPlayer()
    {
        float angle = Library.MousePlayerAngle(player.transform.position, transform.position);
            
        spell.GetComponent<Ability>().ability.Spawn(transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void SetNewTargetPosition()
    {
        bool found_valid_pos = false;
        int iters = 0;

        while (!found_valid_pos)
        {
            iters += 1;

            if (iters > 1000) {
                print("could not find");
                break;
            }
            found_valid_pos = true;

            targetPosition = new Vector2(
                Random.Range(rb2D.position.x-10, rb2D.position.x+10), 
                Random.Range(rb2D.position.y-10, rb2D.position.y+10)
            );

            // targetPosition = new Vector2(-7f, -7f);

            // print(targetPosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(
                rb2D.position, 
                (targetPosition - rb2D.position).normalized, 
                Vector2.Distance(rb2D.position, targetPosition)
            );

            //Debug.DrawRay(rb2D.position, (targetPosition - rb2D.position).normalized, Color.red, 10, false);
            Debug.DrawLine(rb2D.position, targetPosition, Color.red, 10, false);

            for (int j = 0; j < hits.Length; j++) {
                if (hits[j].collider != bc2D && hits[j].collider != null) {
                    print(hits[j].transform.name);
                    found_valid_pos = false;
                    break;
                }
            }
        }
    }
}