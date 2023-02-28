using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enables the target monobehaviour when the player is near.
/// </summary>
public class ActivateWhenNear : MonoBehaviour
{
    public MonoBehaviour target;
    public bool startInactive;
    public float checkTime = 5f;
    public float detectionRange = 5f;

    private PlayerController player;
    private float checkTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (startInactive)
            target.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (checkTimer > checkTime)
        {
            checkTime = 0;
            if (Vector3.Distance(player.transform.position,transform.position) < detectionRange)
            {
                target.enabled = true;
                this.enabled = false;
                //Debug.Log($"Player is near {gameObject.name}");
            }
        }
        else
        {
            checkTimer += Time.deltaTime;
        }
    }
    // Show detection range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
