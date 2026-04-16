using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGamePlayManager : MonoBehaviour
{
    public int PieaceIDChoosing = -1;
    public List<int> PiecesRemain { get; private set; } = new List<int>();

    public Action OnPieaceIDChoosingChanged;
    public Action<int> OnPieceCompleted;
    public Action OnSetupModelComplete;
    public Action<int> OnGenPieceInList;

    public static CurrentGamePlayManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }
     IEnumerator Start()
    {
        yield return null;
        SpawnModel();
    }
    private void SpawnModel()
    {
        BaseItem obj = Resources.Load<BaseItem>(GameConfig.GetLevelModelTotal(GameManager.Instance.CurrentLevel));
        LeanPool.Spawn(obj, transform).transform.localPosition = Vector3.zero;

        int pieceCount = obj.GetPieceCount();
        for (int i = 1; i <= pieceCount; i++)
        {
            PiecesRemain.Add(i);
        }
        OnSetupModelComplete?.Invoke();
    }
    public int GetOrderInList(int pID)
    {
        if (PiecesRemain.Contains(pID))
        {
            return PiecesRemain.IndexOf(pID);
        }
        return -1;
    }

    public void ChoosePieceID(int pID)
    {
        if (IsPieceCompleted(pID)) return;
        PieaceIDChoosing = pID;
        OnPieaceIDChoosingChanged?.Invoke();
    }
    public bool CanMoveCam() => PieaceIDChoosing == -1;
    public bool IsPieceCompleted(int pID) => PiecesRemain.Contains(pID) == false;
    public void CompletePiece(int pID)
    {
        if (!IsPieceCompleted(pID))
        {
            PiecesRemain.Remove(pID);
            OnPieceCompleted?.Invoke(pID);
        }
        ChoosePieceID(-1);
    }
}
