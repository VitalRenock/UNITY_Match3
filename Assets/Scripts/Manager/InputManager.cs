using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region Variables

    [SerializeField] PauseMenu _manager;
    private bool _isGameStarted;
    private Camera _camera;

    #endregion

    #region Unity Method

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        FindObjectOfType<GameManager>().EventIsGameReady += IsGameStarted;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isGameStarted && !_manager._paused)
        {
            IsClicked(new IsClickedEventArgs() { isClicked = true, mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition) });
        }
    }

    #endregion

    #region Custom Methods

    private void IsGameStarted(object sender, GameManager.IsGameReadyEventArgs e) => _isGameStarted = e.isGameReady;

    #endregion

    #region Events

    public class IsClickedEventArgs
    {
        public bool isClicked;
        public Vector2 mouseWorldPosition;
    }

    public event EventHandler<IsClickedEventArgs> EventIsClicked;

    public void IsClicked(IsClickedEventArgs e) => EventIsClicked?.Invoke(this, e);

    #endregion
}
