using UnityEngine;
using UnityEngine.EventSystems;

public class AdvancedOrbitCamera : MonoBehaviour
{
    public Vector3 center;

    [Header("Distance")]
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;




    [Header("Damping")]
    [SerializeField] private float rotationDamping = 10f;

    private float rotX;
    private float rotY;
    private float targetRotX;
    private float targetRotY;
    private float velocityX;
    private float velocityY;
    private float zoomRatio = 0.5f;



    void Start()
    {

        Vector3 angles = transform.eulerAngles;
        rotX = targetRotX = angles.y;
        rotY = targetRotY = angles.x;
        zoomRatio = 0.5f;
        UserInputManager.Instance.OnRotateCam += ApplyCamRotation;
        UserInputManager.Instance.OnZoomCam += ApplyZooming;
    }

    private void OnDestroy()
    {
        UserInputManager.Instance.OnRotateCam -= ApplyCamRotation;
        UserInputManager.Instance.OnZoomCam -= ApplyZooming;
    }




    private void Update()
    {
        rotX = Mathf.SmoothDamp(rotX, targetRotX, ref velocityX, 1f / rotationDamping);
        rotY = Mathf.SmoothDamp(rotY, targetRotY, ref velocityY, 1f / rotationDamping);
        UpdateCamera();
    }

    private void ApplyCamRotation(float _targetRotX, float _targetRotY)
    {
        targetRotY = _targetRotY;
        targetRotX = _targetRotX;
    }


    private void ApplyZooming(float _targetZoomRatio)
    {
        zoomRatio = _targetZoomRatio;

    }

    // ================= CAMERA =================
    void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);
        GameManager.Instance.OnRotate?.Invoke(new Vector2(rotX, rotY));

        Vector3 offset = rotation * new Vector3(0, 0, -(minDistance + zoomRatio * (maxDistance - minDistance)));

        transform.position = center + offset;
        transform.LookAt(center);
    }
}