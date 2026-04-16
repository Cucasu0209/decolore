using UnityEngine;

public class BaseDemoPiece : MonoBehaviour
{
    [SerializeField] private int pieceID;
    [SerializeField] private Transform root;
    [SerializeField] private Transform demoModel;
    private float minDistance;
    private Camera currenCamera;
    private Transform camTf;
    private Vector3 currentPos = Vector3.zero;
    private void Start()
    {
        currenCamera = Camera.main;
        camTf = Camera.main.transform;
        currentPos = demoModel.position;
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged += CheckEnable;
        minDistance = Mathf.Min(CaculateMinDistance() / 2, 0.3f);
        CheckEnable();
    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged -= CheckEnable;

    }
    private void CheckEnable()
    {
        demoModel.gameObject.SetActive(CurrentGamePlayManager.Instance.PieaceIDChoosing == pieceID);
    }
    private void Update()
    {
        if (CurrentGamePlayManager.Instance.PieaceIDChoosing == pieceID)
        {

            currentPos = GetMousePointOnPlanePlusUp();
            if (TryFindPointD(root.position, currentPos, camTf.position, camTf.forward, out Vector3 D))
            {
                currentPos = D;
                demoModel.position = currentPos;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Vector3.Distance(root.position, demoModel.position) < minDistance)
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
}
