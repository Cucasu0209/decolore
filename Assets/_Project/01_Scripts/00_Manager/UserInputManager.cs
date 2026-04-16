using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInputManager : MonoBehaviour
{
    public static UserInputManager Instance { get; private set; }

    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minY = -20f;
    [SerializeField] private float maxY = 80f;
    private float targetRotX;
    private float targetRotY;
    private float zoomRatio;


    public Action<float, float> OnRotateCam;
    public Action<float> OnZoomCam;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return null;
        SetZoomRatio(0.5f);
    }


    void Update()
    {
        if (!CurrentGamePlayManager.Instance.CanMoveCam())
            return;
        HandleInput();
    }
    private bool isMouseDown = false;
    void HandleInput()
    {

#if UNITY_EDITOR || UNITY_STANDALONE
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float scroll = Input.GetAxis("Mouse ScrollWheel");


        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                isMouseDown = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        if (isMouseDown)
        {
            targetRotX += mouseX * 6000f * rotationSpeed * Time.deltaTime;
            targetRotY -= mouseY * 6000f * rotationSpeed * Time.deltaTime;
            targetRotY = Mathf.Clamp(targetRotY, minY, maxY);

            OnRotateCam?.Invoke(targetRotX, targetRotY);
        }

        if (scroll != 0f)
        {
            zoomRatio -= scroll * zoomSpeed * 5f;
            zoomRatio = Mathf.Clamp(zoomRatio, 0, 1);
            OnZoomCam?.Invoke(zoomRatio);
        }
#endif

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
            {
                Debug.Log("Touch vào UI");
            }
            if (t.phase == TouchPhase.Moved)
            {
                targetRotX += t.deltaPosition.x * rotationSpeed;
                targetRotY -= t.deltaPosition.y * rotationSpeed;
                targetRotY = Mathf.Clamp(targetRotY, minY, maxY);

                OnRotateCam?.Invoke(targetRotX, targetRotY);

            }
        }

        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            float prev = (t1.position - t1.deltaPosition - (t2.position - t2.deltaPosition)).magnitude;
            float curr = (t1.position - t2.position).magnitude;

            float delta = curr - prev;

            zoomRatio -= delta * zoomSpeed * 0.01f;
            zoomRatio = Mathf.Clamp(zoomRatio, 0, 1);
            OnZoomCam?.Invoke(zoomRatio);

        }
    }

    public void SetZoomRatio(float target)
    {
        zoomRatio = Mathf.Clamp(target, 0, 1);
        OnZoomCam?.Invoke(zoomRatio);
    }
}

