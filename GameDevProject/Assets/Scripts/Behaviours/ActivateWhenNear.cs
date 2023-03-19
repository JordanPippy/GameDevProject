using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enables target behaviour(s) when the player is near.
/// </summary>
public class ActivateWhenNear : MonoBehaviour
{
    public List<Behaviour> targets;
    public bool startInactive;
    public float checkTime = 1f;
    public float detectionRange = 10f;

    private PlayerController player;
    private float checkTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (startInactive)
            foreach (Behaviour target in targets)
            {
                target.enabled = false;
            }
    }
    // Update is called once per frame
    void Update()
    {
        // A collider trigger would make more sense but this distance check works fine
        if (checkTimer > checkTime)
        {
            checkTime = 0;
            if (Vector3.Distance(player.transform.position,transform.position) < detectionRange)
            {
                foreach (Behaviour target in targets)
                {
                    target.enabled = true;
                }
                this.enabled = false;
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
