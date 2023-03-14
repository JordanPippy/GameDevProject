using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float health = 10;
    public float speed = 1;

    public GameObject spell;
    private GameObject player;

    private Vector2 targetPosition;

    private float time = 0.0f;
    private float abilityTime = 2.0f;

    private Rigidbody2D rb2D;
    private BoxCollider2D bc2D;

    public BehaviourDelegate behaviour;

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

        behaviour = new BehaviourDelegate(ChasePlayer);
        behaviour();
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
            //AttackPlayer();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    //TODO: make a short range melee spell and use that instead of damaging player via collision2d
    // private void AttackPlayer()
    // {
    //     float angle = Library.MousePlayerAngle(player.transform.position, transform.position);
            
    //     spell.GetComponent<Ability>().ability.Spawn(transform.position, Quaternion.Euler(0, 0, angle));
    // }

    public void FixedUpdate()
    {
        if (Vector3.Distance(rb2D.position, targetPosition) >= 0.1f)
        {
            rb2D.MovePosition(Vector3.MoveTowards(rb2D.position, targetPosition, speed * Time.deltaTime));
        }
        else
        {
            behaviour();
        }
    }

    private void MoveRandomPosition()
    {
        int iters = 0;

        targetPosition = new Vector2(
            Random.Range(rb2D.position.x-5, rb2D.position.x+5), 
            Random.Range(rb2D.position.y-5, rb2D.position.y+5)
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
                Random.Range(rb2D.position.x-5, rb2D.position.x+5), 
                Random.Range(rb2D.position.y-5, rb2D.position.y+5)
            );
        }
    }

    private void MoveAwayFromPlayer()
    {
        int iters = 0;

        //TODO: store this so as to not look it up every time
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        Vector2 oppositePlayerDir = (rb2D.position - playerRB.position).normalized;
        
        //add some varation to his movement
        oppositePlayerDir.x = oppositePlayerDir.x + Random.Range(-0.3f, 0.3f);
        oppositePlayerDir.y = oppositePlayerDir.y + Random.Range(-0.3f, 0.3f);

        print(oppositePlayerDir);

        targetPosition = rb2D.position + oppositePlayerDir * 5f;

        while (!isValidPos(targetPosition))
        {
            iters += 1;

            //make sure the game doesnt get stuck in a loop
            //if a pos can't be found
            if (iters > 100) {
                print("could not find");
                MoveRandomPosition();
                break;
            }

            targetPosition = rb2D.position + oppositePlayerDir * 5f;
        }
    }

    private void ChasePlayer()
    {
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();

        Vector2 playerDir = (playerRB.position - rb2D.position).normalized;

        if (Vector2.Distance(rb2D.position, playerRB.position) > 4)
        {
            targetPosition = playerRB.position - (playerDir * 2f);
            Debug.DrawLine(rb2D.position, targetPosition, Color.blue, 10, false);

            if (!isValidPos(targetPosition))
            {
                MoveRandomPosition();
            }
        } else {
            targetPosition = playerRB.position;
        }
    }

    //check if a given position is valid for the ai to move to
    //this code is still ugly and could be more efficient
    private bool isValidPos(Vector2 targetPosition)
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
            if (hits[j].collider != bc2D && hits[j].collider != null) 
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
            Vector2 offset = new Vector2(0f, 0f);
            Vector2 step = (targetPosition - rb2D.position).normalized * 0.35f;

            while (Vector2.Distance(rb2D.position+offset, targetPosition) > 0.7f) 
            {
                Collider2D result = Physics2D.OverlapCircle(rb2D.position+offset, 0.1f);
                
                if (result != bc2D && result != null)
                {
                    print("too small");
                    print(result.transform.name);
                    is_valid_pos = false;
                    //Debug.DrawRay(rb2D.position, rb2D.position+offset, Color.blue, 10);
                    Debug.DrawLine(rb2D.position, rb2D.position+offset, Color.blue, 10, false);
                    break;
                }

                offset += step;
            }
        }

        //finally, if theres no wall between us and the route is wide enough,
        //check if the final position is wide enough for the character to stand in
        if (is_valid_pos)
        {
            if (Physics2D.OverlapCircle(new Vector2(targetPosition.x, targetPosition.y - 0.7f), 0.875f/2f) != null) {
                print("Occupied");
                is_valid_pos = false;
            }

            if (Physics2D.OverlapCircle(new Vector2(targetPosition.x, targetPosition.y + 0.7f), 0.875f/2f) != null) {
                print("Occupied");
                is_valid_pos = false;
            }
        }

        return is_valid_pos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == player)
        {
            MoveRandomPosition();
            player.GetComponent<PlayerController>().TakeDamage(3);
        }
    }
}