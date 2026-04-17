using UnityEngine;

public class Sender : MonoBehaviour
{
    [SerializeField] private int ID;

    private void Update()
    {
        GameManager.Instance.OnObjectChangeAngle?.Invoke(ID, Camera.main.transform.position - transform.position);
    }

}
