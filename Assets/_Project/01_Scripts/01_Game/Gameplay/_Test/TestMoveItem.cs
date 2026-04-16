using UnityEngine;

public class TestMoveItem : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform CamT;
    [SerializeField] private Transform TargetPoint;
    [SerializeField] private float MoveSpeed = 0.1f;
    private Vector3 currentPos = Vector3.zero;

    private void Update()
    {
        bool moved = false;
        if (Input.GetKey(KeyCode.W))
        {
            currentPos += Camera.transform.up * MoveSpeed * Time.deltaTime;
            moved = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moved = true;

            currentPos += Camera.transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moved = true;

            currentPos += -Camera.transform.up * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moved = true;

            currentPos += -Camera.transform.right * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(0))
        {
            moved = true;
            currentPos = GetMousePointOnPlanePlusUp();
        }

        if (moved)
        {
            if (TryFindPointD(TargetPoint.position, currentPos, Camera.transform.position, Camera.transform.forward, out Vector3 D))
            {
                currentPos = D;
                transform.position = currentPos;
            }
        }
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
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition + Vector3.up * offset);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector3 hitPoint = hit.point;

            return hitPoint;
        }

        return Vector3.zero;
    }
}
