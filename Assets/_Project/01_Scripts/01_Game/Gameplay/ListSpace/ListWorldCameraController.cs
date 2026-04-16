using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class ListWorldCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTf;
    [SerializeField] private Transform pieceParent;

    [SerializeField] private int order;

    private BasePiece currentPiece;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.SendMessage += ReceiveMessage;

        CurrentGamePlayManager.Instance.OnGenPieceInList += GenPiece;
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged += OnPieceChange;
    }

    private void OnDestroy()
    {
        GameManager.Instance.SendMessage -= ReceiveMessage;
        CurrentGamePlayManager.Instance.OnGenPieceInList -= GenPiece;
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged -= OnPieceChange;

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
        currentPiece = LeanPool.Spawn(piece, pieceParent);
        currentPiece.transform.localPosition = Vector3.zero;
        currentPiece.DisplayInList();
    }

    private void OnPieceChange()
    {
        if (currentPiece != null)
        {
            if (CurrentGamePlayManager.Instance.PieaceIDChoosing == currentPiece.GetID())
            {
                pieceParent.DOKill();
                pieceParent.DOScale(0, 0.3f);
            }
        }
        if (CurrentGamePlayManager.Instance.PieaceIDChoosing < 0)
        {
            pieceParent.DOKill();
            pieceParent.DOScale(1, 0.3f);
        }
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
