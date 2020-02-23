using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Just to limit frame rate to 60fps
 * In mobile games, 30fps is default
 */

public class FrameRate : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
