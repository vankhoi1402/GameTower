using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("Tên của Scene chọn Level hoặc Scene chuẩn bị trận đấu")]
    [SerializeField] private string nextSceneName = "MainMenuScene";

    private UIDocument _uiDocument;
    private Button _playButton;
    private Button _settingsButton;
    private Button _quitButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (_uiDocument == null) return;

        // Lấy root visual element của UI Toolkit
        VisualElement root = _uiDocument.rootVisualElement;

        // Tìm kiếm chính xác các nút bấm qua tên (Name)
        _playButton = root.Q<Button>("PlayButton");
        _settingsButton = root.Q<Button>("SettingsButton");
        _quitButton = root.Q<Button>("QuitButton");

        // Đăng ký lắng nghe sự kiện Click
        if (_playButton != null) _playButton.clicked += OnPlayClicked;
        if (_settingsButton != null) _settingsButton.clicked += OnSettingsClicked;
        if (_quitButton != null) _quitButton.clicked += OnQuitClicked;
    }

    private void OnDisable()
    {
        // Hủy đăng ký khi Object bị tắt để tránh rò rỉ bộ nhớ
        if (_playButton != null) _playButton.clicked -= OnPlayClicked;
        if (_settingsButton != null) _settingsButton.clicked -= OnSettingsClicked;
        if (_quitButton != null) _quitButton.clicked -= OnQuitClicked;
    }

    private void OnPlayClicked()
    {
        Debug.Log("<color=green>[MainMenu]</color> Bấm PLAY -> Đang chuyển hướng sang Scene tiếp theo...");
        UIEvents.RaisePlaySound(SoundType.UI_Click);

        // Load ván chơi hoặc scene chọn màn
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnSettingsClicked()
    {
        Debug.Log("<color=yellow>[MainMenu]</color> Bấm SETTINGS -> Hiện tại chưa làm panel cài đặt.");
        // Bạn có thể làm một bảng UI ẩn khác rồi dùng style.display = DisplayStyle.Flex để bật lên ở đây
        UIEvents.RaisePlaySound(SoundType.UI_Click);
        SettingsManager.Instance.Open();
    }

    private void OnQuitClicked()
    {
        Debug.Log("<color=red>[MainMenu]</color> Bấm QUIT -> Đang đóng ứng dụng game...");
        UIEvents.RaisePlaySound(SoundType.UI_Click);

        Application.Quit(); // Lệnh thoát game khi Build ra máy hành chỉnh

        // Lệnh thoát chế độ Play Mode khi đang test trong Editor Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}