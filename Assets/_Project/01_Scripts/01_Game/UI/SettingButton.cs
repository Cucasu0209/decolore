using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private Button btn;

    private void Start()
    {
        btn.onClick.AddListener(OnSettingButtonClick);
        //btn.transform.localScale = Vector3.zero;


        //UIManager.Instance.OnLoadingAOAComplete += ShowButton;
        //UIManager.Instance.OnShowFakeLoading += HideButton;
        //UIManager.Instance.OnCloseFakeLoading += ShowButton;
    }
    private void OnDestroy()
    {
        //UIManager.Instance.OnLoadingAOAComplete -= ShowButton;
        //UIManager.Instance.OnShowFakeLoading -= HideButton;
        //UIManager.Instance.OnCloseFakeLoading -= ShowButton;

    }
    private void ShowButton()
    {
        btn.transform.DOScale(1, 0.3f).SetDelay(1);
    }
    private void ShowButton(bool isShowFull)
    {
        btn.transform.DOScale(1, 0.3f).SetDelay(isShowFull ? 1 : 0.2f);
    }

    private void HideButton(bool isShowfull, Action OnComplete)
    {
        btn.transform.DOScale(0, 0.2f);
    }
    private void OnSettingButtonClick()
    {
        UIManager.Instance.OnOpenSettingPopup?.Invoke();
    }
}
