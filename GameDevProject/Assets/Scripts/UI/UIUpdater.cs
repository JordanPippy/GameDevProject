using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public Image mindswapImage, abilityImage;

    private float abilityWait = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        abilityImage.fillAmount += 1.0f / abilityWait * Time.deltaTime;
        mindswapImage.fillAmount += 1.0f / abilityWait * Time.deltaTime;
    }

    public void UpdateAbilityUI(Sprite abilitySprite)
    {
        abilityImage.GetComponent<Image>().sprite = abilitySprite;
    }

    public void abilityCooldown()
    {
        abilityImage.fillAmount = 0;
    }

    public void mindswapCooldown()
    {
        mindswapImage.fillAmount = 0;
    }
}
