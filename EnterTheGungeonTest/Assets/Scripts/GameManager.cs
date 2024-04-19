using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game Manager class currently only responsible for setting frame rate so editor and device behave similarly with time.deltaTime
/// </summary>
public class GameManager : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 90;
    }
}
