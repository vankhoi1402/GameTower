using UnityEngine;
using UnityEngine.UIElements;

public class EndGamePanel : UIPanel
{
    private Label _resultLabel;
    private Button _continueButton;
    private Button _returnButton;

    public EndGamePanel(VisualElement root, UIManager uiManager, string panelName)
        : base(root, uiManager, panelName)
    {
        // Kiểm tra chính xác tên ID đặt trong UXML
        _resultLabel = RootElement.Q<Label>("ResultLabel");
        _continueButton = RootElement.Q<Button>("ContinueButton");
        _returnButton = RootElement.Q<Button>("ReturnButton");

        if (_returnButton != null) _returnButton.clicked += OnReturnButtonClicked;
        if (_continueButton != null) _continueButton.clicked += OnContinueButtonClicked;
    }

    // Tự động chạy khi UIManager gọi Show()
    public override void Show()
    {
        base.Show(); // Gỡ class 'hidden' để hiện Panel

        // Lấy dữ liệu kết quả từ BattleManager để áp dụng giao diện tương ứng
        if (BattleManager.Instance != null)
        {
            ApplyMatchResult(BattleManager.Instance.LastMatchResult);
        }
    }

    private void ApplyMatchResult(MatchResult result)
    {
        if (_resultLabel == null) return;

        if (result == MatchResult.Victory)
        {
            _resultLabel.text = "VICTORY";
            _resultLabel.style.color = new StyleColor(new Color(1f, 0.87f, 0f)); // Màu Vàng

            // Hiện nút Continue để đi tiếp sang màn sau
            if (_continueButton != null) _continueButton.style.display = DisplayStyle.Flex;
        }
        else if (result == MatchResult.Defeat)
        {
            _resultLabel.text = "DEFEAT";
            _resultLabel.style.color = new StyleColor(Color.red); // Màu Đỏ

            // Thua trận thì ẩn nút Continue đi (Bắt buộc quay về Menu hoặc bạn đổi logic thành nút Thử Lại - Retry)
            if (_continueButton != null) _continueButton.style.display = DisplayStyle.None;
        }
    }

    private void OnContinueButtonClicked()
    {
        Debug.Log("[EndGamePanel] Bấm Continue -> Logic tải màn tiếp theo xử lý tại đây.");
        // TODO: Viết code gọi sang LevelManager hoặc GameMenuManager để Load ván mới
    }

    private void OnReturnButtonClicked()
    {
        if (GameMenuManager.Instance != null)
        {
            GameMenuManager.Instance.BackToMenu("MainMenuScene");
            BGMManager.Instance.PlayBGM(SoundType.SO_BGM_Menu);
        }
    }
}