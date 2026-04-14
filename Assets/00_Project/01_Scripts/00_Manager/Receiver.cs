using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField] private int ID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.SendMessage += ReceiveMessage;
    }

    private void OnDestroy()
    {
        GameManager.Instance.SendMessage -= ReceiveMessage;

    }

    private void ReceiveMessage(int ID, Vector3 Campos)
    {
        if (ID == this.ID)
        {
            transform.localPosition = Campos.normalized * 1.7f;
            transform.LookAt(transform.parent.position);
        }
    }
}
