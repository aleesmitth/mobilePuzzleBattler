using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightStarter : MonoBehaviour {
    private EnemyData enemyData;
    private bool enemyReadyToFight;

    private void OnEnable() {
        enemyReadyToFight = false;
    }

    public void LoadEnemyData(EnemyData enemyData) {
        this.enemyData = enemyData;
        transform.GetComponent<SpriteRenderer>().sprite = enemyData.sprite;
        ResizeBoxCollider();
        this.enemyReadyToFight = true;
    }

    private void ResizeBoxCollider() {
        Vector2 size = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = size;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, size.y / 2);
    }

    private void OnMouseDown() {
        if (!enemyReadyToFight) return;
        Debug.Log("Started fight by clicking enemy");
        GameContextData.EnemyData = enemyData;
        EventManager.OnCollisionWithEnemy();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!enemyReadyToFight) return;
        if (!other.CompareTag("Player")) return;
        Debug.Log("Started fight by colliding with enemy");
        GameContextData.EnemyData = enemyData;
        EventManager.OnCollisionWithEnemy();
    }
}
