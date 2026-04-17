using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDemoController : MonoBehaviour
{
    public static DragDemoController Instance { get; private set; }

    private int cachePieceID = -1;
    private float lastTimeCache = -1;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CurrentGamePlayManager.Instance.OnPieceChange += OnIdChoosingChange;
    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieceChange -= OnIdChoosingChange;

    }

    private void OnIdChoosingChange()
    {
        cachePieceID = -1;
        lastTimeCache = -1;
    }

    public void SetCacheDrag(int pieceID)
    {
        if (CurrentGamePlayManager.Instance.CurrentPieaceID == -1)
        {
            cachePieceID = pieceID;
            lastTimeCache = Time.time;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE

        if (cachePieceID != -1 && CurrentGamePlayManager.Instance.CurrentPieaceID == -1 && Time.time - lastTimeCache > 0.2f)
        {
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {
                    CurrentGamePlayManager.Instance.ChoosePieceID(cachePieceID);

                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(DelayResetID());
        }

#endif

        //if (Input.touchCount >0 )
        //{
        //    for (int i = 0; i < Input.touchCount; i++)
        //    {
        //        Touch t = Input.GetTouch(i);
        //        if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
        //        {
        //            Debug.Log("Touch vào UI");
        //        }
        //    }
        //}


    }
    IEnumerator DelayResetID()
    {
        yield return null;
        CurrentGamePlayManager.Instance.ChoosePieceID(-1);
    }
}
