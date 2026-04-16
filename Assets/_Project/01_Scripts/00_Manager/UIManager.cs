using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
        Input.multiTouchEnabled = false;
    }

    #region Events

    public Action OnOpenSettingPopup;
    public Action OnOpenGaragePopup;
    public Action OnCloseGaragePopup;
    public Action OnOpenRatingPopup;
    public Action<bool, Action> OnShowFakeLoading;
    public Action<bool> OnCloseFakeLoading;

    public Action OnLoadingAOAComplete;
    public Action<int, Action> OnShowRate;

    #endregion

    #region Global 
    public bool IsShowingFakeLoading = false;
    public bool IsShowingRate = false;
    public bool IsAdsShowing = false;
    public bool IsShowingNoInternet = false;
    public float LastTimeShowAds = -99;
    public bool IsStartgame = true;
    #endregion

    #region CheckAndShowAds
    public void SetCanShowAds()
    {
        PlayerPrefs.SetInt("CanShowAds", 1);
    }
    public bool CanShowAds()
    {
        return PlayerPrefs.GetInt("CanShowAds", 0) == 1;
    }
    public void ShowRewarded(Action OnComplete, string placement)
    {
        //if (AdsManager.Instance.IsRemoveAds())
        //{
        //    OnComplete?.Invoke();
        //    return;
        //}
        //IsAdsShowing = true;
        //float cacheTimeScale = Time.timeScale;
        //LastTimeShowAds = Time.time;

        //AdsManager.Instance.ShowRewarded(() =>
        //{
        //    IsAdsShowing = false;
        //    Time.timeScale = cacheTimeScale;
        //    LastTimeShowAds = Time.time;
        //    AdsManager.Instance._timeCloseBreakAds = Time.unscaledTime;
        //    OnComplete?.Invoke();
        //}, placement);
    }
    public void ShowInters(Action OnComplete, string placement)
    {
        //if (AdsManager.Instance.IsRemoveAds())
        //{
        //    OnComplete?.Invoke();
        //    return;
        //}
        //if (CanShowAds())
        //{
        //    IsAdsShowing = true;
        //    float cacheTimeScale = Time.timeScale;
        //    LastTimeShowAds = Time.time;

        //    AdsManager.Instance.ShowInterstitial(() =>
        //    {
        //        IsAdsShowing = false;
        //        Time.timeScale = cacheTimeScale;
        //        LastTimeShowAds = Time.time;
        //        OnComplete?.Invoke();
        //    }, placement);
        //}
        //else
        //{
        //    OnComplete?.Invoke();
        //}
    }

    public void ShowAdsBreak(Action OnComplete, string placement)
    {
        //if (AdsManager.Instance.IsRemoveAds())
        //{
        //    OnComplete?.Invoke();
        //    return;
        //}
        //if (CanShowAds())
        //{
        //    IsAdsShowing = true;
        //    float cacheTimeScale = Time.timeScale;
        //    LastTimeShowAds = Time.time;

        //    AdsManager.Instance.ShowAdsBreak(() =>
        //    {
        //        AdsManager.Instance._timeCloseBreakAds = Time.unscaledTime;
        //        IsAdsShowing = false;
        //        Time.timeScale = cacheTimeScale;
        //        LastTimeShowAds = Time.time;
        //        OnComplete?.Invoke();
        //    }, placement);
        //}
        //else
        //{
        //    OnComplete?.Invoke();
        //}
    }
    public void LogEvent(string message)
    {
        //Debug.LogWarning("LogEvent: " + message);
        //AnalyticsManager.Instance.LogEvent(message);
    }
    #endregion
}
