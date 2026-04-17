using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class HeartZone : MonoBehaviour
{
    [SerializeField] private HeartIcon heartPrefab;
    [SerializeField] private float heartSpacing = 100f;

    private List<HeartIcon> heartIcons = new List<HeartIcon>();
    void Start()
    {
        CurrentGamePlayManager.Instance.OnHeartChange += DisplayHeart;
    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnHeartChange -= DisplayHeart;

    }

    private void DisplayHeart()
    {
        if (heartIcons.Count < GameConfig.HEART_INIT)
        {
            int count = GameConfig.HEART_INIT - heartIcons.Count;
            for (int i = 1; i <= count; i++)
            {
                HeartIcon newHeart = LeanPool.Spawn(heartPrefab, transform);
                heartIcons.Add(newHeart);
            }
        }

        for (int i = 0; i < heartIcons.Count; i++)
        {
            heartIcons[i].SetHeart(i < CurrentGamePlayManager.Instance.Hearts);
            heartIcons[i].SelfRect.anchoredPosition = new Vector2(i * heartSpacing - heartSpacing * (GameConfig.HEART_INIT - 1) / 2, 0);
        }
    }
}

