using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : GenericAIController
{
    private Vector2 targetPosition;

    private float time = 0.0f;
    private float abilityTime = 0.8f;

    private float update_pos_time = 0.0f;
    public bool run_away_reached = true;

    // Start is called before the first frame update
    void Start()
    {
        AIStart();
        targetPosition = ChasePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        AIUpdate();
        Debug.DrawLine(rb2D.position, targetPosition, Color.green, 0, false);

        time += Time.deltaTime;

        if (time >= abilityTime)
        {
            time -= abilityTime;
            AttackPlayer();
        }

        if ((Time.time - update_pos_time) > 0.1f && run_away_reached)
        {
            targetPosition = ChasePlayer();

            if (targetPosition == new Vector2(999999f, 999999f))
            {
                targetPosition = MoveRandomPosition(5);
                run_away_reached = false;
            }
        }
    }

    public void FixedUpdate()
    {
        if (Vector3.Distance(rb2D.position, targetPosition) >= 0.1f)
        {
            MoveTowards(targetPosition);
        }
        else
        {
            targetPosition = ChasePlayer();

            if (!run_away_reached)
            {
                run_away_reached = true;
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == player)
        {
            targetPosition = MoveAwayFromPlayer(10);
            player.GetComponent<PlayerController>().TakeDamage(2);

            run_away_reached = false;
        }
    }
}

