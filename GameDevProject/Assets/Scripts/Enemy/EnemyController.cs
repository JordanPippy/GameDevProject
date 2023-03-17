using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : GenericAIController
{
    private Vector2 targetPosition;

    private float time = 0.0f;
    private float abilityTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        AIStart();
        targetPosition = MoveRandomPosition(5);
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
    }

    public void FixedUpdate()
    {
        if (Vector3.Distance(rb2D.position, targetPosition) >= 0.1f)
        {
            MoveTowards(targetPosition);
        }
        else
        {
            targetPosition = MoveRandomPosition(5);
        }
    }
}