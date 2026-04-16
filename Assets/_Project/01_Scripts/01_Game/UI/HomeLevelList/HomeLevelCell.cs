using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.UI;

public class HomeLevelCell : MonoBehaviour, ICell
{
    //UI
    [SerializeField] private HomeLevelCellBox cellBox1;
    [SerializeField] private HomeLevelCellBox cellBox2;
    [SerializeField] private GameObject commingSoon;

    //Model
    private LevelHomeInfo data;
    private int cellIndex;

    private void Start()
    {
        //Can also be done in the inspector
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(LevelHomeInfo _data, int _cellIndex)
    {
        cellIndex = _cellIndex;
        data = _data;

        if(data.id * 2 + 1 > GameConfig.CURRENT_MAX_LEVEL)
        {
            cellBox1.gameObject.SetActive(false);
            cellBox2.gameObject.SetActive(false);
            commingSoon.gameObject.SetActive(true);
        }
        else
        {
            cellBox1.gameObject.SetActive(true);
            cellBox2.gameObject.SetActive(true);
            commingSoon.gameObject.SetActive(false);
            cellBox1.SetLevel(data.id * 2 + 1);
            cellBox2.SetLevel(data.id * 2 + 2);
        }
   
    }

}
