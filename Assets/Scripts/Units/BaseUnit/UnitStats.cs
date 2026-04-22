using UnityEngine;

[System.Serializable] // Để có thể hiển thị trong Inspector nếu cần
public class UnitStats
{
    public float currentHP;
    public float currentDamage;
    public float currentMoveSpeed;
    public float currentAttackSpeed;
    public float currentAttackRange;

    private float _nextAttackTime;

    // Constructor: Copy dữ liệu từ UnitData sang UnitStats
    public UnitStats(UnitData data)
    {
        currentHP = data.maxHP;
        currentDamage = data.damage;
        currentMoveSpeed = data.moveSpeed;
        currentAttackSpeed = data.attackSpeed;
        currentAttackRange = data.attackRange;
    }
    public void UpdateAttackTimer()
    {
        _nextAttackTime = Time.time + (1f / currentAttackSpeed);
    }

  
    // Hàm để nhận sát thương
    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
    }

    // Hàm để tướng Buff chỉ số (Ví dụ: tăng 20% dame)
    public void ApplyDamageBuff(float multiplier)
    {
        currentDamage *= multiplier;
    }
    public bool IsAttackReady()
    {
        return Time.time >= _nextAttackTime;
    }
}