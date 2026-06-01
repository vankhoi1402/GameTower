using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private Dictionary<UIPanelType, UIPanel> _panels;
    private UIPanel _currentPanel;

    private void OnEnable()
    {
        // Lắng nghe sự kiện thay đổi trạng thái từ State Machine của GameManager
        GameEvents.OnStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnStateChanged -= HandleGameStateChanged;
    }

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _panels = new Dictionary<UIPanelType, UIPanel>()
        {
            { UIPanelType.Prepare, new PreparePanel(root, this, "PreparePanelElement") },
            { UIPanelType.Playing, new PlayingPanel(root, this, "PlayingPanelElement") },
            { UIPanelType.Paused,  new PausePanel(root, this, "PausePanelElement") },
            { UIPanelType.EndGame, new EndGamePanel(root, this, "EndGamePanelElement") }
        };

        // QUAN TRỌNG: Ép UI hiển thị trạng thái ban đầu để gán _currentPanel 
        // và kích hoạt logic sự kiện (hàm Show() của PreparePanel sẽ được chạy).
       // Show(UIPanelType.Prepare);
    }

    private void HandleGameStateChanged(GameState newState)
    {
        Debug.Log($"[UIManager] Đã nhận được lệnh chuyển sang trạng thái: {newState}");
        // Ánh xạ trực tiếp từ GameState sang UIPanelType
        switch (newState)
        {
            case GameState.Prepare:
                Show(UIPanelType.Prepare);
                break;
            case GameState.Playing:
                Show(UIPanelType.Playing);
                break;
            case GameState.Paused:
                Show(UIPanelType.Paused);
                break;
            case GameState.EndGame: // THÊM CASE NÀY
                Show(UIPanelType.EndGame);
                break;
        }
    }

    public void Show(UIPanelType type)
    {
        if (!_panels.ContainsKey(type)) return;

        _currentPanel?.Hide();
        _currentPanel = _panels[type];
        _currentPanel.Show();
    }
}