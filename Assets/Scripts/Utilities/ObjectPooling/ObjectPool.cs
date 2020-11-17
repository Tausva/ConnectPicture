using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    private static Dictionary<ObjectPoolEnum, PoolObject> pools;

    public static void Initialize()
    {
        pools = new Dictionary<ObjectPoolEnum, PoolObject>();
    }

    public static void InitializeNewPool(ObjectPoolEnum poolType, int capacity, GameObject prefab)
    {
        InitializeNewPoolInParent(poolType, capacity, prefab, null);
    }

    public static void InitializeNewPoolInParent(ObjectPoolEnum poolType, int capacity, GameObject prefab, Transform parent)
    {
        pools.Add(poolType, new PoolObject(prefab, capacity, parent));

        for (int i = 0; i < pools[poolType].ObjectList.Capacity; i++)
        {
            pools[poolType].ObjectList.Add(GetNewObject(prefab, parent));
            pools[poolType].Parent = parent;
        }
    }

    public static GameObject GetPooledObject(ObjectPoolEnum poolType)
    {
        List<GameObject> pool = pools[poolType].ObjectList;

        // check for available object in pool
        if (pool.Count > 0)
        {
            GameObject obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            obj.GetComponent<IObjectPoolController>().Activate();

            return obj;
        }
        else
        {
            // pool empty, so expand pool and return new object
            pool.Capacity++;

            GameObject obj = GetNewObject(pools[poolType].Prefab, pools[poolType].Parent);
            obj.GetComponent<IObjectPoolController>().Activate();

            return obj;
        }
    }

    public static void ReturnPooledObject(ObjectPoolEnum name, GameObject obj)
    {
        pools[name].ObjectList.Add(obj);

        obj.GetComponent<IObjectPoolController>().Deactivate();
    }

    static GameObject GetNewObject(GameObject prefab, Transform parent)
    {
        GameObject obj;
        
        obj = GameObject.Instantiate(prefab, parent);
        obj.SetActive(false);

        return obj;
    }

    private class PoolObject
    {
        public GameObject Prefab { get; set; }
        public List<GameObject> ObjectList { get; set; }
        public Transform Parent { get; set; }

        public PoolObject(GameObject prefab, int capacity, Transform parent)
        {
            Prefab = prefab;
            ObjectList = new List<GameObject>(capacity);
            Parent = parent;
        }
    }
}

public enum ObjectPoolEnum
{
    Node,
    NodeText,
    Rope
}