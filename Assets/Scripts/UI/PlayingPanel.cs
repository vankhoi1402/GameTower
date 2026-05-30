using UnityEngine.UIElements;

public class PlayingPanel : UIPanel
{
    private Button _btnPause;

    public PlayingPanel(VisualElement root, UIManager manager, string elementName)
        : base(root, manager, elementName)
    {
        _btnPause = RootElement.Q<Button>("BtnPause");

        if (_btnPause != null)
        {
            _btnPause.clicked += OnPauseClicked;
        }
    }

    private void OnPauseClicked()
    {
        // UI ra lệnh: Tạm dừng game lại!
        GameEvents.CallStateChange(GameState.Paused);
    }
}