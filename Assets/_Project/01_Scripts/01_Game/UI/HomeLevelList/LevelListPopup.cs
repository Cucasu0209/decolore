using UnityEngine;
using UnityEngine.UI;

public class LevelListPopup : MonoBehaviour
{
    [SerializeField] private RectTransform ListContent;
    [SerializeField] private Button Back;
    private void Start()
    {
        GameManager.Instance.OnCurrentLevelChange += OnChooseLevel;
        UIManager.Instance.OnShowPreview += ShowPreview;
        UIManager.Instance.OnShowListLevel += ShowList;
        Back.onClick.AddListener(OnBackToList);
        OnBackToList();

    }
    private void OnDestroy()
    {
        GameManager.Instance.OnCurrentLevelChange -= OnChooseLevel;
        UIManager.Instance.OnShowPreview -= ShowPreview;
        UIManager.Instance.OnShowListLevel -= ShowList;
    }
    private void OnChooseLevel()
    {
        UIManager.Instance.OnShowPreview?.Invoke();
    }
    private void OnBackToList()
    {
        UIManager.Instance.OnShowListLevel?.Invoke();
    }

    private void ShowPreview()
    {
        ListContent.gameObject.SetActive(false);
        Back.gameObject.SetActive(true);
    }
    private void ShowList()
    {
        ListContent.gameObject.SetActive(true);
        Back.gameObject.SetActive(false);

    }
}
