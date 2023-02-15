using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Public Variables
    public float speed = 10;
    public GameObject spell;
    public GameObject mindSwap;
    public float health = 20;

    // Private Variables
    private float horizontal, vertical;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CastAbility();
        }

        
        if (Input.GetMouseButtonDown(0)) 
        {  
            DetectMindSwap(); 
        }  
    }  

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal, vertical, 0).normalized;

        transform.position += (movement * speed * Time.deltaTime);

    }

    private void CastAbility()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Library.MousePlayerAngle(mousePos, transform.position);
            
        spell.GetComponent<Ability>().ability.Spawn(transform.position, Quaternion.Euler(0, 0, angle));

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    private void DetectMindSwap()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null) 
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                ((MindSwap)mindSwap.GetComponent<Ability>().ability).Spawn(hit.collider.gameObject);
            }
        }
    }
}
