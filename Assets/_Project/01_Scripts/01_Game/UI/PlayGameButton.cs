using UnityEngine;
using UnityEngine.UI;

public class PlayGameButton : MonoBehaviour
{
    [SerializeField] private Button PlayBtn;
    void Start()
    {
        PlayBtn.onClick.AddListener(UIManager.Instance.LoadSceneGameplay);
    }

   
}
