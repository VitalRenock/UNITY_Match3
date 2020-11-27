#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class LevelManager : MonoBehaviour
{
    #region Variables

    // Declaration for grid and cells parameters (column & row visible in Unity)
    [SerializeField] private int2 _columnAndRow;
    private Vector2 _cellSize, _cellExtent, _gridSize, _gridExtent;
    public Cell[] _cells;

    // Declaration for Level (Array of LevelData -> Scriptable Objects) and Sprites (Array initialized in Unity)
    [SerializeField] private LevelData[] _levels;
    [SerializeField] private Sprite[] _pieceSprites;

    private bool _isSwapping;
    private int2 _firstPiecePositionMatching;
    private int2 _secondPiecePsotionMatching;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        // Initialise grid size variables beside column and row values
        _gridSize.x = _columnAndRow.x + 2.5f;
        _gridSize.y = _columnAndRow.y + 2.5f;
    }

    private void Start()
    {
        // Call a custom method to calculate cell size and offset
        InitializeCellSize();

        // Call custom method to generate the grid
        GenerateGrid();

        FindObjectOfType<GameplayManager>().EventOnSwap += IsSwapping;
    }

    private void IsSwapping(object sender, GameplayManager.OnSwapEventArgs e)
    {
        _isSwapping = e.isSwapping;
        _firstPiecePositionMatching = e.firstPiecePosition;
        _secondPiecePsotionMatching = e.secondPiecePosition;
    }

    private void Update()
    {
        if (_isSwapping)
        {
            StartCoroutine(Matching());
        }
    }

    #endregion

    #region Custom Methods

    private void GenerateGrid()
    {
        // Fill the cells array with new Cell instance and define the array lenght beside of column/row
        _cells = new Cell[_columnAndRow.x * _columnAndRow.y];

        // Double loop system to go through each future cell of the grid ( vertically -> horizontally )
        for (int x = 0; x < _columnAndRow.x; x++)
        {
            for (int y = 0; y < _columnAndRow.y; y++)
            {
                // Call a custom method to create/place/render each cell with right position
                CreateTile(x, y);
            }
        }

        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i].GetAdjacentCells();
        }

        // Trigger an event to warn that the grid is ready (send EventArgs to the GameManager subscribe)
        IsMapReady(new IsMapReadyEventArgs() { isMapReady = true });
    }

    private void InitializeCellSize()
    {
        // Calculate a cell size (x,y) beside of grid size and numbers of columns/rows
        _cellSize.x = _gridSize.x / _columnAndRow.x;
        _cellSize.y = _gridSize.y / _columnAndRow.y;

        // Calculate the offset to replace future cell and entire grid at the right position
        _cellExtent = _cellSize * 0.5f;
        _gridExtent = _gridSize * 0.5f;
    }

    private void CreateTile(int x, int y)
    {
        // Call a custom method to get the cell index from the grid position
        int tileIndex = GridPositionToIndex(x, y);

        // Declaration of a new gameobject
        GameObject go = new GameObject();

        // Call a custom method to add a sprite to the gameobject and return the spritename
        string spriteName = AddSpriteRendererToCell(go, tileIndex, x, y);

        // Initialize name/parent/position gameobejct properties
        go.name = spriteName;
        go.transform.parent = GameObject.Find("Tiles").transform;
        go.transform.position = GridPositionToWorldPosition(x, y);

        // Fill the cells array with a each cell corresponding with each gameobject from the grid
        _cells[tileIndex] = new Cell(go, new int2(x, y), this);
    }

    private string AddSpriteRendererToCell(GameObject go, int tileIndex, int xPosition, int yPosition)
    {
        List<Sprite> possibleJewels = new List<Sprite>();
        possibleJewels.AddRange(_pieceSprites);
        possibleJewels.Remove(GetCellFromPosition(xPosition, yPosition - 1)?._cellGo?.GetComponent<SpriteRenderer>()?.sprite);
        possibleJewels.Remove(GetCellFromPosition(xPosition - 1, yPosition)?._cellGo?.GetComponent<SpriteRenderer>()?.sprite);
        Sprite tileSprite = tileIndex == _levels[InfoManager._chooseALevel]._spawnSquare ? possibleJewels[0] : possibleJewels[UnityEngine.Random.Range(1, possibleJewels.Count)];
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = tileSprite;
        return tileSprite.name;
    }

    private IEnumerator Matching()
    {
        Cell PieceA = GetCellFromPosition(_firstPiecePositionMatching);
        Cell PieceB = GetCellFromPosition(_secondPiecePsotionMatching);

        Debug.Log(PieceA._cellGo + " " + PieceA._gridPosition);
        Debug.Log(PieceB._cellGo + " " + PieceB._gridPosition);



        _isSwapping = false;
        yield return null;
    }

    public Cell GetCellFromPosition(int2 position) => GetCellFromPosition(position.x, position.y);

    public Cell GetCellFromPosition(int xPosition, int yPosition) => xPosition >= 0 && xPosition < _columnAndRow.x && yPosition >= 0 && yPosition < _columnAndRow.y ? _cells[GridPositionToIndex(xPosition, yPosition)] : null;

    public int GridPositionToIndex(int xPosition, int yPosition) => (xPosition * _columnAndRow.y + yPosition);

    public Vector2 GridPositionToWorldPosition(int xPosition, int yPosition) => ((new Vector2(xPosition, yPosition) * _cellSize) + _cellExtent - _gridExtent) + (Vector2)transform.position;

    public int2 WorldToGridPosition(Vector2 position)
    {
        Vector2 localPosition = (Vector2)this.transform.InverseTransformPoint(position) + _gridExtent;
        return new int2(Mathf.FloorToInt(localPosition.x / _cellSize.x), Mathf.FloorToInt(localPosition.y / _cellSize.y));
    }

    public int2 ClampPositionToGrid(int xPosition, int yPosition)
    {
        return new int2(Mathf.Clamp(xPosition, 0, _columnAndRow.x - 1), Mathf.Clamp(yPosition, 0, _columnAndRow.y - 1));
    }

    public int2 ClampPositionToGrid(int2 position)
    {
        return ClampPositionToGrid(position.x, position.y);
    }

    #endregion

    #region Events

    public class IsMapReadyEventArgs
    {
        public bool isMapReady;
    }

    public event EventHandler<IsMapReadyEventArgs> EventIsMapReady;

    public void IsMapReady(IsMapReadyEventArgs e) => EventIsMapReady?.Invoke(this, e);

    #endregion
}

#pragma warning restore 0649