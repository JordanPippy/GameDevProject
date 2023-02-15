using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 10;
    public float speed = 1;

    public GameObject spell;

    public GameObject player;

    private Vector3 targetPosition;

    private float time = 0.0f;
    private float abilityTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        SetNewTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position, targetPosition) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
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
       targetPosition = new Vector3(Library.RandomWithinRange(transform.position.x, 5), Library.RandomWithinRange(transform.position.y, 5), transform.position.z);
    }
}
