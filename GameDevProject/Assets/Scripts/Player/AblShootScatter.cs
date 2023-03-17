using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AblShootScatter : Ability
{
    public int projectileTotal = 2;
    public float scatterAngle = 90f;
    public AbilityBase projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        bool isEven = false;
        // Adjust for even parity
        if (projectileTotal % 2 == 0)
        {
            i++;
            projectileTotal++;
            isEven = true;
        }
        for (; i < projectileTotal; i ++)
        {
            Vector3 angle;
            // Adjust spread based on parity
            if (isEven)
                angle = new Vector3(0, 0, Mathf.Ceil(i/2f) * scatterAngle - scatterAngle/2);
            else
                angle = new Vector3(0, 0, Mathf.Ceil(i/2f) * scatterAngle);
            // Alternate +/- for above/below
            if (i % 2 == 0)
                angle.z = -angle.z;
            projectilePrefab.Spawn(transform.position, Quaternion.Euler(transform.rotation.eulerAngles + angle));
        }
        Destroy(gameObject);
    }
}
