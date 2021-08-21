using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightGUI : MonoBehaviour {
    private int turns;
    private int maxTurns;
    private float timeLeft;
    private bool timedFight;
    public TextMeshProUGUI turnsGUI;
    public TextMeshProUGUI fireDamageGUI;
    public TextMeshProUGUI waterDamageGUI;
    public TextMeshProUGUI earthDamageGUI;
    public TextMeshProUGUI airDamageGUI;
    public GameObject timerImage;

    private void OnEnable() {
        turns = 0;
        maxTurns = GameContextData.EnemyData.maxTurnsToKill;
        timeLeft = GameContextData.EnemyData.maxTimeToKill;
        timedFight = GameContextData.EnemyData.isItTimedFight;
        if (timedFight) {
            timerImage.SetActive(true);
        }
        else {
            ShowTurns();
        }

        EventManager.onNodesDamageCalculated += UpdateTurnCounterGUI;
    }
    private void OnDisable() {
        EventManager.onNodesDamageCalculated -= UpdateTurnCounterGUI;
    }
    
    private void Update() {
        if (!timedFight) return;
        ShowTime();
    }

    private void UpdateTurnCounterGUI(Dictionary<ElementType, float> elementsDamage) {
        foreach (var elementDamage in elementsDamage) {
            switch (elementDamage.Key) {
                case ElementType.FIRE:
                    fireDamageGUI.text = elementDamage.Value.ToString("f1");
                    break;
                case ElementType.WATER:
                    waterDamageGUI.text = elementDamage.Value.ToString("f1");
                    break;
                case ElementType.EARTH:
                    earthDamageGUI.text = elementDamage.Value.ToString("f1");
                    break;
                case ElementType.AIR:
                    airDamageGUI.text = elementDamage.Value.ToString("f1");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (timedFight) return;
        turns++;
        ShowTurns();
    }

    private void ShowTurns() {
        turnsGUI.text = turns.ToString() + "/" + maxTurns;
    }

    private void ShowTime() {
        turnsGUI.text = (Mathf.Ceil((timeLeft -= Time.deltaTime))).ToString();
    }
}
