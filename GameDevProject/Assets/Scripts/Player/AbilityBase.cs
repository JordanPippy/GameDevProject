using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability", order = 0)]
public class AbilityBase : ScriptableObject
{
    public string title;
    public int damage;
    public int cost;
    public GameObject prefab;

    public void Use(GameObject other)
    {
        Debug.Log("Called Use from scriptableObject");
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject clone = Instantiate(prefab, position, rotation);
        Destroy(clone, 5);
    }
}
