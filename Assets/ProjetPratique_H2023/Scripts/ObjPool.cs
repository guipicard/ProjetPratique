using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    private GameObject newObj;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }

    public List<Pool> Pools;
    public Dictionary<string, List<GameObject>> PoolsDictionary;

    void Start()
    {
        PoolsDictionary = new Dictionary<string, List<GameObject>>();
        foreach (var pool in Pools)
        {
            List<GameObject> newList = new List<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, pool.parent);
                obj.SetActive(false);
                newList.Add(obj);
            }
            PoolsDictionary.Add(pool.tag, newList);
            
        }
    }

    public GameObject GetObj(string listName)
    {
        foreach (var obj in PoolsDictionary[listName])
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }

        GameObject newObj = new GameObject();
        
        foreach (var pool in Pools)
        {
            if (pool.tag == listName)
            {
                newObj = Instantiate(pool.prefab);
            }
        }
        newObj.SetActive(false);
        PoolsDictionary[listName].Add(newObj);
        return newObj;
    }

    public List<GameObject> GetActive(string tag)
    {
        List<GameObject> objList = new List<GameObject>();
        foreach (var obj in PoolsDictionary[tag])
        {
            if (obj.activeSelf) objList.Add(obj);
        }

        return objList;
    }
}
