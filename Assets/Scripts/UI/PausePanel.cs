using UnityEngine.UIElements;

public class PausePanel : UIPanel
{
    private Button _btnResume;

    public PausePanel(VisualElement root, UIManager manager, string elementName)
        : base(root, manager, elementName)
    {
        _btnResume = RootElement.Q<Button>("BtnResume");

        if (_btnResume != null)
        {
            _btnResume.clicked += OnResumeClicked;
        }
    }

    private void OnResumeClicked()
    {
        // UI ra lệnh: Tiếp tục chơi game!
        GameEvents.CallStateChange(GameState.Playing);
    }
}