using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.Port;

public class PreparePanel : UIPanel
{
    private Button _btnStart;
    private Label _lblCapacity;

    public PreparePanel(VisualElement root, UIManager manager, string elementName)
        : base(root, manager, elementName)
    {
        _btnStart = RootElement.Q<Button>("ready-button");
        // ---- THÊM MỚI: Tìm nhãn capacity-label trong file UXML ----
        _lblCapacity = RootElement.Q<Label>("capacity-label");

        if (_btnStart == null)
        {
            Debug.LogError($"[PreparePanel] Lỗi: Không tìm thấy 'ready-button' bên trong {elementName}. Hãy kiểm tra lại file UXML!");
        }
    }

    public override void Show()
    {
        base.Show();
        if (_btnStart != null)
        {
            _btnStart.clicked += OnStartClicked;

            // ---- KHU VỰC THÊM MỚI ----
            // 1. Vừa mở panel lên thì cập nhật ngay trạng thái nút dựa vào sân cờ hiện tại
            if (BattleManager.Instance != null)
            {
                UpdateButtonState(BattleManager.Instance.HasPlayerUnits);
            }
            else
            {
                UpdateButtonState(false); // Đề phòng trường hợp chưa có BattleManager
            }

            // 2. Lắng nghe thay đổi khi người chơi đặt thêm hoặc gỡ bớt quân
            GlobalEventBus.OnPlayerUnitsAvailabilityChanged += UpdateButtonState;
        }
        // ----KHU VỰC THÊM MỚI CỦA CAPACITY LABEL ----
        if (_lblCapacity != null)
        {
            // 1. Lắng nghe thay đổi quân số khi người chơi đặt/hủy lính
            GlobalEventBus.OnPlacementCapacityChanged += UpdateCapacityUI;

            // 2. Cập nhật số liệu chuẩn ngay khi vừa mở Panel lên
            if (ArmyManager.Instance != null)
            {
                UpdateCapacityUI(ArmyManager.Instance.CurrentUsedCapacity, ArmyManager.Instance.MaxCapacity);
            }
            else
            {
                _lblCapacity.text = "QUÂN SỐ: 0/0";
            }
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (_btnStart != null)
        {
            _btnStart.clicked -= OnStartClicked;

            // ---- KHU VỰC THÊM MỚI ----
            // 3. Hủy lắng nghe để tránh lỗi tràn bộ nhớ (Memory Leak)
            GlobalEventBus.OnPlayerUnitsAvailabilityChanged -= UpdateButtonState;
        }
        // ---- KHU VỰC THÊM MỚI: Hủy đăng ký để tránh tràn bộ nhớ ----
        if (_lblCapacity != null)
        {
            GlobalEventBus.OnPlacementCapacityChanged -= UpdateCapacityUI;
        }
    }

    // ---- HÀM XỬ LÝ TRẠNG THÁI NÚT ----
    private void UpdateButtonState(bool hasUnits)
    {
        if (_btnStart != null)
        {
            // 1. Bật/Tắt khả năng click của nút
            _btnStart.SetEnabled(hasUnits);

            // 2. Thay đổi màu sắc thông qua USS Class
            if (hasUnits)
            {
                // Nếu có quân -> Xóa class màu đỏ đi (nút trở lại màu bình thường)
                _btnStart.RemoveFromClassList("button-disabled-red");
            }
            else
            {
                // Nếu KHÔNG có quân -> Thêm class màu đỏ vào để cảnh báo
                _btnStart.AddToClassList("button-disabled-red");
            }
        }
    }

    // ---- HÀM XỬ LÝ HIỂN THỊ SỐ LIỆU QUÂN SỐ ----
    private void UpdateCapacityUI(int current, int max)
    {
        if (_lblCapacity != null)
        {
            _lblCapacity.text = $"QUÂN SỐ: {current}/{max}";

            // Tính năng phụ (UX): Nếu đầy quân (max) thì đổi chữ sang màu Vàng cảnh báo cho đẹp
            if (current >= max && max > 0)
            {
                _lblCapacity.style.color = new StyleColor(Color.yellow);
            }
            else
            {
                // Trở lại màu xanh cyan gốc (#64c8ff) đã thiết kế ở USS
                _lblCapacity.style.color = new StyleColor(new Color(0.39f, 0.78f, 1f));
            }
        }
    }

    private void OnStartClicked()
    {
        Debug.Log("Nút Start đã được bấm!");
        GameEvents.CallStateChange(GameState.Playing);
    }
}