using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    public string title;
    public int damage;
    public float cooldown;
    public GameObject prefab;
    public Sprite UIicon;

    public abstract void Use(GameObject other);

    public virtual void Spawn(Vector3 position, Quaternion rotation, string tag)
    {
        GameObject clone = Instantiate(prefab, position, rotation);
        clone.tag = tag;
        Destroy(clone, 5);
    }
}
