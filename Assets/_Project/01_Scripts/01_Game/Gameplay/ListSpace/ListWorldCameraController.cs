using Lean.Pool;
using UnityEngine;

public class ListWorldCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTf;

    [SerializeField] private int order;

    private BasePiece currentPiece;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.SendMessage += ReceiveMessage;

        CurrentGamePlayManager.Instance.OnGenPieceInList += GenPiece;
    }

    private void OnDestroy()
    {
        GameManager.Instance.SendMessage -= ReceiveMessage;
        CurrentGamePlayManager.Instance.OnGenPieceInList -= GenPiece;

    }
    private void GenPiece(int pieceID)
    {
        if (order >= CurrentGamePlayManager.Instance.PiecesRemain.Count)
        {
            if (currentPiece != null) LeanPool.Despawn(currentPiece);
            return;
        }


        if (CurrentGamePlayManager.Instance.GetOrderInList(pieceID) % GameConfig.MAX_CAMERA_COUNT != order) return;
        if (currentPiece != null) LeanPool.Despawn(currentPiece);
        BasePiece piece = Resources.Load<BasePiece>(GameConfig.GetLevelModelPiece(GameManager.Instance.CurrentLevel, pieceID));
        currentPiece = LeanPool.Spawn(piece, transform);
        currentPiece.transform.localPosition = Vector3.zero;
        currentPiece.DisplayInList();
    }

    private void ReceiveMessage(int ID, Vector3 Campos)
    {
        if (currentPiece != null && ID == currentPiece.GetID())
        {
            cameraTf.localPosition = Campos.normalized;
            cameraTf.LookAt(transform.position);
        }
    }
}
