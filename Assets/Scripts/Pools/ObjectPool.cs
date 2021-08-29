using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public GameObject prefab;
    public Transform parent;
    public int growthSize;
    public static ObjectPool instance;
    private Queue<GameObject> queue = new Queue<GameObject>();

    private Dictionary<string, Pool> poolsDictionary;
    public List<Pool> pools;
    
    private void Awake() {
        this.MakeSingleton();
        poolsDictionary = new Dictionary<string, Pool>();
        foreach (var pool in pools) {
            GrowPool(pool);
            poolsDictionary.Add(pool.GetName(), pool);
        }
    }

    private void MakeSingleton() {
        if (instance == default) {
            instance = this;
        }
        else if(instance != this)
            Destroy(gameObject);
    }
    
    private void GrowPool(Pool pool) {
        //i'd like for each pool to instantiate each own prefabs, but i would need to make them inherit
        //from monobehaviour and it doesn't work well with the inspector
        for (int i = 0; i < pool.GetGrowthSize(); i++) {
            GameObject instanceOfPrefab = Instantiate(pool.GetPrefab(), parent, false);
            instanceOfPrefab.SetActive(false);
            pool.Enqueue(instanceOfPrefab);
        }
    }

    public GameObject GetObject(string poolName, Transform parent = default(Transform)) {
        if (!poolsDictionary.ContainsKey(poolName))
            return null;
        
        GameObject objectToSpawn = poolsDictionary[poolName].Dequeue();
        while (objectToSpawn == null) {
            GrowPool(poolsDictionary[poolName]);
            objectToSpawn = poolsDictionary[poolName].Dequeue();
        }

        objectToSpawn.SetActive(true);
        if (parent != default(Transform))
            objectToSpawn.transform.SetParent(parent);
        return objectToSpawn;
    }

    public void DestroyObject(string poolName, GameObject objectToDestroy) {
        if (objectToDestroy == null) return;
        if (!poolsDictionary.ContainsKey(poolName)) {
            Destroy(objectToDestroy);
            return;
        }
        objectToDestroy.SetActive(false);
        poolsDictionary[poolName].Enqueue(objectToDestroy);
    }
}