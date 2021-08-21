using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData {
    [Header("and another array of possible objects to drop maybe.")]
    [Header("here i can also add stuff like health, damage,")]
    public Sprite sprite;
    public int maxTurnsToKill;
    public bool isItTimedFight;
    public int maxTimeToKill;
}
