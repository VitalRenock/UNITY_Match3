//#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GameplayManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelManager _levelManager;
    private Cell _firstCellSelected;
    private Vector2 _mousePosition;
    private bool _isLeftClicked;
    private bool _isPieceSelected;

    #endregion

    #region Unity Methods

    private void Start()
    {
        FindObjectOfType<InputManager>().EventIsClicked += DetectClick;
    }

    private void Update()
    {
        if (_isLeftClicked)
        {
            Cell cell = _levelManager.GetCellFromPosition(_levelManager.WorldToGridPosition(_mousePosition));
            _isLeftClicked = false;

            if (!_isPieceSelected && cell != null)
            {
                _firstCellSelected = cell;
                _firstCellSelected._cellGo.transform.localScale += new Vector3(1, 1, 0);
                _isPieceSelected = true;
                OnSoundTrigger(new OnSoundTriggerEventArgs { sound = "Selected" });
            }

            else if (_isPieceSelected || (cell == _firstCellSelected && _firstCellSelected != null))
            {
                _firstCellSelected._cellGo.transform.localScale -= new Vector3(1, 1, 0);

                if (((IList<Cell>)_firstCellSelected._adjacentCells).Contains(cell) && cell != null)
                {
                    StartCoroutine(Switching(_firstCellSelected, cell));
                }

                _firstCellSelected = null;
                _isPieceSelected = false;
            }
        }
    }

    #endregion

    #region Custom Methods

    private void DetectClick(object sender, InputManager.IsClickedEventArgs e)
    {
        _isLeftClicked = e.isClicked;
        _mousePosition = e.mouseWorldPosition;
    }

    IEnumerator Switching(Cell firstCellToSwap, Cell secondCellToSwap)
    {
        firstCellToSwap._cellGo.transform.position = _levelManager.GridPositionToWorldPosition(secondCellToSwap._gridPosition.x, secondCellToSwap._gridPosition.y);
        secondCellToSwap._cellGo.transform.position = _levelManager.GridPositionToWorldPosition(firstCellToSwap._gridPosition.x, firstCellToSwap._gridPosition.y);

        GameObject tempGo = firstCellToSwap._cellGo;

        firstCellToSwap._cellGo = secondCellToSwap._cellGo;
        secondCellToSwap._cellGo = tempGo;

        OnSwap(new OnSwapEventArgs() { isSwapping = true, firstPiecePosition = firstCellToSwap._gridPosition, secondPiecePosition = secondCellToSwap._gridPosition });

        yield return null;
    }

    #endregion

    #region Events

    public class OnSwapEventArgs
    {
        public bool isSwapping;
        public int2 firstPiecePosition;
        public int2 secondPiecePosition;
    }

    public event EventHandler<OnSwapEventArgs> EventOnSwap;

    public void OnSwap(OnSwapEventArgs e) => EventOnSwap?.Invoke(this, e);


    public class OnSoundTriggerEventArgs
    {
        public string sound;
    }

    public event EventHandler<OnSoundTriggerEventArgs> EventOnSoundTrigger;

    public void OnSoundTrigger(OnSoundTriggerEventArgs e) => EventOnSoundTrigger?.Invoke(this, e);

    #endregion
}

//#pragma warning restore 0649
