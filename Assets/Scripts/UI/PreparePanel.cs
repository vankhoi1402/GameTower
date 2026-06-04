using UnityEngine;
using UnityEngine.UIElements;

public class PreparePanel : UIPanel
{
    private Button _btnStart;

    public PreparePanel(VisualElement root, UIManager manager, string elementName)
        : base(root, manager, elementName)
    {
        _btnStart = RootElement.Q<Button>("ready-button");

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

    private void OnStartClicked()
    {
        Debug.Log("Nút Start đã được bấm!");
        GameEvents.CallStateChange(GameState.Playing);
    }
}