using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAIController: MonoBehaviour
{
    public int maxHealth = 10;
    public int health = 0;
    public float speed = 1;
    public bool swappable = true;

    public GameObject spell;
    public GameObject player;

    public Rigidbody2D playerRB;
    public Collider2D playerCol2D;

    public Rigidbody2D rb2D;
    public Collider2D col2D;

    public void AIStart()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        playerRB = player.GetComponent<Rigidbody2D>();
        playerCol2D = player.GetComponent<Collider2D>();

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        col2D = gameObject.GetComponent<Collider2D>();
        health = maxHealth;
    }

    public void AIUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void MoveTowards(Vector2 pos)
    {
        rb2D.MovePosition(Vector3.MoveTowards(rb2D.position, pos, speed * Time.deltaTime));
    }

    public Vector2 MoveRandomPosition(int range)
    {
        int iters = 0;

        Vector2 targetPosition = new Vector2(
            Random.Range(rb2D.position.x-range, rb2D.position.x+range), 
            Random.Range(rb2D.position.y-range, rb2D.position.y+range)
        );

        while (!isValidPos(targetPosition))
        {
            iters += 1;

            //make sure the game doesnt get stuck in a loop
            //if a pos can't be found
            if (iters > 100) {
                print("could not find");
                break;
            }

            targetPosition = new Vector2(
                Random.Range(rb2D.position.x-range, rb2D.position.x+range), 
                Random.Range(rb2D.position.y-range, rb2D.position.y+range)
            );
        }

        return targetPosition;
    }

    public Vector2 MoveAwayFromPlayer(float dist)
    {
        int iters = 0;

        //TODO: store this so as to not look it up every time
        Vector2 oppositePlayerDir = (rb2D.position - playerRB.position).normalized;
        
        //add some varation to his movement
        oppositePlayerDir.x = oppositePlayerDir.x + Random.Range(-0.3f, 0.3f);
        oppositePlayerDir.y = oppositePlayerDir.y + Random.Range(-0.3f, 0.3f);

        Vector2 targetPosition = rb2D.position;
        Vector2 increment = oppositePlayerDir * 2f;

        while (isValidPos(targetPosition) && Vector2.Distance(playerRB.position, targetPosition) <= dist)
        {
            print(Vector2.Distance(playerRB.position, targetPosition));
            iters += 1;

            //make sure the game doesnt get stuck in a loop
            //if a pos can't be found
            //set to ten for cultist 2, if dif otions needed then add a param
            if (iters > 10) {
                print("could not find");
                targetPosition = MoveRandomPosition(5);
                break;
            }

            targetPosition = targetPosition + increment;
        }
        print("break" + Vector2.Distance(playerRB.position, targetPosition));

        targetPosition = targetPosition - increment;

        return targetPosition;
    }

    public Vector2 ChasePlayer()
    {
        Vector2 playerDir = (playerRB.position - rb2D.position).normalized;

        Vector2 targetPosition = playerRB.position - (playerDir * 1.5f);

        if (Vector2.Distance(rb2D.position, playerRB.position) > 4)
        {
            targetPosition = playerRB.position - (playerDir * 1.5f);

            if (!isValidPos(targetPosition))
            {
                return new Vector2(999999f, 999999f);
            }
        } else {
            targetPosition = playerRB.position;
        }

        return targetPosition;
    }

    //check if a given position is valid for the ai to move to
    //this code is still ugly and could be more efficient
    public bool isValidPos(Vector2 targetPosition)
    {
        bool is_valid_pos = true;

        //first check if their is a wall between ourselves and the position chosen
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            rb2D.position, 
            (targetPosition - rb2D.position).normalized, 
            Vector2.Distance(rb2D.position, targetPosition)
        );

        for (int j = 0; j < hits.Length; j++) 
        {
            if (hits[j].collider != col2D && hits[j].collider != null && !hits[j].collider.isTrigger) 
            {
                is_valid_pos = false;
                Debug.DrawLine(rb2D.position, targetPosition, Color.red, 10, false);
                break;
            }
        }

        //if the position is still valid, check if the route is wide enough
        //for the character to pass
        if (is_valid_pos) 
        {
            RaycastHit2D[] hits2 = new RaycastHit2D[10];

            int total = col2D.Cast((targetPosition - rb2D.position).normalized, hits2, Vector2.Distance(rb2D.position, targetPosition));

            for (int i = 0; i < total; i ++)
            {
                if (hits2[i] != null && 
                    hits2[i].collider != playerCol2D &&
                    !hits2[i].collider.isTrigger
                )
                {
                    //print("name " + hits2[i].collider.transform.name);
                    is_valid_pos = false;
                }
            }

            // Vector2 offset = new Vector2(0f, 0f);
            // Vector2 step = (targetPosition - rb2D.position).normalized * 0.35f;

            // while (Vector2.Distance(rb2D.position+offset, targetPosition) > 0.7f) 
            // {
            //     Collider2D result = Physics2D.OverlapCircle(rb2D.position+offset, 0.1f);
                
            //     if (result != col2D && result != null)
            //     {
            //         print("too small");
            //         print(result.transform.name);
            //         is_valid_pos = false;
            //         //Debug.DrawRay(rb2D.position, rb2D.position+offset, Color.blue, 10);
            //         Debug.DrawLine(rb2D.position, rb2D.position+offset, Color.blue, 10, false);
            //         break;
            //     }

            //     offset += step;
            // }
        }

        // //finally, if theres no wall between us and the route is wide enough,
        // //check if the final position is wide enough for the character to stand in
        // if (is_valid_pos)
        // {
        //     Collider2D col_clone = Object.Instantiate(col2D);
        //     col_clone.transform = targetPosition;

        //     if (col_clone.OverlapCollider(new ContactFilter2D().NoFilter(), new List<Collider2D>()) > 0) {
        //         print("Occupied");
        //         is_valid_pos = false;
        //     }
        // }

        return is_valid_pos;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    public void AttackPlayer()
    {
        float angle = Library.MousePlayerAngle(player.transform.position, transform.position);
            
        spell.GetComponent<Ability>().ability.Spawn(transform.position, Quaternion.Euler(0, 0, angle), tag);
    }
}