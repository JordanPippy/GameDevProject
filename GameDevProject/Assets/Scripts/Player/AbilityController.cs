using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public Ability ability;
    private List<Ability> abilities = new List<Ability>();
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CastAbility();
        }
    }

    private void CastAbility()
    {
        GameObject spell;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        float angle = Mathf.Atan((mousePos.y - transform.position.y) / (mousePos.x - transform.position.x)) * Mathf.Rad2Deg;
        if (mousePos.x > transform.position.x)
            angle += 180;
            
        spell = Instantiate(ability.prefab, mousePos, Quaternion.Euler(0, 0, angle));
        Destroy(spell, 5);

        spell.GetComponent<Rigidbody2D>().AddForce(VectorFromAngle(angle * Mathf.Deg2Rad) * -500.0f);
    }

    public Vector2 VectorFromAngle (float theta) 
    {
        return new Vector2 (Mathf.Cos(theta), Mathf.Sin(theta)); // Trig is fun
    }
}
