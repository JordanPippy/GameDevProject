using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Boss0Controller : GenericAIController
{
    private Vector2 targetPosition;

    private float time = 0.0f;
    private float abilityTime = 0.8f;
    private float update_pos_time;
    //private float healVal=0;
    public GameObject boss1, boss2, boss3;
    public TMP_FontAsset orbitron;
    float boss1Dist=1000;
    float boss2Dist=1000;
    float boss3Dist=1000;
    private bool endFlag=true;

    // Start is called before the first frame update
    void Start()
    {
        AIStart();
        targetPosition = MoveRandomPosition(10);
        update_pos_time = Time.time;
        swappable = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Boss will heal 1 health/sec
        //healVal+=Time.deltaTime;
        //if(healVal>1){
        //    healVal-=1;
        //    if(health<maxHealth){
        //        health+=1;
        //    }
        //}
        // It was too hard

        // If below 1/5 health, pick closest existing body to take and destroy old one
        if(health<maxHealth/5){
            if(boss1!=null){
                boss1Dist=Vector3.Distance(boss1.transform.position, transform.position);
            }
            if(boss2!=null){
                boss2Dist=Vector3.Distance(boss2.transform.position, transform.position);
            }
            if(boss3!=null){
                boss3Dist=Vector3.Distance(boss3.transform.position, transform.position);
            }
            if(boss1Dist<boss2Dist&&boss1Dist<boss3Dist&&boss1!=null){
                MindSwap(boss1);
                boss1=null;
                boss1Dist=1000;
            }else if(boss2Dist<boss1Dist&&boss2Dist<boss3Dist&&boss2!=null){
                MindSwap(boss2);
                boss2=null;
                boss2Dist=1000;
            }else if(boss3Dist<boss1Dist&&boss3Dist<boss2Dist&&boss3!=null){
                MindSwap(boss3);
                boss3=null;
                boss3Dist=1000;
            }else if(endFlag){
                endFlag=false;
                // Spaghetti code so it's as if the boss if gone but the coroutine runs
                gameObject.transform.position=new Vector3(1000, 1000, 0);
                // Win the game
                Debug.Log("You win");
                GameObject endDisplay=new GameObject("End Game", typeof(TextMeshPro));
                endDisplay.transform.position=GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width*0.5f, Screen.height*0.5f, 0));
                endDisplay.transform.position=new Vector3(endDisplay.transform.position.x, endDisplay.transform.position.y, 10);
                endDisplay.GetComponent<MeshRenderer>().sortingLayerName="Default";
                endDisplay.GetComponent<MeshRenderer>().sortingOrder=100;
                endDisplay.GetComponent<RectTransform>().sizeDelta=new Vector2(500f, 0.5f);
                endDisplay.GetComponent<TextMeshPro>().alignment=TextAlignmentOptions.Center;
                endDisplay.GetComponent<TextMeshPro>().fontSize=7;
                endDisplay.GetComponent<TextMeshPro>().font=orbitron;
                endDisplay.GetComponent<Renderer>().material=orbitron.material;
                endDisplay.GetComponent<TextMeshPro>().color=Color.green;
                endDisplay.GetComponent<TextMeshPro>().faceColor=Color.green;
                endDisplay.GetComponent<Renderer>().material.color=Color.green;
                endDisplay.GetComponent<TextMeshPro>().text="YOU HAVE DEFEATED THE IMMORTAL";
                StartCoroutine(Ender());
            }
        }
        AIUpdate();
        Debug.DrawLine(rb2D.position, targetPosition, Color.green, 0, false);

        time += Time.deltaTime;

        if (time >= abilityTime)
        {
            time -= abilityTime;
            AttackPlayer();
        }

        if ((Time.time - update_pos_time) > 0.3f)
        {
            if (Vector2.Distance(rb2D.position, playerRB.position) < 1)
            {
                targetPosition = MoveAwayFromPlayer(1);
                update_pos_time = Time.time;
            } else if (Vector2.Distance(rb2D.position, playerRB.position) > 4){
                targetPosition = ChasePlayer();
                update_pos_time = Time.time;
            } else {
                targetPosition = rb2D.position;
            }
        }
    }
    
    IEnumerator Ender(){
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }

    public void FixedUpdate()
    {
        if (Vector3.Distance(rb2D.position, targetPosition) >= 0.1f)
        {
            MoveTowards(targetPosition);
        }
        else
        {
            if (Vector2.Distance(rb2D.position, playerRB.position) < 8)
            {
                targetPosition = MoveAwayFromPlayer(10);
                update_pos_time = Time.time;
            }
        }
    }

    private void MindSwap(GameObject other)
    {
        GenericAIController otherBoss = other.GetComponent<GenericAIController>();
        GenericAIController thisBoss = gameObject.GetComponent<GenericAIController>();

        SpriteRenderer thisRenderer = gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer otherRenderer = otherBoss.GetComponent<SpriteRenderer>();

        GameObject tempSpell;

        Vector3 tempPos = new Vector3(thisBoss.transform.position.x, thisBoss.transform.position.y, thisBoss.transform.position.z);
        thisBoss.transform.position = otherBoss.transform.position;
        otherBoss.transform.position = tempPos;

        Sprite temp = thisRenderer.sprite;
        thisRenderer.sprite = otherRenderer.sprite;
        otherRenderer.sprite = temp;

        int tempHealth = thisBoss.health;
        thisBoss.health = otherBoss.health;
        otherBoss.health = tempHealth;

        int tempMaxHealth = thisBoss.maxHealth;
        thisBoss.maxHealth = otherBoss.maxHealth;
        otherBoss.maxHealth = tempMaxHealth;

        tempSpell = thisBoss.spell;
        thisBoss.spell = otherBoss.spell;
        otherBoss.spell = tempSpell;

        Vector2 tempCollider = thisBoss.GetComponent<BoxCollider2D>().size;
        thisBoss.GetComponent<BoxCollider2D>().size = otherBoss.GetComponent<BoxCollider2D>().size;
        otherBoss.GetComponent<BoxCollider2D>().size = tempCollider;
        
        Vector2 tempColliderOffset = thisBoss.GetComponent<BoxCollider2D>().offset;
        thisBoss.GetComponent<BoxCollider2D>().offset = otherBoss.GetComponent<BoxCollider2D>().offset;
        otherBoss.GetComponent<BoxCollider2D>().offset = tempColliderOffset;
    }

}