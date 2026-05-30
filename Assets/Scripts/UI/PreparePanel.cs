using UnityEngine;
using UnityEngine.UIElements;

public class PreparePanel : UIPanel
{
    private Button _btnStart;

    public PreparePanel(VisualElement root, UIManager manager, string elementName)
        : base(root, manager, elementName)
    {
        // elementName truyền vào phải là "PreparePanelElement"
        _btnStart = RootElement.Q<Button>("ready-button");

        // THÊM DÒNG NÀY ĐỂ DEBUG:
        if (_btnStart == null)
        {
            Debug.LogError($"[PreparePanel] Lỗi: Không tìm thấy 'ready-button' bên trong {elementName}. Hãy kiểm tra lại file UXML!");
        }
    }

    // NẾU BẠN MUỐN ĐĂNG KÝ/HUỶ ĐĂNG KÝ THEO TRẠNG THÁI ẨN/HIỆN:
    public override void Show() // Giả định base class có hàm này
    {
        base.Show();
        if (_btnStart != null)
        {
            // Đăng ký lại mỗi khi panel được bật lên
            _btnStart.clicked += OnStartClicked;
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (_btnStart != null)
        {
            // Huỷ đăng ký khi ẩn đi
            _btnStart.clicked -= OnStartClicked;
        }
    }

    private void OnStartClicked()
    {
        Debug.Log("Nút Start đã được bấm!"); // Log ra để chắc chắn hàm chạy
        GameEvents.CallStateChange(GameState.Playing);
    }
}