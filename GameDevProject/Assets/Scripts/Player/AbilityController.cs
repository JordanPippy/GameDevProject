using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public GameObject spell;
    private List<AbilityBase> abilities = new List<AbilityBase>();
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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Library.MousePlayerAngle(mousePos, transform.position);
            
        //spell = Instantiate(ability, transform.position, Quaternion.Euler(0, 0, angle));
        spell.GetComponent<Ability>().ability.Spawn(transform.position, Quaternion.Euler(0, 0, angle));

        //spell.GetComponent<Rigidbody2D>().AddForce(Library.VectorFromAngle(angle * Mathf.Deg2Rad) * -500.0f);
    }
}
