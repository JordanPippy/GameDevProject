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
    private float abilityTime = 2.0f;

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


        if (time >= abilityTime)
        {
            time -= abilityTime;
            AttackPlayer();
            //SetNewTargetPosition();
        }

        if (Input.GetKeyDown("a"))
        {
            SetNewTargetPosition();
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

    public void FixedUpdate()
    {
        if (Vector3.Distance(rb2D.position, targetPosition) >= 0.1f)
        {
            rb2D.MovePosition(Vector3.MoveTowards(rb2D.position, targetPosition, speed * Time.deltaTime));
        }
        else
        {
            SetNewTargetPosition();
        }
    }

    private void SetNewTargetPosition()
    {
        bool found_valid_pos = false;
        int iters = 0;

        while (!found_valid_pos)
        {
            iters += 1;

            if (iters > 100) {
                print("could not find");
                //targetPosition = new Vector2(rb2D.position.x, rb2D.position.y-10);
                break;
            }
            
            found_valid_pos = true;

            targetPosition = new Vector2(
                Random.Range(rb2D.position.x-5, rb2D.position.x+5), 
                Random.Range(rb2D.position.y-5, rb2D.position.y+5)
            );

            //targetPosition = new Vector2(rb2D.position.x, rb2D.position.y+10);

            //targetPosition = new Vector2(-8f, -2f);

            // print(targetPosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(
                rb2D.position, 
                (targetPosition - rb2D.position).normalized, 
                Vector2.Distance(rb2D.position, targetPosition)
            );

            //Debug.DrawRay(rb2D.position, (targetPosition - rb2D.position).normalized, Color.red, 10, false);s

            for (int j = 0; j < hits.Length; j++) {
                if (hits[j].collider != bc2D && hits[j].collider != null) {
                    found_valid_pos = false;
                    Debug.DrawLine(rb2D.position, targetPosition, Color.red, 10, false);
                    break;
                }
            }

            //if the position is still valid, check if the route is wide enough
            if (found_valid_pos) 
            {
                Vector2 offset = new Vector2(0f, 0f);
                Vector2 step = (targetPosition - rb2D.position).normalized * 0.35f;

                while (Vector2.Distance(rb2D.position+offset, targetPosition) > 0.7f) {
                    Collider2D result = Physics2D.OverlapCircle(rb2D.position+offset, 0.1f);
                    
                    if (result != bc2D && result != null) {
                        print("too small");
                        print(result.transform.name);
                        found_valid_pos = false;
                        //Debug.DrawRay(rb2D.position, rb2D.position+offset, Color.blue, 10);
                        Debug.DrawLine(rb2D.position, rb2D.position+offset, Color.blue, 10, false);
                        break;
                    }

                    //print(rb2D.position+offset);
                    //print(rb2D.position);
                    //print(offset);

                    offset += step;
                    //print(offset);
                    //print(step);
                }
            }

            if (found_valid_pos)
            {
                if (Physics2D.OverlapCircle(new Vector2(targetPosition.x, targetPosition.y - 0.7f), 0.875f/2f) != null) {
                    print("Occupied");
                    found_valid_pos = false;
                }

                if (Physics2D.OverlapCircle(new Vector2(targetPosition.x, targetPosition.y + 0.7f), 0.875f/2f) != null) {
                    print("Occupied");
                    found_valid_pos = false;
                }

            }
        }
    }
}