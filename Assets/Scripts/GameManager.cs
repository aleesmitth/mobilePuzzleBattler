using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Player player;
    public NodeGrid grid;
    private InventorGrid2D inventoryGrid;
    private bool inInventory;
    public Sprite hitNodeSprite;
    public FightingHeros fightingHeros;
    public FloatValue handSize;
    private EnemyData fightingEnemyData;
    private bool inMap;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance!=this)
            Destroy(gameObject);
    }

    private void OnEnable() {
        inMap = true;
        inInventory = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.onNodesDestroyed += ProcessDestroyedNodes;
        EventManager.onCollisionWithEnemy += LoadFightScene;
    }

    private void LoadFightScene(EnemyData enemyData) {
        //placeholder to load scene
        SceneManager.LoadScene("Scenes/Fight", LoadSceneMode.Single);
        this.fightingEnemyData = enemyData;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Inventory") {
            inventoryGrid = GameObject.FindWithTag("Grid").GetComponent<InventorGrid2D>();
            inInventory = true;
        } else if (scene.name == "Fight") {
            //TODO this is so hardcoded, need to refactor the game manager and delete this find with tag stuff
            //TODO also refactor player, because i'm having 2 player objects with different behaviour one in each scene
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies) {
                enemy.GetComponent<Enemy>().LoadEnemyData(fightingEnemyData);
            }


            this.fightingHeros = GameObject.FindWithTag("Heros").GetComponent<FightingHeros>();
            var heros = player.GetHeros();
            this.fightingHeros.LoadHeros(heros);
            this.grid = GameObject.FindWithTag("Grid").GetComponent<NodeGrid>();
            inMap = false;
        }
    }
    
    //TODO theres a bug if i press S -> A -> S the game breaks, it's because A is mostly for debugging.
    //TODO i'm not sure why it happens but for now it's not worth looking into, A will be removed, it's just for show
    private void Update() {
        //placeholder to open inventory
        if (Input.GetKeyUp(KeyCode.I)) {
            SceneManager.LoadScene("Scenes/Inventory", LoadSceneMode.Single);
        }

        if (this.inMap) return;
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (inInventory) {
                Debug.Log(GameObject.FindWithTag("Grid").GetComponent<InventorGrid2D>());
                inventoryGrid.ResetGrid();
                return;
            }
            grid.ResetGrid();
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            var hits = grid.GetMatrixHits();
            foreach (var listOfHits in hits) {
                foreach (var hit in listOfHits) {
                    hit.GetComponent<SpriteRenderer>().sprite = hitNodeSprite;
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
        fightingHeros.Attack(elementsDamage);
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
