using System;
using UnityEngine;

public class AttackSystem 
{
    private BaseUnit _owner;
    private IAttackStrategy _strategy;

    // --- [OBSERVER] ---
    // Phát tín hiệu khi đòn đánh thực sự xảy ra để VFX/SFX/UI lắng nghe
    public event Action<BaseUnit, float> OnAttackPerformed;

    public AttackSystem(BaseUnit owner, IAttackStrategy initialStrategy)
    {
        _owner = owner;
        _strategy = initialStrategy;
    }

    // Cho phép đổi chiến thuật lúc đang chơi (Ví dụ nhặt được buff)
    public void SetStrategy(IAttackStrategy newStrategy) => _strategy = newStrategy;

    public void Update()
    {
        // Cập nhật cooldown từ Stats (đã viết ở các bước trước)
        _owner.stats.UpdateAttackTimer();
    }

    public void TryExecuteAttack(BaseUnit target)
    {

        if (target == null || target.Health.IsDead) {
            
        }
        Debug.Log(_owner.stats.IsAttackReady());
        // Kiểm tra xem đã đến lúc được đánh chưa
        if (_owner.stats.IsAttackReady())
        {
            // 1. Thực thi logic gây dame (Strategy)
             _strategy.ExecuteAttack(_owner, target, _owner.stats.currentDamage);
                Debug.Log(" danh nhau ");
            // 2. Phát tín hiệu cho các bên liên quan
            OnAttackPerformed?.Invoke(target, _owner.stats.currentDamage);

            // 3. Reset thời gian chờ
            _owner.stats.UpdateAttackTimer();
            
        }
    }
}