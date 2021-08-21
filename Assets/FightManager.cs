using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightManager : MonoBehaviour {
    private void OnEnable() {
        EventManager.onNodesDestroyed += ProcessDestroyedNodes;
    }

    private void OnDisable() {
        EventManager.onNodesDestroyed -= ProcessDestroyedNodes;
    }


    //here i should process hit info, as damage, target, combo etc.
    //double check but pretty sure that list of hits, has 1 list per line that hit, with no duplicated nodes if
    //it hits both vertically and horizontally
    //so i could do like
    //Combo points = 1 + numberOfLists * 0.1f (this would give me 10% bonus increase per line hit.
    //elementDamage = NodesOfElement * Combo points
    private void ProcessDestroyedNodes(LinkedList<LinkedList<ElementType>> listsOfElementsHit) {
        float comboBonus = 1 + listsOfElementsHit.Count * .1f;
        Dictionary<ElementType, float> elementsDamage = new Dictionary<ElementType, float>();
        foreach (ElementType element in Enum.GetValues(typeof(ElementType))) {
            elementsDamage.Add(element,0);
        }

        foreach (var list in listsOfElementsHit) {
            if (list.Count == 0) continue;
            print(list.First.Value);
            print(elementsDamage[list.First.Value]);
            elementsDamage[list.First.Value] += list.Count;
        }
        
        //debug lines
        foreach (var element in elementsDamage) {
            print("Of element: " + element.Key + " there was " + element.Value + " nodes hit");
        }
        
        print("Combo is " + ((comboBonus - 1)*100) + "%" + " and multiplier is " + comboBonus);

        elementsDamage = ApplyCombo(comboBonus, elementsDamage);
        
        //debug lines
        foreach (var element in elementsDamage) {
            print("Of element: " + element.Key + " total damage is " + element.Value);
        }
        //aca deberia mandarle al player los elementsDamage, y cada carta va a saber si su elemento esta
        //y como actuar.
        EventManager.OnNodesDamageCalculated(elementsDamage);
    }

    private Dictionary<ElementType, float> ApplyCombo(float comboBonus, Dictionary<ElementType,float> elementsDamage) {
        Dictionary<ElementType, float> updatedElementsDamage = new Dictionary<ElementType, float>();
        foreach (var element in elementsDamage) {
            var damage = element.Value * comboBonus;
            updatedElementsDamage.Add(element.Key, damage);
        }

        return updatedElementsDamage;
    }
}
