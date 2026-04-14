using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action<float> OnZoom;
    public static GameManager Instance { get; private set; }    
    private  void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

}
