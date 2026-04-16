using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    public bool IsOpen { get; private set; } = false;
    [Header("General")]
    [SerializeField] private Image Dark;
    [SerializeField] private Button CloseButton;
    [SerializeField] private RectTransform PopupContent;

    [Header("Content")]
    [SerializeField] private Button MusicButton;
    [SerializeField] private Button SoundFxButton;
    [SerializeField] private Button HapticButton;
    [Header("Images")]
    [SerializeField] private Sprite ToggleOn;
    [SerializeField] private Sprite ToggleOff;




    private void Start()
    {
        SetupStart();
        HapticButton.onClick.AddListener(ToggleHaptic);
        SoundFxButton.onClick.AddListener(ToggleSoundFx);
        MusicButton.onClick.AddListener(ToggleMusic);
        CloseButton.onClick.AddListener(HidePopup);
        UIManager.Instance.OnOpenSettingPopup += OpenPopup;
    }
    private void OnDestroy()
    {
        if (UIManager.Instance != null) UIManager.Instance.OnOpenSettingPopup -= OpenPopup;

    }
    private void SetupStart()
    {
        Dark.rectTransform.anchoredPosition = Vector2.zero;
        Dark.color = new Color(Dark.color.r, Dark.color.g, Dark.color.b, 0);
        Dark.gameObject.SetActive(false);
        Dark.rectTransform.anchoredPosition = Vector2.zero;
    }
    private void OpenPopup()
    {
        GameManager.Instance.PauseGame();

        IsOpen = true;
        Dark.gameObject.SetActive(true);
        Dark.DOFade(0.8f, 0.2f).SetUpdate(true);
        PopupContent.transform.localScale = Vector3.zero;
        PopupContent.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear).SetDelay(0.1f).SetUpdate(true);
        UpdateState();
    }
    private void HidePopup()
    {
        GameManager.Instance.ResumeGame();

        IsOpen = false;
        PopupContent.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).SetUpdate(true);
        Dark.DOFade(0, 0.2f).SetDelay(0.1f).OnComplete(() =>
        {
            Dark.gameObject.SetActive(false);
        }).SetUpdate(true);
    }
    private void UpdateState()
    {
        HapticButton.image.sprite = (UserData.GetHapticState() ? ToggleOn : ToggleOff);
        SoundFxButton.image.sprite = (UserData.GetSoundFxState() ? ToggleOn : ToggleOff);
        MusicButton.image.sprite = (UserData.GetMusicState() ? ToggleOn : ToggleOff);
    }
    private void ToggleHaptic()
    {
        SoundManager.Instance.SetHapticState(!UserData.GetHapticState());
        UpdateState();
    }
    private void ToggleMusic()
    {
        SoundManager.Instance.SetBGMusicState(!UserData.GetMusicState());
        UpdateState();
    }
    private void ToggleSoundFx()
    {
        SoundManager.Instance.SetSoundFxState(!UserData.GetSoundFxState());
        UpdateState();
    }
}
