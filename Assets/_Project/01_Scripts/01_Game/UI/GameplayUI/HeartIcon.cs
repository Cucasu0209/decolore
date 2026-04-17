using UnityEngine;

public class HeartIcon : MonoBehaviour
{
    public RectTransform SelfRect;
    [SerializeField] private RectTransform HeartOn;
    [SerializeField] private RectTransform HeartOff;

    public void SetHeart(bool isOn)
    {
        HeartOn.gameObject.SetActive(isOn);
        HeartOff.gameObject.SetActive(!isOn);
    }
}
