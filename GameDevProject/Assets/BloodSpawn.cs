using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpawn : MonoBehaviour
{
    SpriteRenderer srenderer;

    // Start is called before the first frame update
    void Start()
    {
        srenderer = GetComponent<SpriteRenderer>();
        srenderer.flipX = Random.value > 0.5f;
        float brightness = Random.Range(0.6f, 1.0f);
        srenderer.color = new Color(brightness, brightness, brightness);

        transform.eulerAngles = Vector3.forward * Random.Range(0, 360);
        transform.localScale = Vector2.one * Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
