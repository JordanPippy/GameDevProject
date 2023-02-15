using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Library
{
    public static Vector2 VectorFromAngle(float theta) 
    {
        return new Vector2 (Mathf.Cos(theta), Mathf.Sin(theta)); // Trig is fun
    }

    public static float MousePlayerAngle(Vector3 mouse, Vector3 player)
    {
        float angle = Mathf.Atan((mouse.y - player.y) / (mouse.x - player.x)) * Mathf.Rad2Deg;
        if(mouse.x > player.x){
            angle += 180;
        }
        return angle;
    }

    public static float RandomWithinRange(float offest, float range)
    {
        return Random.Range(offest - range, offest + range);
    }

    public static void StandardDamage(GameObject other, int damage)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }
}
