using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables



    #endregion

    #region Unity Methods

    private void Start()
    {
        FindObjectOfType<LevelManager>().EventIsMapReady += GameReady;
    }

    #endregion

    #region Custom Methods

    private void GameReady(object sender, LevelManager.IsMapReadyEventArgs e) => IsGameReady(new IsGameReadyEventArgs() { isGameReady = e.isMapReady });

    #endregion

    #region Events

    public class IsGameReadyEventArgs
    {
        public bool isGameReady;
    }

    public event EventHandler<IsGameReadyEventArgs> EventIsGameReady;

    public void IsGameReady(IsGameReadyEventArgs e) => EventIsGameReady?.Invoke(this, e);

    #endregion
}