using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BaseUnit unit; // Kéo thả script BaseUnit vào đây
    [SerializeField] private GameObject healthBarContainer; // Kéo thả GameObject chứa toàn bộ thanh máu
    [SerializeField] private Transform fillTransform; // Kéo thả phần "Fill" (phần màu đỏ/xanh)

    [Header("Settings")]
    [SerializeField] private float showDuration = 5f; // Thời gian hiển thị sau khi bị đánh (giây)

    private float _hideTimer;

    private void Start()
    {
        // Ẩn thanh máu ngay từ đầu
        healthBarContainer.SetActive(false);

        // Đăng ký sự kiện
        if (unit != null && unit.Health != null) // Đảm bảo BaseUnit đã khởi tạo HealthSystem
        {
            unit.Health.OnHealthChanged += HandleHealthChanged;
            unit.Health.OnDeath += HandleDeath;
        }
    }

    private void OnDestroy()
    {
        // Phải hủy đăng ký sự kiện khi lính bị xóa/chết để tránh memory leak
        if (unit != null && unit.Health != null)
        {
            unit.Health.OnHealthChanged -= HandleHealthChanged;
            unit.Health.OnDeath -= HandleDeath;
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        // 1. Cập nhật độ dài thanh máu
        float pct = current / max;
        Vector3 currentScale = fillTransform.localScale;
        currentScale.x = pct;
        fillTransform.localScale = currentScale;

        // 2. Bật thanh máu lên và reset lại đồng hồ đếm ngược
        healthBarContainer.SetActive(true);
        _hideTimer = showDuration;
    }

    private void HandleDeath()
    {
        // Tắt thanh máu ngay lập tức khi unit chết
        healthBarContainer.SetActive(false);
    }

    private void Update()
    {
        // Xử lý logic đếm ngược để ẩn thanh máu
        if (_hideTimer > 0)
        {
            _hideTimer -= Time.deltaTime;

            // Khi hết thời gian -> ẩn thanh máu đi
            if (_hideTimer <= 0)
            {
                healthBarContainer.SetActive(false);
            }
        }
    }
}