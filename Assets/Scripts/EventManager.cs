using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager {
    public static event Action<Vector3, TextMeshProUGUI> onDeleteNode;
    public static event Action<Vector3> onSelectNode;
    public static event Action<Vector2> onNewEdgeWith;
    public static event Action<int[]> onHitsProcessed;
    public static event Action onNewNodesSpawned;
    public static event Action<LinkedList<LinkedList<ElementType>>> onNodesDestroyed;
    public static event Action<float> onAttackOneEnemy;
    public static event Action<Enemy> onEnemyDefeated;
    public static event Action onItemSelected;
    public static event Action<EnemyData> onCollisionWithEnemy;

    public static void OnDeleteNode(Vector3 position, TextMeshProUGUI textMeshProUGUI) {
        onDeleteNode?.Invoke(position, textMeshProUGUI);
    }
    
    public static void OnSelectNode(Vector3 position) {
        onSelectNode?.Invoke(position);
    }

    public static void OnNewEdgeWith(Vector2 position) {
        onNewEdgeWith?.Invoke(position);
    }

    public static void OnHitsProcessed(int[] hitsPerColumn) {
        onHitsProcessed?.Invoke(hitsPerColumn);
    }

    public static void OnNewNodesSpawned() {
        onNewNodesSpawned?.Invoke();
    }

    public static void OnNodesDestroyed(LinkedList<LinkedList<ElementType>> hits) {
        onNodesDestroyed?.Invoke(hits);
    }

    public static void OnAttackOneEnemy(float damage) {
        onAttackOneEnemy?.Invoke(damage);
    }

    public static void OnEnemyDefeated(Enemy enemy) {
        onEnemyDefeated?.Invoke(enemy);
    }

    public static void OnItemSelected() {
        onItemSelected?.Invoke();
    }

    public static void OnCollisionWithEnemy(EnemyData enemyData) {
        onCollisionWithEnemy?.Invoke(enemyData);
    }
}
