using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }



    #region Level Unlocked
    private string LevelUnlockedKey = "LevelUnlocked";
    public Action OnUnlockNewLevel;
    public void UnlockLevel(int level)
    {
        float newLevel = Mathf.Clamp(level, 1, GameConfig.CURRENT_MAX_LEVEL);
        if (level != newLevel)
        {
            PlayerPrefs.SetInt(LevelUnlockedKey, Mathf.Clamp(level, 1, GameConfig.CURRENT_MAX_LEVEL));
            OnUnlockNewLevel?.Invoke();
        }
    }
    public int GetLevelUnlocked()
    {
        return Mathf.Min(PlayerPrefs.GetInt(LevelUnlockedKey, 1), GameConfig.CURRENT_MAX_LEVEL);
    }
    public bool IsLevelUnlocked(int level)
    {
        return GetLevelUnlocked() >= level;
    }
    #endregion
}
