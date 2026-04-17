using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class UserInputManager : MonoBehaviour
{
    public static UserInputManager Instance { get; private set; }

    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minY = -20f;
    [SerializeField] private float maxY = 80f;

    [Header("UI Components")]
    [SerializeField] private RectTransform itemListRect;


    [HideInInspector] public float TargetRotX;
    [HideInInspector] public float TargetRotY;
    [HideInInspector] public float ZoomRatio;


    public Action OnRotateCam;
    public Action OnZoomCam;

    private float mouseX;
    private float mouseY;
    private float scroll;
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

        mouseX = mouseY = 0;
        scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(0))
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(mouseX) > 5) mouseX = 0;
            if (Mathf.Abs(mouseY) > 5) mouseY = 0;
        }

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
            TargetRotX += mouseX * 6000f * rotationSpeed * Time.deltaTime;
            TargetRotY -= mouseY * 6000f * rotationSpeed * Time.deltaTime;
            TargetRotY = Mathf.Clamp(TargetRotY, minY, maxY);

            OnRotateCam?.Invoke();
        }

        if (scroll != 0f)
        {
            ZoomRatio -= scroll * zoomSpeed * 5f;
            ZoomRatio = Mathf.Clamp(ZoomRatio, 0, 1);
            OnZoomCam?.Invoke();
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
                TargetRotX += t.deltaPosition.x * rotationSpeed;
                TargetRotY -= t.deltaPosition.y * rotationSpeed;
                TargetRotY = Mathf.Clamp(TargetRotY, minY, maxY);

                OnRotateCam?.Invoke();

            }
        }

        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            float prev = (t1.position - t1.deltaPosition - (t2.position - t2.deltaPosition)).magnitude;
            float curr = (t1.position - t2.position).magnitude;

            float delta = curr - prev;

            ZoomRatio -= delta * zoomSpeed * 0.01f;
            ZoomRatio = Mathf.Clamp(ZoomRatio, 0, 1);
            OnZoomCam?.Invoke();

        }
    }

    public void SetZoomRatio(float target)
    {
        ZoomRatio = Mathf.Clamp(target, 0, 1);
        OnZoomCam?.Invoke();
    }

    public bool IsPointerOverPieceList()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(
            itemListRect,
            Input.mousePosition,
            null))
        {
            return true;
        }
        return false;
    }

}

