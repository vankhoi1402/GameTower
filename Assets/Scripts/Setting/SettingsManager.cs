using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [SerializeField] private SettingsUIController settingsUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Open() => settingsUI?.SetVisibility(true);

    public void Close() => settingsUI?.SetVisibility(false);

    public void Toggle() => settingsUI?.ToggleVisibility();
}