using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public const int BOTTOM_GAP = 150;
    public const int BOTTOM_GAP_TOOL = 100;
    public const float TIME_LOADING = 5;
    public const float DEFAULT_ALPHA = 100;
    public const float CURRENT_MAX_LEVEL = 8;


    public const string RESOURCE_LINK_LEVEL_ICON = "Icon/lv";
    public static string GetLevelIconLink(int level)
    {
        return RESOURCE_LINK_LEVEL_ICON + level;
    }
}
