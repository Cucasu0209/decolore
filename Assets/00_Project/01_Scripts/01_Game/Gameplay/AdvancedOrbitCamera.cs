using UnityEngine;
using UnityEngine.InputSystem;

public class AdvancedOrbitCamera : MonoBehaviour
{
    public Transform target;

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;

    [Header("Rotation")]
    public float rotationSpeed = 0.2f;
    public float minY = -20f;
    public float maxY = 80f;

    [Header("Zoom")]
    public float zoomSpeed = 0.01f;

    [Header("Damping")]
    public float rotationDamping = 10f;
    public float zoomDamping = 10f;

    [Header("Elasticity")]
    public float elasticityStrength = 5f;
    public float maxElasticOffset = 15f;

    [Header("Auto Rotate")]
    public float autoRotateSpeed = 10f;
    public float idleTimeBeforeAuto = 2f;

    private float rotX;
    private float rotY;

    private float targetRotX;
    private float targetRotY;

    private float targetDistance;

    private float velRotX;
    private float velRotY;
    private float velElastic;

    private float lastInputTime;

    private Bounds targetBounds;

    void Start()
    {
        if (target == null) return;

        Vector3 angles = transform.eulerAngles;
        rotX = targetRotX = angles.y;
        rotY = targetRotY = angles.x;

        distance = targetDistance = Vector3.Distance(transform.position, target.position);

        CacheBounds();
    }

    void CacheBounds()
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        targetBounds = renderers[0].bounds;
        foreach (var r in renderers)
            targetBounds.Encapsulate(r.bounds);

        float height = targetBounds.size.y;
        maxY = Mathf.Lerp(30f, 80f, height / 5f);
        minY = -maxY * 0.5f;
    }

    void Update()
    {
        if (target == null) return;

        HandleInput();
        HandleAutoRotate();
        ApplyElasticity();
        ApplyDamping();
        UpdateCamera();
    }

    // ================= INPUT =================
    void HandleInput()
    {
        bool hasInput = false;

        // ===== MOUSE =====
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();

                targetRotX += delta.x * rotationSpeed;
                targetRotY -= delta.y * rotationSpeed;

                hasInput = true;
            }

            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                targetDistance -= scroll * zoomSpeed;
                hasInput = true;
            }
        }

        // ===== TOUCH =====
        if (Touchscreen.current != null)
        {
            var touches = Touchscreen.current.touches;

            // 1 finger rotate
            if (touches.Count > 0 && touches[0].press.isPressed && touches.Count == 1)
            {
                Vector2 delta = touches[0].delta.ReadValue();

                targetRotX += delta.x * rotationSpeed;
                targetRotY -= delta.y * rotationSpeed;

                hasInput = true;
            }

            // pinch zoom
            if (touches.Count >= 2)
            {
                var t1 = touches[0];
                var t2 = touches[1];

                if (t1.press.isPressed && t2.press.isPressed)
                {
                    Vector2 prev1 = t1.position.ReadValue() - t1.delta.ReadValue();
                    Vector2 prev2 = t2.position.ReadValue() - t2.delta.ReadValue();

                    float prevDist = (prev1 - prev2).magnitude;
                    float currDist = (t1.position.ReadValue() - t2.position.ReadValue()).magnitude;

                    float delta = currDist - prevDist;

                    targetDistance -= delta * zoomSpeed;
                    hasInput = true;
                }
            }
        }

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

    // ================= ELASTIC =================
    void ApplyElasticity()
    {
        float clamped = Mathf.Clamp(targetRotY, minY, maxY);

        // cho phép vượt giới hạn 1 chút
        float offset = targetRotY - clamped;
        offset = Mathf.Clamp(offset, -maxElasticOffset, maxElasticOffset);

        float elasticTarget = clamped + offset;

        // spring kéo về
        targetRotY = Mathf.SmoothDamp(
            targetRotY,
            elasticTarget,
            ref velElastic,
            1f / elasticityStrength
        );
    }

    // ================= DAMPING =================
    void ApplyDamping()
    {
        rotX = Mathf.SmoothDamp(rotX, targetRotX, ref velRotX, 1f / rotationDamping);
        rotY = Mathf.SmoothDamp(rotY, targetRotY, ref velRotY, 1f / rotationDamping);

        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomDamping);
    }

    // ================= CAMERA =================
    void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);

        Vector3 center = targetBounds.center;

        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = center + offset;
        transform.LookAt(center);
    }
}