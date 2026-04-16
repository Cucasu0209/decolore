using UnityEngine;

public class AdvancedOrbitCamera : MonoBehaviour
{
    public Vector3 center;

    [Header("Distance")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float minY = -20f;
    [SerializeField] private float maxY = 80f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 0.1f;

    [Header("Damping")]
    [SerializeField] private float rotationDamping = 10f;
    [SerializeField] private float zoomDamping = 10f;

    [Header("Auto Rotate")]
    [SerializeField] private float autoRotateSpeed = 10f;
    [SerializeField] private float idleTimeBeforeAuto = 2f;

    private float rotX;
    private float rotY;

    private float targetRotX;
    private float targetRotY;

    private float targetDistance;

    private float velocityX;
    private float velocityY;

    private float lastInputTime;


    void Start()
    {

        Vector3 angles = transform.eulerAngles;
        rotX = targetRotX = angles.y;
        rotY = targetRotY = angles.x;

        distance = targetDistance = Vector3.Distance(transform.position, center);

    }



    void Update()
    {
        if (!CurrentGamePlayManager.Instance.CanMoveCam())
            return;
        HandleInput();
        HandleAutoRotate();
        ApplyDamping();
        UpdateCamera();
    }

    // ================= INPUT =================
    void HandleInput()
    {
        bool hasInput = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(0))
        {
            targetRotX += mouseX * 6000f * rotationSpeed * Time.deltaTime;
            targetRotY -= mouseY * 6000f * rotationSpeed * Time.deltaTime;
            hasInput = true;
        }

        if (scroll != 0f)
        {
            targetDistance -= scroll * 5f;
            hasInput = true;
        }
#endif

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Moved)
            {
                targetRotX += t.deltaPosition.x * rotationSpeed;
                targetRotY -= t.deltaPosition.y * rotationSpeed;
                hasInput = true;
            }
        }

        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            float prev = (t1.position - t1.deltaPosition - (t2.position - t2.deltaPosition)).magnitude;
            float curr = (t1.position - t2.position).magnitude;

            float delta = curr - prev;

            targetDistance -= delta * zoomSpeed * 0.01f;
            hasInput = true;
        }

        targetRotY = Mathf.Clamp(targetRotY, minY, maxY);
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        if (hasInput)
            lastInputTime = Time.time;
    }

    // ================= AUTO ROTATE =================
    void HandleAutoRotate()
    {
        if (Time.time - lastInputTime > idleTimeBeforeAuto)
        {
            targetRotX += autoRotateSpeed * Time.deltaTime;
        }
    }

    // ================= DAMPING =================
    void ApplyDamping()
    {
        rotX = Mathf.SmoothDamp(rotX, targetRotX, ref velocityX, 1f / rotationDamping);
        rotY = Mathf.SmoothDamp(rotY, targetRotY, ref velocityY, 1f / rotationDamping);

        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomDamping);
    }

    // ================= CAMERA =================
    void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);
        GameManager.Instance.OnRotate?.Invoke(new Vector2(rotX, rotY));

        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = center + offset;
        transform.LookAt(center);
    }
}