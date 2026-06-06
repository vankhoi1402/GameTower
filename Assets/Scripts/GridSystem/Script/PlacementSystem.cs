using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlacementSystem : MonoBehaviour
{
    [Header("Data & UI")]
    [SerializeField] private List<UnitData> availableUnits;
    [SerializeField] private UIDocument uiDocument;

    [Header("References")]
    [SerializeField] private FormationGrid2D grid;
    [SerializeField] private GameObject previewObject; // Cái bóng mờ

    private UnitData _selectedUnit;
    private FormationCell _currentCell;

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        var bar = root.Q<VisualElement>("inventory-bar");
        bar.Clear();

        foreach (var unit in availableUnits)
        {
            Button btn = new Button();
            btn.AddToClassList("unit-button");
            btn.style.backgroundImage = new StyleBackground(unit.icon);

            // Đăng ký sự kiện chọn quân
            btn.clicked += () => BeginPlacement(unit);
            bar.Add(btn);
        }
    }

    private void Update()
    {
        if (_selectedUnit == null)
        {
            if (previewObject.activeSelf) previewObject.SetActive(false);
            return;
        }

        UpdateTargetCell();
        HandleVisuals();
        HandleInput();
    }

    private void BeginPlacement(UnitData data)
    {
        _selectedUnit = data;
    }

    private void UpdateTargetCell()
    {
        // Cách lấy vị trí chuột mới của Input System
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        _currentCell = grid.GetCellFromWorld(mouseWorldPos);
    }

    private void HandleVisuals()
    {
        if (_currentCell == null)
        {
            previewObject.SetActive(false);
            return;
        }

        previewObject.SetActive(true);
        previewObject.transform.position = _currentCell.WorldPosition;

        // Thay đổi màu sắc preview nếu ô đã bị chiếm (tùy chọn)
        var spriteRenderer = previewObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = _currentCell.Occupied ? new Color(1, 0, 0, 0.5f) : new Color(1, 1, 1, 0.5f);
        }
    }

    private void HandleInput()
    {
        // Chuột trái để đặt
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_currentCell != null && !_currentCell.Occupied)
            {
                // SỬA Ở ĐÂY: Thêm điều kiện kiểm tra Capacity
                if (ArmyManager.Instance.CanPlaceUnit(_selectedUnit))
                {
                    PlaceUnit();
                }
                else
                {
                    Debug.Log("Không đủ chỉ số quân (Capacity) để đặt thêm!");
                    // (Tùy chọn) Thêm âm thanh báo lỗi hoặc hiệu ứng rung UI tại đây
                }
            }
        }

        // Chuột phải để hủy
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
        }
    }

    private void PlaceUnit()
    {
        // 1. Chốt khóa ô cờ
        _currentCell.Occupied = true;

        // 2. Trừ tài nguyên quân số
        ArmyManager.Instance.ConsumeCapacity(_selectedUnit);

        // 3. SỬA CHÍNH: Bàn giao hoàn toàn việc Instantiate cho SpawnManager
        SpawnManager.Instance.SpawnUnit(_selectedUnit, _currentCell.WorldPosition, TeamType.Player);
        // phat sound
        BattleEvents.RaisePlaySound3D(SoundType.Battle_Place_Unit, _currentCell.WorldPosition);
        // Hủy chọn sau khi đặt (hoặc giữ nguyên nếu muốn đặt liên tục nhiều lính cùng loại)
        CancelPlacement();
    }

    private void CancelPlacement()
    {
        _selectedUnit = null;
        previewObject.SetActive(false);
    }
}