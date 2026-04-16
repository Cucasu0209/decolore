using UnityEngine;
using DG.Tweening;
public class BasePiece : MonoBehaviour
{
    [Header("Identifying")]
    [SerializeField] private int pieceID;


    [Header("Properties")]
    [SerializeField] private GameObject objectDone;
    [SerializeField] private BaseDemoPiece objectDemo;
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

        CurrentGamePlayManager.Instance.OnPieceCompleted += OnPieceComplete;


        Renderer[] renderers = objectInList.GetComponentsInChildren<Renderer>();

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        objectInList.transform.localScale = Vector3.one * 0.2f / maxSize;
    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieceCompleted -= OnPieceComplete;

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
    private void Update()
    {
        if (isInList == false)
        {
            GameManager.Instance.SendMessage?.Invoke(pieceID, Camera.main.transform.position - transform.position);
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
}
