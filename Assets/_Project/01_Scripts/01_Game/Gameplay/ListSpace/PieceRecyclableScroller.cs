using Lean.Pool;
using PolyAndCode.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

//Dummy Data model for demostraion
public struct PieceInfo
{
    public int id;
}

public class PieceRecyclableScroller : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;


    //Dummy data List
    private List<PieceInfo> _contactList = new List<PieceInfo>();

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Start()
    {
        CurrentGamePlayManager.Instance.OnSetupModelComplete += InitData;
        CurrentGamePlayManager.Instance.OnPieceCompleted += OnDataChange;
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged += StopScroll;

        _recyclableScrollRect.DataSource = this;
    }

    private void OnDestroy()
    {
        CurrentGamePlayManager.Instance.OnSetupModelComplete -= InitData;
        CurrentGamePlayManager.Instance.OnPieceCompleted -= OnDataChange;
        CurrentGamePlayManager.Instance.OnPieaceIDChoosingChanged -= StopScroll;
    }

    private void OnDataChange(int id)
    {
        InitData();
        _recyclableScrollRect.ReloadData();
    }

    private void StopScroll()
    {

        _recyclableScrollRect.StopMovement();              // stop inertia
        _recyclableScrollRect.velocity = Vector2.zero;     // clear velocity

        EventSystem.current.SetSelectedGameObject(null);

        // Force reset drag state (hack nhưng hiệu quả)
        ExecuteEvents.Execute<IEndDragHandler>(
            _recyclableScrollRect.gameObject,
            new PointerEventData(EventSystem.current),
            ExecuteEvents.endDragHandler
        );
    }
    private void InitData()
    {
        if (_contactList != null) _contactList.Clear();


        for (int i = 0; i < CurrentGamePlayManager.Instance.PiecesRemain.Count; i++)
        {
            _contactList.Add(new PieceInfo() { id = CurrentGamePlayManager.Instance.PiecesRemain[i] });
        }

    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _contactList.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as PieceCell;
        item.ConfigureCell(_contactList[index], index);
    }

    #endregion
}