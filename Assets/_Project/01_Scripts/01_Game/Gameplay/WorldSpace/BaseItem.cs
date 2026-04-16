using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    [SerializeField] private List<BasePiece> Pieces;

    public int GetPieceCount() => Pieces.Count; 
}
