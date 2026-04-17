using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGamePlayManager : MonoBehaviour
{
    public static CurrentGamePlayManager Instance { get; private set; }
    public int CurrentPieaceID { get; private set; } = -1;
    public List<int> PiecesRemain { get; private set; } = new List<int>();
    public int TotalPiece { get; private set; }

    #region Actions
    public Action OnPieceChange;
    public Action<int> OnPieceComplete;
    public Action OnStartGame;
    public Action<int> OnPieceInListAppear;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Instance = this;
    }
    IEnumerator Start()
    {
        yield return null;
        SpawnModel();
    }
    #endregion


    #region Setup Start game
    private void SpawnModel()
    {
        BaseItem obj = Resources.Load<BaseItem>(GameConfig.GetLevelModelTotal(GameManager.Instance.CurrentLevel));
        LeanPool.Spawn(obj, transform).transform.localPosition = Vector3.zero;

        TotalPiece = obj.GetPieceCount();
        for (int i = 1; i <= TotalPiece; i++)
        {
            PiecesRemain.Add(i);
        }
        StartGame();
    }
    private void StartGame()
    {
        OnStartGame?.Invoke();
    }
    #endregion


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
        if (IsPieceCompleted(pID) && pID > 0) return;
        CurrentPieaceID = pID;
        OnPieceChange?.Invoke();
    }


    #region In Game Methods
    public bool CanMoveCam() => CurrentPieaceID == -1;
    public bool IsPieceCompleted(int pID) => PiecesRemain.Contains(pID) == false;
    public void CompletePiece(int pID)
    {
        if (!IsPieceCompleted(pID))
        {
            PiecesRemain.Remove(pID);
            OnPieceComplete?.Invoke(pID);
        }
        ChoosePieceID(-1);
    }
    #endregion
}
