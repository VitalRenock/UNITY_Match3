using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/New Level")]

public class LevelData : ScriptableObject
{
    public int _spawnSquare, _endSquare;
}
