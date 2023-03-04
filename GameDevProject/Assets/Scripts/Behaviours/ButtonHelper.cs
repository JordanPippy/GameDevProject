using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour{
    [SerializeField] private GameObject volumeButtonRef;
    [SerializeField] private GameObject musicButtonRef;
    [SerializeField] private Sprite volumeOff;
    [SerializeField] private Sprite volumeOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite musicOn;
    private Image volumeButtonImage;
    private Image musicButtonImage;

    public void Start(){
        if(!PlayerPrefs.HasKey("VolumeIndictor")){
            PlayerPrefs.SetInt("VolumeIndictor", 1);
        }
        if(!PlayerPrefs.HasKey("MusicIndictor")){
            PlayerPrefs.SetInt("MusicIndictor", 1);
        }
        PlayerPrefs.Save();
        volumeButtonImage=volumeButtonRef.GetComponent<Image>();
        musicButtonImage=musicButtonRef.GetComponent<Image>();
        UpdateMusicButton();
        UpdateVolumeButton();
    }

    public void NewGame(){
        SceneManager.LoadScene(1);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void VolumeClicked(){
        if(PlayerPrefs.GetInt("VolumeIndictor")==1){
            PlayerPrefs.SetInt("VolumeIndictor", 0);
        }else{
            PlayerPrefs.SetInt("VolumeIndictor", 1);
        }
        PlayerPrefs.Save();
        UpdateVolumeButton();
    }
    
    public void MusicClicked(){
        if(PlayerPrefs.GetInt("MusicIndictor")==1){
            PlayerPrefs.SetInt("MusicIndictor", 0);
        }else{
            PlayerPrefs.SetInt("MusicIndictor", 1);
        }
        PlayerPrefs.Save();
        UpdateMusicButton();
    }

    private void UpdateVolumeButton(){
        if(PlayerPrefs.GetInt("VolumeIndictor")==1){
            volumeButtonImage.sprite=volumeOn;
        }else{
            volumeButtonImage.sprite=volumeOff;
        }
    }

    private void UpdateMusicButton(){
        Debug.Log(PlayerPrefs.GetInt("MusicIndictor"));
        if(PlayerPrefs.GetInt("MusicIndictor")==1){
            musicButtonImage.sprite=musicOn;
        }else{
            musicButtonImage.sprite=musicOff;
        }
    }
}
