using System.Collections.Generic;
using UnityEngine;

public class PoolManager<T> where T : Component
{
    private readonly Queue<T> objects = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;
    private readonly int expandSize;

    public PoolManager(T prefab, int initialSize, Transform parent = null, int expandSize = 10)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.expandSize = expandSize;

        Expand(initialSize); // pre-instantiate pool
    }

    private void Expand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T newObj = Object.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            objects.Enqueue(newObj);
        }
    }

    // Get object from pool (does NOT auto-release)
    public T Get()
    {
        if (objects.Count == 0)
        {
            Expand(expandSize); // batch expand
        }

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    // Return object to pool manually
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
        obj.transform.SetParent(parent);
    }

    // Clear all objects in pool
    public void Clear()
    {
        while (objects.Count > 0)
        {
            T obj = objects.Dequeue();
            if (obj != null)
                Object.Destroy(obj.gameObject);
        }
    }
}
