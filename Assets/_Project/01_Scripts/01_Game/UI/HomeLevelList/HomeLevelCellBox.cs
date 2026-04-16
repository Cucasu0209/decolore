using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeLevelCellBox : MonoBehaviour
{
    [SerializeField] private Button enterLevel;
    [SerializeField] private Image levelIcon;
    [SerializeField] private TextMeshProUGUI levelIndex;
    [SerializeField] private GameObject LockPanel;
    [SerializeField] private Image LockIcon;
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

        LockPanel.gameObject.SetActive(PlayerData.Instance.IsLevelUnlocked(level) == false);
    }
    private void PlayLevel()
    {
        if (PlayerData.Instance.IsLevelUnlocked(level))
        {
            GameManager.Instance.ChooseLevel(level);
        }
        else
        {
            LockIcon.DOKill();
            LockIcon.color = Color.white;
            LockIcon.DOColor(Color.red, 0.08f).SetLoops(4, LoopType.Yoyo);
        }
    }

}
