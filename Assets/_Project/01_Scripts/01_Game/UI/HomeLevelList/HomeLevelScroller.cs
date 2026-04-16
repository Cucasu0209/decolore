using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

//Dummy Data model for demostraion
public struct LevelHomeInfo
{
    public int id;
}
public class HomeLevelScroller : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    //Dummy data List
    private List<LevelHomeInfo> _data = new List<LevelHomeInfo>();

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        InitData();
        _recyclableScrollRect.DataSource = this;
    }

    //Initialising _contactList with dummy data 
    private void InitData()
    {
        if (_data != null) _data.Clear();

        for (int i = 0; i < GameConfig.CURRENT_MAX_LEVEL / 2 + 1; i++)
        {
            LevelHomeInfo obj = new LevelHomeInfo();
            obj.id = i;
            _data.Add(obj);
        }
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _data.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as HomeLevelCell;
        item.ConfigureCell(_data[index], index);
    }

    #endregion
}