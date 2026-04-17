using UnityEngine;
using UnityEngine.UI;

public class Test_ClinkChooseItem : MonoBehaviour
{
    [SerializeField] private int pieceID;
    [SerializeField] private Button btn;
    private void Start()
    {
        btn.onClick.AddListener(() => CurrentGamePlayManager.Instance.ChoosePieceID(pieceID));
        CurrentGamePlayManager.Instance.OnPieceChange += OnIDChange;
        CurrentGamePlayManager.Instance.OnPieceComplete += OnIDChange;
        OnIDChange();
        OnIDChange(1);
    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieceChange -= OnIDChange;
        CurrentGamePlayManager.Instance.OnPieceComplete -= OnIDChange;

    }
    private void OnIDChange()
    {
        btn.image.color = CurrentGamePlayManager.Instance.CurrentPieaceID == pieceID ? Color.red : Color.white;
    }
    private void OnIDChange(int id)
    {
        btn.transform.localScale = CurrentGamePlayManager.Instance.IsPieceCompleted(pieceID) ? Vector2.zero : Vector2.one;
        btn.image.color = CurrentGamePlayManager.Instance.CurrentPieaceID == pieceID ? Color.red : Color.white;
    }
}
