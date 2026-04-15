using Unity.VisualScripting;
using UnityEngine;

public class SubCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnRotate += UpdateCamera;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnRotate -= UpdateCamera;

    }

    void UpdateCamera(Vector2 rot)
    {
        Quaternion rotation = Quaternion.Euler(rot.y, rot.x, 0);

        Vector3 offset = rotation * new Vector3(0, 0, -0.8419991f);

        transform.localPosition = offset;
        transform.LookAt(transform.parent.position);
    }

}
