using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public GameObject ability;
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
        GameObject spell;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        float angle = Library.MousePlayerAngle(mousePos, transform.position);
        if (mousePos.x > transform.position.x)
            angle += 180;
            
        spell = Instantiate(ability, transform.position, Quaternion.Euler(0, 0, angle));
        Destroy(spell, 5);

        //spell.GetComponent<Rigidbody2D>().AddForce(Library.VectorFromAngle(angle * Mathf.Deg2Rad) * -500.0f);
    }
}
