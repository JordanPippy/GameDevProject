using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MindSwap", menuName = "Abilities/MindSwap", order = 0)]
public class MindSwap : AbilityBase
{
    public override void Use(GameObject other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        PlayerController player = enemy.player.GetComponent<PlayerController>();

        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();

        GameObject tempSpell;

        Vector3 tempPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        player.transform.position = enemy.transform.position;
        enemy.transform.position = tempPos;

        Sprite temp = playerRenderer.sprite;
        playerRenderer.sprite = enemyRenderer.sprite;
        enemyRenderer.sprite = temp;

        int tempHealth = player.health;
        player.health = enemy.health;
        enemy.health = tempHealth;

        // float tempSpeed = speed;
        // speed = enemy.speed;
        // enemy.speed = tempSpeed;

        tempSpell = player.spell;
        player.spell = enemy.spell;
        enemy.spell = tempSpell;
    }

    public void Spawn(GameObject other)
    {
        GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity); 
        Use(other);
        Destroy(clone);
    }
}
