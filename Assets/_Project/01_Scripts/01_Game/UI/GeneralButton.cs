using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneralButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform RectBtn;
    [SerializeField] private bool PlayButtonSound = true;
    [SerializeField] private bool EnableScale = true;
    private Button SelfButton;
    private void Awake()
    {
        if (RectBtn == null)
        {
            //if (transform.childCount > 0)
            //    RectBtn = transform.GetChild(0);
            //else RectBtn = transform;
            RectBtn = transform;
        }
        SelfButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        if (SelfButton != null && SelfButton.interactable == false) return;

        if (EnableScale) RectBtn.localScale = Vector3.one;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (SelfButton != null && SelfButton.interactable == false) return;
        if (EnableScale) RectBtn.localScale = Vector3.one * (0.9f);
        if (PlayButtonSound) SoundManager.Instance.PlayButtonSound();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (SelfButton != null && SelfButton.interactable == false) return;

        if (EnableScale) RectBtn.localScale = Vector3.one;
    }
}
//this.transform.localScale = Vector3.one;
