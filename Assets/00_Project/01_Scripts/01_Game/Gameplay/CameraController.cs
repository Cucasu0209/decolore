using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform HorizontalCam;
    [SerializeField] private Transform VerticalCam;

    [Header("Scroll properties")]
    [SerializeField] private  float Sensitivity = 1f;
    [SerializeField] private float Elasticity = 0.1f;

    private void Start()
    {
        
    }

    private void Update()
    {
      
    }
}
