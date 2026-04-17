using UnityEngine;
using DG.Tweening;
public class BasePiece : MonoBehaviour
{
    [Header("Identifying")]
    [SerializeField] private int pieceID;


    [Header("Properties")]
    [SerializeField] private GameObject objectDone;
    [SerializeField] private Transform objectDemo;
    [SerializeField] private GameObject objectNotYet;
    [SerializeField] private GameObject objectInList;

    bool isInList = false;
    private void OnEnable()
    {
        isInList = false;
    }
    private void Start()
    {
        if (isInList == false)
        {
            objectDone.gameObject.SetActive(false);
            objectNotYet.gameObject.SetActive(true);
        }
        Resize();
        currenCamera = Camera.main;
        camTf = Camera.main.transform;
        currentPos = demoModel.position;
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged += CheckEnable;
        minDistance = Mathf.Min(CaculateMinDistance() / 2, 0.3f);
        CheckEnable();
        CurrentGamePlayManager.Instance.OnPieceCompleted += OnPieceComplete;

    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieceCompleted -= OnPieceComplete;
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged -= CheckEnable;

    }
    private void Update()
    {
        if (isInList == false)
        {
            GameManager.Instance.SendMessage?.Invoke(pieceID, Camera.main.transform.position - transform.position);


            if (CurrentGamePlayManager.Instance.PieaceIDChoosing == pieceID)
            {

                currentPos = GetMousePointOnPlanePlusUp();
                if (TryFindPointD(objectDemo.position, currentPos, camTf.position, camTf.forward, out Vector3 D))
                {
                    currentPos = D;
                    demoModel.position = currentPos;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.LogError("Distance " + Vector3.Distance(objectDemo.position, demoModel.position) + " MinDistance " + minDistance);
                    if (Vector3.Distance(objectDemo.position, demoModel.position) < minDistance)
                    {
                        CurrentGamePlayManager.Instance.CompletePiece(pieceID);
                        Debug.LogError("Complete Piece " + pieceID);
                    }
                    else
                    {
                        Debug.LogError("Fail Piece " + pieceID);


                    }
                }
            }
        }

    }

    #region General Methods
    private void Resize()
    {


        Renderer[] renderers = objectInList.GetComponentsInChildren<Renderer>();

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        objectInList.transform.localScale = Vector3.one * 0.2f * objectInList.transform.lossyScale.x / maxSize;
    }
    private void OnPieceComplete(int id)
    {
        if (id == pieceID)
        {
            objectDone.transform.localScale = Vector3.one * 0.6f;
            objectDone.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

            objectDone.gameObject.SetActive(true);
            objectNotYet.gameObject.SetActive(false);
            if (objectDemo != null)
                objectDemo.gameObject.SetActive(false);
        }
    }

    public void DisplayInList()
    {
        objectDone.SetActive(false);
        objectDemo.gameObject.SetActive(false);
        objectNotYet.SetActive(false);
        objectInList.SetActive(true);
        isInList = true;

    }
    public int GetID() => pieceID;
    #endregion

    #region Demo Process
    [SerializeField] private Transform demoModel;
    private float minDistance;
    private Camera currenCamera;
    private Transform camTf;
    private Vector3 currentPos = Vector3.zero;

    private void CheckEnable()
    {
        demoModel.gameObject.SetActive(CurrentGamePlayManager.Instance.PieaceIDChoosing == pieceID);
    }
    private float CaculateMinDistance()
    {
        var renderers = demoModel.GetComponentsInChildren<Renderer>();
        float maxDistance = 0f;
        foreach (var renderer in renderers)
        {
            float distance = Mathf.Max(renderer.bounds.size.x, renderer.bounds.size.y, renderer.bounds.size.z);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }
        return maxDistance / 2;
    }

    public bool TryFindPointD(
       Vector3 A,
       Vector3 B,
       Vector3 C,
       Vector3 V,
       out Vector3 D)
    {
        Vector3 BC = C - B;
        float denom = Vector3.Dot(BC, V);

        // Check parallel case
        if (Mathf.Abs(denom) < 1e-6f)
        {
            D = Vector3.zero;
            return false; // no unique solution
        }

        float t = Vector3.Dot(A - B, V) / denom;
        D = B + t * BC;

        return true;
    }

    public Vector3 GetMousePointOnPlanePlusUp(float offset = 150f)
    {
        Ray ray = currenCamera.ScreenPointToRay(Input.mousePosition + Vector3.up * offset);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector3 hitPoint = hit.point;

            return hitPoint;
        }

        return Vector3.zero;
    }
    #endregion
}
