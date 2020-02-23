using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Struct for Script Reference
 * Optimization : Insead of using GetComponent<IPoolableComponent> all of the time
 */

public struct PoolablePrefabData
{
    public GameObject gameObject;
    public IPoolableComponent poolableComponent;
}


public class PrefabPool
{
    private Dictionary<GameObject, PoolablePrefabData> _activeList = new Dictionary<GameObject, PoolablePrefabData>();
    private Queue<PoolablePrefabData> _inactiveList = new Queue<PoolablePrefabData>();

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        PoolablePrefabData data;

        if (_inactiveList.Count > 0)
        {
            data = _inactiveList.Dequeue();
        }
        else
        {
            GameObject newGo = GameObject.Instantiate(prefab, position, rotation) as GameObject;
            data = new PoolablePrefabData();
            data.gameObject = newGo;
            data.poolableComponent = newGo.GetComponent<IPoolableComponent>();
        }

        data.gameObject.SetActive(true);
        data.gameObject.transform.position = position;
        data.gameObject.transform.rotation = rotation;

        data.poolableComponent.Spawned();

        _activeList.Add(data.gameObject, data);

        return data.gameObject;
    }

    public bool Despawn(GameObject objToDespawn)
    {
        if (!_activeList.ContainsKey(objToDespawn))
        {
            return false;
        }

        PoolablePrefabData data = _activeList[objToDespawn];

        data.poolableComponent.Despawned();

        data.gameObject.SetActive(false);

        _activeList.Remove(objToDespawn);
        _inactiveList.Enqueue(data);

        return true;
    }
}
