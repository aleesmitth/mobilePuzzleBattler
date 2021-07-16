using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackDispatcher : MonoBehaviour {
    public Enemy[] enemies;
    private void OnEnable() {
        EventManager.onAttackOneEnemy += AttackLowestHealthEnemy;
        EventManager.onEnemyDefeated += DeleteEnemyData;
    }
    
    private void OnDisable() {
        EventManager.onAttackOneEnemy -= AttackLowestHealthEnemy;
        EventManager.onEnemyDefeated -= DeleteEnemyData;
    }

    private void DeleteEnemyData(Enemy enemyDefeated) {
        //linq expression to keep every enemy != enemyDefeated
        enemies = enemies.Where(enemy => enemy != enemyDefeated).ToArray();
        //placeholder, el attack dispatcher no deberia controlar la destruccion de enemigo
        Destroy(enemyDefeated.gameObject);
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
