using UnityEngine;

public class FormationCell
{
    public Vector2Int GridPosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public bool Occupied { get; set; }

    public FormationCell(Vector2Int gridPos, Vector3 worldPos)
    {
        GridPosition = gridPos;
        WorldPosition = worldPos;
        Occupied = false;
    }
}