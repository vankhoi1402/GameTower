using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSliderUI : MonoBehaviour
{
    [SerializeField] private AudioChannel channel;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.minValue = 0.0001f;
        _slider.maxValue = 1f;
    }

    private void Start()
    {
        if (UISoundManager.Instance != null)
        {
            _slider.value = UISoundManager.Instance.GetSavedVolume(channel);
        }
        _slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (UISoundManager.Instance != null)
        {
            UISoundManager.Instance.SetVolume(channel, value);
        }
    }

    private void OnDestroy() => _slider.onValueChanged.RemoveListener(OnSliderChanged);
}