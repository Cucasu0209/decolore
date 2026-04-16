using UnityEngine;

public class PlayerData : MonoBehaviour
{
   public static PlayerData Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public int GetLevelUnlocked()
    {
        return PlayerPrefs.GetInt("LevelUnlocked", 5);
    }
    public bool IsLevelUnlocked(int level)
    {
        return GetLevelUnlocked() >= level;
    }   
}
