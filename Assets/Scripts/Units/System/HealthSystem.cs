using System;
using UnityEngine;

public class HealthSystem
{
    private BaseUnit _owner;

    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    // --- [OBSERVER PATTERN] ---
    // Sự kiện khi máu thay đổi (dùng cho thanh máu UI)
    public event Action<float, float> OnHealthChanged; // (current, max)

    // Sự kiện khi nhận sát thương (dùng cho hiệu ứng rung màn hình, máu văng)
    public event Action<float> OnDamaged;

    // Sự kiện khi chết (dùng cho xử lý biến mất, cộng điểm)
    public event Action OnDeath;

    public HealthSystem(BaseUnit owner, float maxHP)
    {
        _owner = owner;
        MaxHP = maxHP;
        CurrentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0) return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);

        // Phát tín hiệu cho các hệ thống đang lắng nghe
        OnDamaged?.Invoke(amount);
        OnHealthChanged?.Invoke(CurrentHP, MaxHP);

        if (IsDead)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0) return;

        CurrentHP += amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);

        OnHealthChanged?.Invoke(CurrentHP, MaxHP);
    }
}