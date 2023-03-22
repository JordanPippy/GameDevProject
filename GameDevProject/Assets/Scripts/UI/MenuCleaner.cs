using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCleaner : MonoBehaviour
{
    AudioSource menuMusic;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Canvas"));
        menuMusic=gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("MusicIndictor")==0){
            menuMusic.mute=true;
        }else{
            menuMusic.mute=false;
        }
        
    }
}
