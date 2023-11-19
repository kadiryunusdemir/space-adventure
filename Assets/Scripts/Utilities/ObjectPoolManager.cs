using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        [System.Serializable]
        public class ObjectPool
        {
            public Enums.ObjectPoolType objectPoolType; 
            public GameObject originalPrefab;
            public HashSet<GameObject> ActiveObjects = new HashSet<GameObject>();
            public Queue<GameObject> ObjectsInQueue = new Queue<GameObject>();
            public int initialSize; 
        }
    
        public List<ObjectPool> pools;

        private void Start()
        {
            InitializePool();
        }

        public void InitializePool()
        {
            foreach (ObjectPool pool in pools)
            {
                for (int i = 0; i < pool.initialSize; i++)
                {
                    GameObject obj = Instantiate(pool.originalPrefab);
                    obj.SetActive(false); 
                    pool.ObjectsInQueue.Enqueue(obj);
                }
            }
        }

        public GameObject Get(Enums.ObjectPoolType type)
        {
            ObjectPool targetPool = pools.Find(x => x.objectPoolType == type);

            if (targetPool.ObjectsInQueue.Count > 0)
            {
                GameObject obj = targetPool.ObjectsInQueue.Dequeue();
                targetPool.ActiveObjects.Add(obj);
                obj.SetActive(true); // Activate
                return obj;
            }
            GameObject obj2 = Instantiate(targetPool.originalPrefab);
            targetPool.ActiveObjects.Add(obj2);
            obj2.SetActive(true); // Activate
            return obj2;
        }

        public void ReturnToPool(GameObject obj)
        { 
            Enums.ObjectPoolType type = obj.GetComponent<PoolObject>().type;
            ObjectPool targetPool = pools.Find(x => x.objectPoolType == type);
    
            if (targetPool.ActiveObjects.Contains(obj))
            {
                targetPool.ActiveObjects.Remove(obj);
                targetPool.ObjectsInQueue.Enqueue(obj);
                obj.SetActive(false);
                obj.transform.parent = this.transform; 
            }
            else
            {
                Debug.LogError("Duplication error");
            }
        }

        public void ReturnAllToPoolByType(Enums.ObjectPoolType type)
        {
            ObjectPool targetPool = pools.Find(x => x.objectPoolType == type);
            List<GameObject> tmp = new List<GameObject>(targetPool.ActiveObjects);
        
            int count = targetPool.ActiveObjects.Count; 
            for (int i = 0; i < count; i++)
            {
                ReturnToPool(tmp[i]);
            }
        }

        public void ResetAll()
        {
            foreach (ObjectPool pool in pools)
            {
                ReturnAllToPoolByType(pool.objectPoolType);
            }
        }
    }
}
