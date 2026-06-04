using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SettingsUIController : MonoBehaviour
{
    private UIDocument _uiDocument;

    // Phần tử bọc ngoài cùng để ẩn/hiện toàn bộ bảng và nền mờ
    private VisualElement _screenOverlay;

    // Các phần tử tương tác chính xác theo tên (name) trong UXML
    private Slider _sliderMaster;
    private Slider _sliderBGM;
    private Slider _sliderUISFX;
    private Slider _sliderBattleSFX;
    private Button _btnClose;

    private bool _isOpened = false;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void Start()
    {
        var root = _uiDocument.rootVisualElement;
        if (root == null) return;

        // 1. Tìm chính xác các Element dựa trên "name" trong UXML của bạn
        _screenOverlay = root.Q<VisualElement>("screen-overlay");
        _sliderMaster = root.Q<Slider>("slider-master");
        _sliderBGM = root.Q<Slider>("slider-bgm");
        _sliderUISFX = root.Q<Slider>("slider-ui-sfx");
        _sliderBattleSFX = root.Q<Slider>("slider-battle-sfx");
        _btnClose = root.Q<Button>("btn-close");

        // 2. Cấu hình giới hạn và Đăng ký sự kiện (Chỉ chạy 1 lần duy nhất lúc khởi động)
        ConfigureSlider(_sliderMaster, AudioChannel.Master, OnMasterVolumeChanged);
        ConfigureSlider(_sliderBGM, AudioChannel.BGM, OnBGMVolumeChanged);
        ConfigureSlider(_sliderUISFX, AudioChannel.UI_SFX, OnUISFXVolumeChanged);
        ConfigureSlider(_sliderBattleSFX, AudioChannel.Battle_SFX, OnBattleSFXVolumeChanged);

        if (_btnClose != null)
        {
            _btnClose.clicked += OnCloseButtonClicked;
        }

        // 3. Trạng thái mặc định khi vừa vào game: Ẩn bảng cài đặt đi
        SetVisibility(false);
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện để giải phóng bộ nhớ khi Object bị xóa hẳn
        _sliderMaster?.UnregisterValueChangedCallback(OnMasterVolumeChanged);
        _sliderBGM?.UnregisterValueChangedCallback(OnBGMVolumeChanged);
        _sliderUISFX?.UnregisterValueChangedCallback(OnUISFXVolumeChanged);
        _sliderBattleSFX?.UnregisterValueChangedCallback(OnBattleSFXVolumeChanged);

        if (_btnClose != null)
        {
            _btnClose.clicked -= OnCloseButtonClicked;
        }
    }

    /// <summary>
    /// Hàm điều khiển Ẩn/Hiện bảng Settings bằng UI Toolkit Style
    /// </summary>
    public void SetVisibility(bool visible)
    {
        if (_screenOverlay == null) return;

        _isOpened = visible;

        // Sử dụng DisplayStyle thay vì SetActive GameObject
        _screenOverlay.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

        // Khi bảng được mở ra, lập tức đồng bộ lại giá trị các thanh Slider từ Manager
        if (visible)
        {
            UpdateSliderValues();
        }
    }

    public void ToggleVisibility() => SetVisibility(!_isOpened);

    // Hàm tiện ích cấu hình nhanh các thông số cho Slider
    private void ConfigureSlider(Slider slider, AudioChannel channel, EventCallback<ChangeEvent<float>> callback)
    {
        if (slider == null) return;

        slider.lowValue = 0.0001f;
        slider.highValue = 1f;
        slider.RegisterValueChangedCallback(callback);
    }

    // Đọc dữ liệu âm lượng mới nhất để hiển thị lên Slider
    private void UpdateSliderValues()
    {
        if (UISoundManager.Instance == null) return;

        if (_sliderMaster != null) _sliderMaster.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.Master);
        if (_sliderBGM != null) _sliderBGM.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.BGM);
        if (_sliderUISFX != null) _sliderUISFX.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.UI_SFX);
        if (_sliderBattleSFX != null) _sliderBattleSFX.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.Battle_SFX);
    }

    // --- LOGIC XỬ LÝ SỰ KIỆN KHI KÉO SLIDER ---
    private void OnMasterVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.Master, evt.newValue);
    private void OnBGMVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.BGM, evt.newValue);
    private void OnUISFXVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.UI_SFX, evt.newValue);
    private void OnBattleSFXVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.Battle_SFX, evt.newValue);

    // --- LOGIC XỬ LÝ NÚT CLOSE ---
    private void OnCloseButtonClicked()
    {
        // Phát sound hiệu ứng đóng cửa sổ
        UIEvents.RaisePlaySound(SoundType.UI_CloseWindow);

        // Gọi lệnh đóng thông qua SettingsManager trung gian
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.Close();
        }
        else
        {
            // Phương án dự phòng tự đóng nếu chạy test độc lập không có Manager trong Scene
            SetVisibility(false);
        }
    }
}