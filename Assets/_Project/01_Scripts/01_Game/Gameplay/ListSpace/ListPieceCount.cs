using TMPro;
using UnityEngine;

public class ListPieceCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainCount;
    [SerializeField] private TextMeshProUGUI totalCount;
    private void Start()
    {
        CurrentGamePlayManager.Instance.OnPieceCompleted += UpdateText;
        CurrentGamePlayManager.Instance.OnSetupModelComplete += UpdateText;
    }
    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnPieceCompleted -= UpdateText;
        CurrentGamePlayManager.Instance.OnSetupModelComplete -= UpdateText;
    }
    private void UpdateText()
    {
        remainCount.SetText(CurrentGamePlayManager.Instance.PiecesRemain.Count.ToString());
        totalCount.SetText(CurrentGamePlayManager.Instance.TotalPiece.ToString());
    }
    private void UpdateText(int id)
    {
        UpdateText();
    }
}
