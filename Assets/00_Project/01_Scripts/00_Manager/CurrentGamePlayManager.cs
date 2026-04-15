using UnityEngine;

public class CurrentGamePlayManager : MonoBehaviour
{
    public static CurrentGamePlayManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
