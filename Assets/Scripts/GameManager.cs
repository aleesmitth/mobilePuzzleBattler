using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public NodeGrid grid;
    public Material hitNodeMat;
    public Player player;
    public FloatValue handSize;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance!=this)
            Destroy(gameObject);
    }

    private void OnEnable() {
        EventManager.onNodesDestroyed += ProcessDestroyedNodes;
    }
    
    //TODO theres a bug if i press S -> A -> S the game breaks, it's because A is mostly for debugging.
    //TODO i'm not sure why it happens but for now it's not worth looking into, A will be removed, it's just for show
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            grid.ResetGrid();
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            var hits = grid.GetMatrixHits();
            foreach (var listOfHits in hits) {
                foreach (var hit in listOfHits) {
                    hit.GetComponent<MeshRenderer>().material = hitNodeMat;
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            var hits = grid.LookForMatrixHits();
            grid.DestroyHitNodes(hits);
        }
    }
    
    //here i should process hit info, as damage, target, combo etc.
    //double check but pretty sure that list of hits, has 1 list per line that hit, with no duplicated nodes if
    //it hits both vertically and horizontally
    //so i could do like
    //Combo points = 1 + numberOfLists * 0.1f (this would give me 10% bonus increase per line hit.
    //elementDamage = NodesOfElement * Combo points
    private void ProcessDestroyedNodes(LinkedList<LinkedList<NodeType>> listsOfElementsHit) {
        float comboBonus = 1 + listsOfElementsHit.Count * .1f;
        Dictionary<NodeType, float> elementsDamage = new Dictionary<NodeType, float>();
        foreach (NodeType element in Enum.GetValues(typeof(NodeType))) {
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
        player.PlayCards(elementsDamage);
    }

    private Dictionary<NodeType, float> ApplyCombo(float comboBonus, Dictionary<NodeType,float> elementsDamage) {
        Dictionary<NodeType, float> updatedElementsDamage = new Dictionary<NodeType, float>();
        foreach (var element in elementsDamage) {
            var damage = element.Value * comboBonus;
            updatedElementsDamage.Add(element.Key, damage);
        }

        return updatedElementsDamage;
    }

    public void AddCardsToDeck(TMP_InputField inputNumber) {
        player.AddCardsToDeck(inputNumber);
    }

    public void Draw() {
        player.Draw((int)handSize.value);
    }
}
