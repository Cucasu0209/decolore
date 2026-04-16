using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayLevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelIndex;
    [SerializeField] private Button handleBtn;

    private void Start()
    {
        handleBtn.onClick.AddListener(OnClick);
        PlayerData.Instance.OnUnlockNewLevel += DisplayLevel;
        DisplayLevel();
    }

    private void OnDestroy()
    {
        PlayerData.Instance.OnUnlockNewLevel -= DisplayLevel;

    }

    private void DisplayLevel()
    {
        levelIndex.SetText($"Level {PlayerData.Instance.GetLevelUnlocked()}");
    }
    private void OnClick()
    {
       GameManager.Instance.ChooseLevel(PlayerData.Instance.GetLevelUnlocked());
    }

}
