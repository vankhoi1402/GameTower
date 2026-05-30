using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class FormationGridUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FormationGrid2D grid2D;
    [SerializeField] private Camera mainCamera;

    private UIDocument _uiDocument;
    private VisualElement _gridContainer; // Tham chiếu đến #grid trong UXML
    private VisualElement[,] _uiCells;

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();

        // Tìm element có tên "grid" trong UXML để làm nơi chứa các ô
        _gridContainer = _uiDocument.rootVisualElement.Q<VisualElement>("grid");

        if (mainCamera == null) mainCamera = Camera.main;

        CreateUICells();
    }

    private void CreateUICells()
    {
        if (grid2D == null || grid2D.Cells == null || _gridContainer == null) return;

        int width = grid2D.Width;
        int height = grid2D.Height;
        _uiCells = new VisualElement[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                VisualElement cellUI = new VisualElement();

                // Thêm class từ USS vào Element thay vì code cứng
                cellUI.AddToClassList("grid-cell");

                _gridContainer.Add(cellUI);
                _uiCells[x, y] = cellUI;
            }
        }
    }

    private void LateUpdate()
    {
        if (_uiCells == null || grid2D.Cells == null) return;

        float halfCellSize = grid2D.CellSize / 2f;
        Vector3 cellCenter = grid2D.Cells[0, 0].WorldPosition;

        Vector3 minWorld = cellCenter - new Vector3(halfCellSize, halfCellSize, 0);
        Vector3 maxWorld = cellCenter + new Vector3(halfCellSize, halfCellSize, 0);

        Vector2 minScreen = mainCamera.WorldToScreenPoint(minWorld);
        Vector2 maxScreen = mainCamera.WorldToScreenPoint(maxWorld);

        minScreen.y = Screen.height - minScreen.y;
        maxScreen.y = Screen.height - maxScreen.y;

        Vector2 minPanel = RuntimePanelUtils.ScreenToPanel(_uiDocument.rootVisualElement.panel, minScreen);
        Vector2 maxPanel = RuntimePanelUtils.ScreenToPanel(_uiDocument.rootVisualElement.panel, maxScreen);

        float uiWidth = Mathf.Abs(maxPanel.x - minPanel.x);
        float uiHeight = Mathf.Abs(maxPanel.y - minPanel.y);

        for (int x = 0; x < grid2D.Width; x++)
        {
            for (int y = 0; y < grid2D.Height; y++)
            {
                FormationCell cell = grid2D.Cells[x, y];
                VisualElement uiCell = _uiCells[x, y];

                Vector3 screenPos = mainCamera.WorldToScreenPoint(cell.WorldPosition);
                screenPos.y = Screen.height - screenPos.y;

                Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(_uiDocument.rootVisualElement.panel, screenPos);

                // Cập nhật vị trí và kích thước
                uiCell.style.left = panelPos.x - (uiWidth / 2f);
                uiCell.style.top = panelPos.y - (uiHeight / 2f);
                uiCell.style.width = uiWidth;
                uiCell.style.height = uiHeight;

                // Tự động thêm/xóa class "grid-cell--occupied" dựa trên biến Occupied
                // EnableInClassList là tính năng rất mạnh của UI Toolkit
                uiCell.EnableInClassList("grid-cell--occupied", cell.Occupied);
            }
        }
    }
}