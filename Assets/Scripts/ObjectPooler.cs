/*
 * File: ObjectPooler.cs
 * Description: Generic framework class for object pooling.
 * Author: Friedemann Lipphardt
 * Date: 2025-28-03
 */

// Imports
using System.Collections.Generic;
using UnityEngine;

// ObjectPooler class, inherits from MonoBehaviour
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance; // Singleton instance for easy access

    [System.Serializable]
    public class Pool
    {
        public string tag;          // Identifier for the pool
        public GameObject prefab; // Prefab to pool
        public int size; // How many objects to pool
    }

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    private void Awake() {
        instance = this;
    }

    //  Start method, called before the first frame update
    //  Initializes the object pooler by creating a dictionary of
    //  pools, instantiating the specified number of objects for each pool,
    //  and setting them to inactive.
    private void Start() {
        poolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (Pool pool in pools) {
            List<GameObject> objectPool = new List<GameObject>();
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // SpawnFromPool method, used to spawn an object from the pool.
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        foreach (GameObject obj in poolDictionary[tag]) {
            if (!obj.activeInHierarchy) {
                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
                if (pooledObj != null) {
                    pooledObj.OnObjectSpawn();
                }
                return obj;
            }
        }
        // If all objects are in use, expand the pool by instantiating a new object.
        Debug.Log("Expanding pool for tag: " + tag);
        foreach (Pool pool in pools) {
            if (pool.tag == tag) {
                GameObject obj = Instantiate(pool.prefab, position, rotation);
                obj.SetActive(true);
                poolDictionary[tag].Add(obj);
                IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
                if (pooledObj != null) {
                    pooledObj.OnObjectSpawn();
                }
                return obj;
            }
        }
        return null;
    }
}
