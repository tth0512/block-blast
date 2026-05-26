using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Ins { get { return _instance; } }

    private Camera mainCamera;
    public Block CurrentBlock;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        mainCamera = Camera.main;
    }

    private void OnMouseUp()
    {
        CurrentBlock = null;
    }
}
