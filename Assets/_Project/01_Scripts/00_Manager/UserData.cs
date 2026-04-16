
using UnityEngine;

public class UserData
{
    #region Keys
    //setting
    public const string HAPTIC_SETTING_KEY = "HAPTIC_SETTING";
    public const string MUSIC_SETTING_KEY = "MUSIC_SETTING";
    public const string SFX_SETTING_KEY = "SFX_SETTING";
    #endregion

    #region Get + Set Data

    //haptic
    public static bool GetHapticState()
    {
        return PlayerPrefs.GetInt(HAPTIC_SETTING_KEY, 1) == 1;
    }
    public static void SetHapticState(bool state)
    {
        PlayerPrefs.SetInt(HAPTIC_SETTING_KEY, state ? 1 : 0);
    }

    //Music
    public static bool GetMusicState()
    {
        return PlayerPrefs.GetInt(MUSIC_SETTING_KEY, 1) == 1;
    }
    public static void SetMusicState(bool state)
    {
        PlayerPrefs.SetInt(MUSIC_SETTING_KEY, state ? 1 : 0);
    }

    //Sound fx
    public static bool GetSoundFxState()
    {
        return PlayerPrefs.GetInt(SFX_SETTING_KEY, 1) == 1;
    }
    public static void SetSoundFxState(bool state)
    {
        PlayerPrefs.SetInt(SFX_SETTING_KEY, state ? 1 : 0);
    }

    #endregion
}
