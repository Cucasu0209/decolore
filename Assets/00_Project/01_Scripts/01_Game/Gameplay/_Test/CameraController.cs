using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform HorizontalCam;
    [SerializeField] private Transform VerticalCam;
    [SerializeField] private Transform MainCamera;

    [Header("Scroll properties")]
    [SerializeField] private float Sensitivity = 1f;
    [SerializeField] private float ZoomSpeed = 1f;
    [SerializeField] private float Elasticity = 0.1f;

    [SerializeField] private Vector2 BoundRotateY = new Vector2(20f, 70f);
    [SerializeField] private Vector2 BoundDistance = new Vector2(-8, -3.5f);

    //temp
    private float distance = 5f;
    private float rotX = 0f;
    private float rotY = 20f;

    private Vector2 lastTouchPos;
    private float lastPinchDistance;
    private void Start()
    {

    }
    private void OnDestroy()
    {

    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
#if UNITY_EDITOR
        rotX += Input.GetAxis("Mouse X") * Sensitivity * 500f;
        rotY -= Input.GetAxis("Mouse Y") * Sensitivity * 500f;
        distance -= Input.GetAxis("Mouse ScrollWheel") * 5f;
#else
        var touches = Touch.activeTouches;

        // ===== 1 NGÓN: XOAY =====
        if (touches.Count == 1)
        {
            var touch = touches[0];

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 delta = touch.delta;

                rotX += delta.x * Sensitivity;
                rotY -= delta.y * Sensitivity;

                rotY = Mathf.Clamp(rotY, BoundRotateY.x, BoundRotateY.y);
            }
        }

        // ===== 2 NGÓN: ZOOM =====
        if (touches.Count == 2)
        {
            var t1 = touches[0];
            var t2 = touches[1];

            float currentDistance = Vector2.Distance(t1.screenPosition, t2.screenPosition);

            if (t1.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                t2.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                lastPinchDistance = currentDistance;
            }
            else
            {
                float delta = currentDistance - lastPinchDistance;

                distance -= delta * ZoomSpeed;
                distance = Mathf.Clamp(distance, BoundRotateY.x, BoundRotateY.y);

                lastPinchDistance = currentDistance;
            }
        }
#endif

        UpdateCamera();
    }




    void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);

        MainCamera.localPosition = Vector3.forward * distance;
        HorizontalCam.transform.localEulerAngles = Vector3.up * rotX;
        VerticalCam.transform.localEulerAngles = Vector3.right * rotY;
    }
}
