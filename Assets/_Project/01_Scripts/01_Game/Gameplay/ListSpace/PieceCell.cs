using PolyAndCode.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class PieceCell : MonoBehaviour, ICell, IPointerDownHandler
{
    [SerializeField] private List<RenderTexture> renderTectures;
    [SerializeField] private RawImage rawImage;

    //Model
    private PieceInfo _contactInfo;
    private int _cellIndex;

    private void Start()
    {
        //Can also be done in the inspector
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(PieceInfo contactInfo, int cellIndex)
    {
        _cellIndex = cellIndex;
        _contactInfo = contactInfo;

        rawImage.texture = renderTectures[(_contactInfo.id - 1) % renderTectures.Count];
        CurrentGamePlayManager.Instance.OnGenPieceInList?.Invoke(_contactInfo.id);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError(_contactInfo.id);
    }

}
