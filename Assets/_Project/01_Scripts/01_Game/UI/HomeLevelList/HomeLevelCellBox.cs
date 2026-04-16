using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeLevelCellBox : MonoBehaviour
{
    [SerializeField] private Button enterLevel;
    [SerializeField] private Image levelIcon;
    [SerializeField] private TextMeshProUGUI levelIndex;
    [SerializeField] private GameObject LockPanel;
    private int level;
    private void Start()
    {
        enterLevel.onClick.AddListener(PlayLevel);

    }
    public void SetLevel(int lv)
    {
        level = lv;
        Sprite icon = Resources.Load<Sprite>(GameConfig.GetLevelIconLink(level));

        levelIcon.sprite = icon;
        levelIndex.SetText($"Level {level}");

        LockPanel.gameObject.SetActive(PlayerData.Instance.GetLevelUnlocked() < level);
    }
    private void PlayLevel()
    {
        if (PlayerData.Instance.GetLevelUnlocked() < level) return;
        Debug.Log($"Play Level {level}");
    }

}
