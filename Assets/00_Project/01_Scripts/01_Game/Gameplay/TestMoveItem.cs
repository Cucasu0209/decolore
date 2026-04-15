using UnityEngine;

public class TestMoveItem : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    [SerializeField] private Transform CamT;
    [SerializeField] private Transform TargetPoint;
    [SerializeField] private float MoveSpeed = 0.1f;
    private Vector3 currentPos = Vector3.zero;

    private void Update()
    {
        bool moved = false;
        if (Input.GetKey(KeyCode.W))
        {
            currentPos += Camera.up * MoveSpeed * Time.deltaTime;
            moved = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moved = true;

            currentPos += Camera.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moved = true;

            currentPos += -Camera.up * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moved = true;

            currentPos += -Camera.right * MoveSpeed * Time.deltaTime;
        }

        if (moved)
        {
            if (TryFindPointD(TargetPoint.position, currentPos, Camera.position, Camera.forward, out Vector3 D))
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
}
