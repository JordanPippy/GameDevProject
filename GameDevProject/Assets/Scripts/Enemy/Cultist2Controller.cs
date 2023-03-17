using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist2Controller : GenericAIController
{
    private Vector2 targetPosition;

    private float time = 0.0f;
    private float abilityTime = 2.0f;
    private float update_pos_time;

    // Start is called before the first frame update
    void Start()
    {
        AIStart();
        targetPosition = MoveAwayFromPlayer(10);
        update_pos_time = Time.time;
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

        if ((Time.time - update_pos_time) > 0.8f)
        {
            if (Vector2.Distance(rb2D.position, playerRB.position) < 8)
            {
                targetPosition = MoveAwayFromPlayer(10);
                update_pos_time = Time.time;
            } else {
                targetPosition = rb2D.position;
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
            if (Vector2.Distance(rb2D.position, playerRB.position) < 8)
            {
                targetPosition = MoveAwayFromPlayer(10);
                update_pos_time = Time.time;
            }
        }
    }
}