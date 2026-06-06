using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SettingsUIController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _screenOverlay;
    private Slider _sliderMaster;
    private Slider _sliderBGM;
    private Slider _sliderUISFX;
    private Slider _sliderBattleSFX;
    private Button _btnClose;
    private bool _isOpened = false;
    private bool _initialized;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();

        
    }

    private void Start()
    {
        // Để đảm bảo ngay khi Instantiate, mọi biến đã sẵn sàng
        //var root = _uiDocument.rootVisualElement;
        //if (root != null)
        //{
        //    _screenOverlay = root.Q<VisualElement>("screen-overlay");
        //    _sliderMaster = root.Q<Slider>("slider-master");
        //    _sliderBGM = root.Q<Slider>("slider-bgm");
        //    _sliderUISFX = root.Q<Slider>("slider-ui-sfx");
        //    _sliderBattleSFX = root.Q<Slider>("slider-battle-sfx");
        //    _btnClose = root.Q<Button>("btn-close");

        //    ConfigureSlider(_sliderMaster, AudioChannel.Master, OnMasterVolumeChanged);
        //    ConfigureSlider(_sliderBGM, AudioChannel.BGM, OnBGMVolumeChanged);
        //    ConfigureSlider(_sliderUISFX, AudioChannel.UI_SFX, OnUISFXVolumeChanged);
        //    ConfigureSlider(_sliderBattleSFX, AudioChannel.Battle_SFX, OnBattleSFXVolumeChanged);

        //    if (_btnClose != null) _btnClose.clicked += OnCloseButtonClicked;
        //}
    }
    private void Initialize()
    {
        if (_initialized)
            return;

        var root = _uiDocument.rootVisualElement;

        if (root == null)
        {
            Debug.LogError("RootVisualElement is null!");
            return;
        }

        _screenOverlay = root.Q<VisualElement>("screen-overlay");
        _sliderMaster = root.Q<Slider>("slider-master");
        _sliderBGM = root.Q<Slider>("slider-bgm");
        _sliderUISFX = root.Q<Slider>("slider-ui-sfx");
        _sliderBattleSFX = root.Q<Slider>("slider-battle-sfx");
        _btnClose = root.Q<Button>("btn-close");

        ConfigureSlider(_sliderMaster, AudioChannel.Master, OnMasterVolumeChanged);
        ConfigureSlider(_sliderBGM, AudioChannel.BGM, OnBGMVolumeChanged);
        ConfigureSlider(_sliderUISFX, AudioChannel.UI_SFX, OnUISFXVolumeChanged);
        ConfigureSlider(_sliderBattleSFX, AudioChannel.Battle_SFX, OnBattleSFXVolumeChanged);

        if (_btnClose != null)
            _btnClose.clicked += OnCloseButtonClicked;

        _initialized = true;

        Debug.Log("Settings UI Initialized");
    }

    private void OnDestroy()
    {
        if (_sliderMaster != null) _sliderMaster.UnregisterValueChangedCallback(OnMasterVolumeChanged);
        if (_sliderBGM != null) _sliderBGM.UnregisterValueChangedCallback(OnBGMVolumeChanged);
        if (_sliderUISFX != null) _sliderUISFX.UnregisterValueChangedCallback(OnUISFXVolumeChanged);
        if (_sliderBattleSFX != null) _sliderBattleSFX.UnregisterValueChangedCallback(OnBattleSFXVolumeChanged);
        if (_btnClose != null) _btnClose.clicked -= OnCloseButtonClicked;
    }

    public void SetVisibility(bool visible)
    {
        Initialize();
        if (_screenOverlay == null) return;
        _isOpened = visible;
        _screenOverlay.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

        if (visible) UpdateSliderValues();
    }

    private void ConfigureSlider(Slider slider, AudioChannel channel, EventCallback<ChangeEvent<float>> callback)
    {
        if (slider == null) return;
        slider.lowValue = 0.0001f;
        slider.highValue = 1f;
        slider.RegisterValueChangedCallback(callback);
    }

    private void UpdateSliderValues()
    {
        Debug.Log(UISoundManager.Instance);
        if (UISoundManager.Instance == null) return;
        if (_sliderMaster != null) _sliderMaster.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.Master);
        if (_sliderBGM != null) _sliderBGM.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.BGM);
        if (_sliderUISFX != null) _sliderUISFX.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.UI_SFX);
        if (_sliderBattleSFX != null) _sliderBattleSFX.value = UISoundManager.Instance.GetSavedVolume(AudioChannel.Battle_SFX);
    }

    private void OnMasterVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.Master, evt.newValue);
    private void OnBGMVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.BGM, evt.newValue);
    private void OnUISFXVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.UI_SFX, evt.newValue);
    private void OnBattleSFXVolumeChanged(ChangeEvent<float> evt) => UISoundManager.Instance?.SetVolume(AudioChannel.Battle_SFX, evt.newValue);

    private void OnCloseButtonClicked()
    {
        UIEvents.RaisePlaySound(SoundType.UI_CloseWindow);
        if (SettingsManager.Instance != null) SettingsManager.Instance.Close();
        else SetVisibility(false);
    }
}