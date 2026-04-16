using UnityEngine;
using UnityEngine.UI;

public class ZoomSlider : MonoBehaviour
{
    [SerializeField] private Slider zoomSlider;
    private void Start()
    {
        zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChanged);
        UserInputManager.Instance.OnZoomCam += DisplayRatio;
    }
    private void OnDestroy()
    {
        UserInputManager.Instance.OnZoomCam -= DisplayRatio;

    }
    private void OnZoomSliderValueChanged(float value)
    {
        UserInputManager.Instance.SetZoomRatio(value);
    }

    private void DisplayRatio(float value)
    {
        zoomSlider.SetValueWithoutNotify(value);
    }

}
