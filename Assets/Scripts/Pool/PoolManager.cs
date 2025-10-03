using System.Collections.Generic;
using UnityEngine;

public class PoolManager<T> where T : Component
{
    private readonly Queue<T> objects = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;

    public PoolManager(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        // Pre-instantiate objects
        for (int i = 0; i < initialSize; i++)
        {
            T newObj = Object.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            objects.Enqueue(newObj);
        }
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            // Expand dynamically
            T newObj = Object.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            objects.Enqueue(newObj);
        }

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }

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
