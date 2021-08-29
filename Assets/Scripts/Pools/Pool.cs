using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Pool {
    public string prefabTag;
    public GameObject pooledPrefab;
    public int growthSize;
    private Queue<GameObject> objects = new Queue<GameObject>();

    public string GetName() {
        return this.prefabTag;
    }

    public GameObject GetPrefab() {
        return this.pooledPrefab;
    }

    public int GetGrowthSize() {
        return this.growthSize;
    }

    public void Enqueue(GameObject instanceOfPrefab) {
        objects.Enqueue(instanceOfPrefab);
    }

    public GameObject Dequeue() {
        return objects.Count == 0 ? default(GameObject) : objects.Dequeue();
    }
}