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
        return Mathf.Atan((mouse.y - player.y) / (mouse.x - player.x)) * Mathf.Rad2Deg;
    }
}
