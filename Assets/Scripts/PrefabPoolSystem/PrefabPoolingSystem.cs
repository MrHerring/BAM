using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Enemy Spawn Manager Uses this class
 * On class for all prefabs, it's totally independent from Prefab
 */

public static class PrefabPoolingSystem
{
    static Dictionary<GameObject, PrefabPool> _prefabToPoolMap = new Dictionary<GameObject, PrefabPool>();
    static Dictionary<GameObject, PrefabPool> _gameObjectToPoolMap = new Dictionary<GameObject, PrefabPool>();

    static List<GameObject> _spawnedObjects = new List<GameObject>();

    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {

        if (!_prefabToPoolMap.ContainsKey(prefab))
        {
            _prefabToPoolMap.Add(prefab, new PrefabPool());
        }

        PrefabPool pool = _prefabToPoolMap[prefab];
        GameObject gameObject = pool.Spawn(prefab, position, rotation);

        _gameObjectToPoolMap.Add(gameObject, pool);
        _spawnedObjects.Add(gameObject);

        return gameObject;
    }


    public static bool Despawn(GameObject obj)
    {
        if (!_gameObjectToPoolMap.ContainsKey(obj))
        {
            return false;
        }

        PrefabPool pool = _gameObjectToPoolMap[obj];

        if (pool.Despawn(obj))
        {
            _gameObjectToPoolMap.Remove(obj);
            _spawnedObjects.Remove(obj);
            return true;
        }

        return false;
    }

    public static void Reset()
    {
        _prefabToPoolMap.Clear();
        _gameObjectToPoolMap.Clear();
        _spawnedObjects.Clear();
    }

    public static void Prespawn(GameObject[] prefabs, Vector3 position, Quaternion rotation, int numToSpawn)
    {
       
        foreach(GameObject prefab in prefabs)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                _spawnedObjects.Add(Spawn(prefab, position, rotation));
            }

            for (int i = 0; i < numToSpawn; i++)
            {
                Despawn(_spawnedObjects[i]);
            }

            _spawnedObjects.Clear();
        }

        _spawnedObjects.Clear();
    }

    public static void ClearAll()
    {
        while(_spawnedObjects.Count > 0)
        {
            Despawn(_spawnedObjects[0]);
        }
    }

}
