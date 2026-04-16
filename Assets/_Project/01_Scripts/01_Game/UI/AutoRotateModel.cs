using UnityEngine;

public class AutoRotateModel : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    private float currentAngle = 0;
    private void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, currentAngle, 0);
    }
}
