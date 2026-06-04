using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Gán file Prefab Settings UI vào đây")]
    [SerializeField] private GameObject settingsPrefab;

    private SettingsUIController _currentSettingsUI;

    private void Awake()
    {
        // Singleton chuẩn, không Destroy cái Manager này, nhưng UI thì tạo mới theo Scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Open()
    {
        // Nếu ở Scene mới chưa có bảng UI Settings, tự động lôi trong Prefab ra tạo mới
        if (_currentSettingsUI == null)
        {
            // Thử tìm xem trong Scene hiện tại có sẵn cái nào chưa
           // _currentSettingsUI = FindFirstObjectByType<SettingsUIController>();

            // Nếu trong Scene chưa có ai đặt sẵn -> Sinh ra từ Prefab
            if (_currentSettingsUI == null && settingsPrefab != null)
            {
                GameObject spawnedUI = Instantiate(settingsPrefab);
                _currentSettingsUI = spawnedUI.GetComponent<SettingsUIController>();
            }
        }

        // Khi đã chắc chắn có UI trong Scene hiện tại -> Ra lệnh mở
        if (_currentSettingsUI != null)
        {
            _currentSettingsUI.SetVisibility(true);
        }
        else
        {
            Debug.LogError("[SettingsManager] Không tìm thấy hoặc chưa gán Settings Prefab!");
        }
    }

    public void Close()
    {
        if (_currentSettingsUI != null)
        {
            _currentSettingsUI.SetVisibility(false);
        }
    }
}