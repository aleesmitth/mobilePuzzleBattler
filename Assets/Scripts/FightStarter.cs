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
        this.enemyReadyToFight = true;
    }
    private void OnMouseDown() {
        if (!enemyReadyToFight) return;
        Debug.Log("Started fight by clicking enemy");
        EventManager.OnCollisionWithEnemy(this.enemyData);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!enemyReadyToFight) return;
        if (!other.CompareTag("Player")) return;
        Debug.Log("Started fight by colliding with enemy");
        EventManager.OnCollisionWithEnemy(this.enemyData);
    }
}
