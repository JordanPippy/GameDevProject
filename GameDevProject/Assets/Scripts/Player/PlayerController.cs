using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // Public Variables
    public float speed = 10;
    public GameObject spell;
    public GameObject mindSwap;
    public int health = 20;
    public HealthBar healthBar;

    // Private Variables
    private float horizontal, vertical;
    private Rigidbody2D rb2D;
    private UIUpdater uiUpdater;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        uiUpdater = GetComponent<UIUpdater>();
        uiUpdater.UpdateAbilityUI(spell.GetComponent<Ability>().ability.UIicon);
        healthBar.SetMaxHealth(health);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("Canvas"));
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
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb2D.position += (movement * speed * Time.deltaTime);
    }

    private void CastAbility()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Library.MousePlayerAngle(mousePos, transform.position);
            
        spell.GetComponent<Ability>().ability.Spawn(transform.position, Quaternion.Euler(0, 0, angle));

        uiUpdater.abilityCooldown();

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0){
            Destroy(gameObject);
            // Go back to main menu
            SceneManager.LoadScene(0);
            // Delete all persistent player data
        }
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
                uiUpdater.UpdateAbilityUI(spell.GetComponent<SpriteRenderer>().sprite);
                uiUpdater.mindswapCooldown();
                healthBar.SetMaxHealth(health);
                healthBar.SetHealth(health);
            }
        }
    }
}
