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
    }

    #region Level Controller
    public int CurrentLevel { get; private set; } = 1;
    public Action OnCurrentLevelChange;
    public void ChooseLevel(int level)
    {
        CurrentLevel = level;
        OnCurrentLevelChange?.Invoke();
    }

    #endregion

    #region TimeScale Controller
    private int PauseProgressCount = 0;
    public void PauseGame()
    {
        PauseProgressCount++;
        CheckGameTimeScale();
    }
    public void ResumeGame()
    {
        PauseProgressCount--;
        CheckGameTimeScale();
    }
    public void CheckGameTimeScale()
    {
        if (PauseProgressCount <= 0)
        {
            Time.timeScale = 1;
            PauseProgressCount = 0;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    private void Update()
    {
        if (UIManager.Instance.IsShowingNoInternet)
            CheckGameTimeScale();
    }
    #endregion
}
