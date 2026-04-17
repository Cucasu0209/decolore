using System.Collections;
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

    private float velocityX;
    private float velocityY;

    private bool canMoveCam = false;



    void Start()
    {
        canMoveCam = true;
        ResetDataCache();
        StartCoroutine(RotateCamera(-30, 30, 0.5f));
        CurrentGamePlayManager.Instance.OnPieceChange += OnPieceChange;
    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieceChange -= OnPieceChange;
    }

    private void ResetDataCache()
    {
        Vector3 angles = transform.eulerAngles;
        rotX = angles.y;
        rotY = angles.x;
        UserInputManager.Instance.TargetRotX = rotX;
        UserInputManager.Instance.TargetRotY = rotY;
    }

    private void OnPieceChange()
    {
        if (CurrentGamePlayManager.Instance.CurrentPieaceID == -1)
        {
            if (canMoveCam == false)
            {
                ResetDataCache();
            }
            canMoveCam = true;
        }
        else
        {
            canMoveCam = false;
        }
    }



    private void Update()
    {
        if (canMoveCam == false)
            return;
        rotX = Mathf.SmoothDamp(rotX, UserInputManager.Instance.TargetRotX, ref velocityX, 1f / rotationDamping);
        rotY = Mathf.SmoothDamp(rotY, UserInputManager.Instance.TargetRotY, ref velocityY, 1f / rotationDamping);
        UpdateCamera();
    }


    void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);
        GameManager.Instance.OnRotate?.Invoke(new Vector2(rotX, rotY));

        Vector3 offset = rotation * new Vector3(0, 0, -(minDistance + UserInputManager.Instance.ZoomRatio * (maxDistance - minDistance)));

        transform.position = center + offset;
        transform.LookAt(center);
    }

    IEnumerator RotateCamera(float _targetX, float targetY, float targetRatio)
    {
        canMoveCam = false;
        float totalTime = 0.5f;
        int frameCount = 60;
        float deltaX = 360 * 1.3f;
        float deltaY = 20;
        var wait = new WaitForSeconds(totalTime / frameCount);

        rotX = _targetX + deltaX;
        rotY = targetY + deltaY;
        UserInputManager.Instance.ZoomRatio = 1;
        UpdateCamera();
        for (int i = 0; i < frameCount; i++)
        {
            yield return wait;

            rotX = _targetX + deltaX * GetValue(i + 1, frameCount);
            rotY = targetY + deltaY * GetValue(i + 1, frameCount);
            UserInputManager.Instance.ZoomRatio = targetRatio + (1 - targetRatio) * GetValue(i + 1, frameCount);
            UpdateCamera();
        }
        ResetDataCache();
        canMoveCam = true;

    }


    float EaseOut(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }
    float GetValue(int step, int totalSteps)
    {
        float t = (float)step / totalSteps; // 0 → 1
        return 1f - EaseOut(t);             // từ 1 → 0
    }
}