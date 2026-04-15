using UnityEngine;
using DG.Tweening;
public class BasePiece : MonoBehaviour
{
    [Header("Identifying")]
    [SerializeField] private int levelID;
    [SerializeField] private int pieceID;


    [Header("Properties")]
    [SerializeField] private GameObject objectDone;
    [SerializeField] private BaseDemoPiece objectDemo;
    [SerializeField] private GameObject objectNotYet;

    private void Start()
    {
        objectDone.gameObject.SetActive(false);
        objectNotYet.gameObject.SetActive(true);
        CurrentGamePlayManager.Instance.OnPieceCompleted += OnPieceComplete;

    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieceCompleted -= OnPieceComplete;

    }
    private void OnPieceComplete(int id)
    {
        if (id == pieceID)
        {
            objectDone.transform.localScale = Vector3.one * 0.8f;
            objectDone.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

            objectDone.gameObject.SetActive(true);
            objectNotYet.gameObject.SetActive(false);
            if (objectDemo != null)
                objectDemo.gameObject.SetActive(false);
        }
    }
}
