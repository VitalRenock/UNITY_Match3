using UnityEngine;
using Unity.Mathematics;

public enum Direction { Right, Up, Left, Down }

public class Cell
{
    public string _name;
    public GameObject _cellGo;
    public int2 _gridPosition;
    public LevelManager _levelManager;
    public Cell[] _adjacentCells;

    public Cell(GameObject cellGo, int2 cellPosition, LevelManager manager)
    {
        _name = cellGo ? cellGo.name : "empty";
        _cellGo = cellGo;
        _gridPosition = cellPosition;
        _levelManager = manager;
    }

    public void GetAdjacentCells()
    {
        _adjacentCells = new Cell[4];
        _adjacentCells[(int)Direction.Right] = _levelManager.GetCellFromPosition(_gridPosition.x + 1, _gridPosition.y);
        _adjacentCells[(int)Direction.Up] = _levelManager.GetCellFromPosition(_gridPosition.x, _gridPosition.y + 1);
        _adjacentCells[(int)Direction.Left] = _levelManager.GetCellFromPosition(_gridPosition.x - 1, _gridPosition.y);
        _adjacentCells[(int)Direction.Down] = _levelManager.GetCellFromPosition(_gridPosition.x, _gridPosition.y - 1);
    }
}
