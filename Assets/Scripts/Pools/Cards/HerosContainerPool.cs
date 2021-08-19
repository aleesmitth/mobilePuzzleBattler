using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerosContainerPool : NodePool {
    public GameObject prefab;
    public Transform parent;
    public int growthSize;
    public static HerosContainerPool instance;
    private Queue<GameObject> queue = new Queue<GameObject>();
    
    private void Awake() {
        this.MakeSingleton();
    }
    
    private void MakeSingleton() {
        if (instance == default) {
            instance = this;
        }
        else if(instance != this)
            Destroy(gameObject);
    }
    
    public override GameObject Prefab { get => prefab; set => prefab = value; }
    public override Transform Parent { get => parent; set => parent = value; }
    public override int GrowthSize { get => growthSize; set => growthSize = value; }
    protected override Queue<GameObject> Queue { get => queue; set => queue = value; }
}