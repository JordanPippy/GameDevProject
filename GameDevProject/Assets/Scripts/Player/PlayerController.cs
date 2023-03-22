using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // Public Variables
    public float speed = 10;
    public float playerSpeedBonus = 5;
    public GameObject spell;
    public GameObject mindSwap;
    public int health = 20;
    public int maxHealth;
    public HealthBar healthBar;
    public AudioClip cityMusic, labMusic, caveMusic;

    // Private Variables
    private float horizontal, vertical;
    private float spellCooldown, mindSwapCooldown;
    private float spellTimer, mindSwapTimer;
    private Rigidbody2D rb2D;
    private UIUpdater uiUpdater;
    private GameObject canvas;
    private int swapBonus=0;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("MusicIndictor")==1){
            gameObject.GetComponent<AudioSource>().clip=cityMusic;
        }
        gameObject.GetComponent<AudioSource>().Play();
        AbilityBase spellbase = spell.GetComponent<Ability>().ability;
        AbilityBase swapbase = mindSwap.GetComponent<Ability>().ability;
        spellCooldown = spellbase.cooldown;
        mindSwapCooldown = swapbase.cooldown;
        spellTimer = spellbase.cooldown;
        mindSwapTimer = swapbase.cooldown;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        uiUpdater = GetComponent<UIUpdater>();
        uiUpdater.UpdateAbilityUI(spell.GetComponent<Ability>().ability.UIicon);
        maxHealth = health;
        speed += playerSpeedBonus;
        healthBar=GameObject.Find("Health Bar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        uiUpdater.SetAbilityCooldown(spellCooldown);
        uiUpdater.SetMindswapCooldown(mindSwapCooldown);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("Canvas"));
    }

    // Update is called once per frame
    void Update()
    {
        if (spellTimer < spellCooldown) {
            spellTimer +=  Time.deltaTime;
        }
        if (mindSwapTimer < mindSwapCooldown) {
            mindSwapTimer +=  Time.deltaTime;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0) && spellTimer >= spellCooldown)
        {
            spellTimer = 0f;
            CastAbility();
        }

        // Don't even try to swap if we're on cooldown
        if (Input.GetMouseButtonDown(1) && mindSwapTimer >= mindSwapCooldown) 
        {  
            DetectMindSwap();
        }  
    }  

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb2D.position += (movement * speed * Time.deltaTime);
        // Music handling on player because I want it to be even more monolithic
        if(SceneManager.GetActiveScene().buildIndex==2){
            if(gameObject.GetComponent<AudioSource>().clip!=labMusic){
                gameObject.GetComponent<AudioSource>().clip=labMusic;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }else if(SceneManager.GetActiveScene().buildIndex==3){
            if(gameObject.GetComponent<AudioSource>().clip!=caveMusic){
                gameObject.GetComponent<AudioSource>().clip=caveMusic;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    private void CastAbility()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Library.MousePlayerAngle(mousePos, transform.position);
            
        spell.GetComponent<Ability>().ability.Spawn(transform.position, Quaternion.Euler(0, 0, angle), tag);

        uiUpdater.AbilityCooldown();

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0){
            //Destroy(gameObject);
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
                // Only put mindswap on cooldown when successful
                mindSwapTimer = 0f;
                ((MindSwap)mindSwap.GetComponent<Ability>().ability).Spawn(hit.collider.gameObject);
                uiUpdater.UpdateAbilityUI(spell.GetComponent<Ability>().ability.UIicon);
                uiUpdater.MindswapCooldown();
                spellCooldown = spell.GetComponent<Ability>().ability.cooldown;
                uiUpdater.SetAbilityCooldown(spellCooldown);
                // Permanent health bonus from swapping
                swapBonus+=2;
                maxHealth+=swapBonus;
                health+=swapBonus;
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(health);
            }
        }
    }
}
