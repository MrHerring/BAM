using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Interface for Script Reference in PoolSystem
 */
public interface IPoolableComponent 
{
    void Spawned();
    void Despawned();
}
