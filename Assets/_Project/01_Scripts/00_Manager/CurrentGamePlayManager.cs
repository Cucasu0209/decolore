using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGamePlayManager : MonoBehaviour
{
    public int CurrentLevelID = 1;
    public int PieaceIDChoosing = -1;
    private List<int> IdCompleted = new List<int>();

    public Action OnPieaceIDChoosingChanged;
    public Action<int> OnPieceCompleted;
    public static CurrentGamePlayManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ChoosePieceID(int pID)
    {
        if (IsPieceCompleted(pID)) return;
        PieaceIDChoosing = pID;
        OnPieaceIDChoosingChanged?.Invoke();
    }
    public bool CanMoveCam() => PieaceIDChoosing == -1;
    public bool IsPieceCompleted(int pID) => IdCompleted.Contains(pID);
    public void CompletePiece(int pID)
    {
        if (!IsPieceCompleted(pID))
        {
            IdCompleted.Add(pID);
            OnPieceCompleted?.Invoke(pID);
        }
        ChoosePieceID(-1);
    }
}
