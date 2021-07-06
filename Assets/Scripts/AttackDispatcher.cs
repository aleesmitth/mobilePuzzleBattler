using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDispatcher : MonoBehaviour {
    public Enemy[] enemies;
    private void OnEnable() {
        EventManager.onAttackOneEnemy += AttackLowestHealthEnemy;
        EventManager.onEnemyDefeated += DeleteEnemyData;
    }

    private void DeleteEnemyData(Enemy enemyDefeated) {
        Enemy[] enemiesUpdated = new Enemy[enemies.Length - 1];
        int j = 0;
        foreach (var enemy in enemies) {
            if (enemyDefeated == enemy)
                continue;
            enemiesUpdated[j] = enemy;
            j++;
        }
        
        enemies = enemiesUpdated;
        Debug.Log("se murio un enemigo, quedan " + enemies.Length);
    }

    private void AttackLowestHealthEnemy(float damage) {
        if (enemies.Length == 0) return;
        var lowestEnemy = enemies[0];
        for (int i = 1; i < enemies.Length; i++) {
            if (enemies[i].CurrentHealth() < lowestEnemy.CurrentHealth())
                lowestEnemy = enemies[i];
        }
        lowestEnemy.SufferDamage(damage);
    }
}
