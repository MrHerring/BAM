using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ALevelLogic : MonoBehaviour
{
    public float lvlTime = 30f;

    public virtual float GetlvlTime()
    {
        return lvlTime;
    }

    public virtual void SetlvlTime(float value)
    {
        lvlTime = value;
    }

    public abstract int CheckGameState(float time);
}

