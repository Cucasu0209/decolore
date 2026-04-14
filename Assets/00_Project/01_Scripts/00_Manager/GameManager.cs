using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action<float> OnZoom;
    public Action<Vector2> OnRotate;
    public Action<int, Vector3> SendMessage;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

}
