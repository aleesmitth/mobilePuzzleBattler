using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float health;

    public void SufferDamage(float damage) {
        var healthBeforeAttach = health;
        health -= damage;
        Debug.Log("auch, me atacaron por (" + damage + ") tenia (" + healthBeforeAttach + ") vida y ahora (" + health + ")");
        if (health <= 0)
            EventManager.OnEnemyDefeated(this);
    }

    public float CurrentHealth() {
        return health;
    }

    private void Start() {
        transform.GetComponent<SpriteRenderer>().sprite = GameContextData.EnemyData.sprite;
    }
}
