using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Abilities/Projectile", order = 0)]
public class Projectile : AbilityBase
{
    public override void Use(GameObject other)
    {
        Library.StandardDamage(other, damage);
    }
}
