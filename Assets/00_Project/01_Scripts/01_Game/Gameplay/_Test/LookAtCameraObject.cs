using UnityEngine;

public class LookAtCameraObject : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    private void Update()
    {
        transform.LookAt(Camera);
    }
}
