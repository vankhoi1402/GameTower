using UnityEngine;

public class FormationGrid2D : MonoBehaviour
{
    [SerializeField] private BoxCollider2D area;
    [SerializeField] private float cellSize = 1.0f;
    public FormationCell[,] Cells => _cells;
    public float CellSize => cellSize;

    private FormationCell[,] _cells;
    private int _width, _height;
    public int Width => _width;
    public int Height => _height;
    private void Awake() => GenerateGrid();

    private void GenerateGrid()
    {
        Bounds bounds = area.bounds;
        _width = Mathf.FloorToInt(bounds.size.x / cellSize);
        _height = Mathf.FloorToInt(bounds.size.y / cellSize);
        _cells = new FormationCell[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 pos = new Vector3(
                    bounds.min.x + (x * cellSize) + (cellSize / 2f),
                    bounds.min.y + (y * cellSize) + (cellSize / 2f),
                    0
                );
                _cells[x, y] = new FormationCell(new Vector2Int(x, y), pos);
            }
        }
    }
    // Lấy cell theo grid coordinate
    public FormationCell GetCell(int x, int y)
    {
        if (x < 0 || x >= _width ||
            y < 0 || y >= _height)
        {
            return null;
        }

        return _cells[x, y];
    }

    public FormationCell GetCellFromWorld(Vector2 worldPos)
    {
        if (area == null || !area.OverlapPoint(worldPos)) return null;

        int x = Mathf.FloorToInt((worldPos.x - area.bounds.min.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - area.bounds.min.y) / cellSize);

        if (x >= 0 && x < _width && y >= 0 && y < _height) return _cells[x, y];
        return null;
    }
}