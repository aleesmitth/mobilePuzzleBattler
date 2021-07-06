using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float health;
    private void OnEnable() {
        EventManager.onAttackAllEnemies += SufferDamage;
    }

    public void SufferDamage(float damage) {
        var healthBeforeAttach = health;
        health -= damage;
        Debug.Log("auch, me atacaron por (" + damage + ") tenia (" + healthBeforeAttach + ") vida y ahora (" + health + ")");
        if (health <= 0) {
            EventManager.OnEnemyDefeated(this);
            Destroy(gameObject);
        }
    }

    public float CurrentHealth() {
        return health;
    }
}
