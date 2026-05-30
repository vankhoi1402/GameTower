using System;
using UnityEngine;

public class AttackSystem
{
    private BaseUnit _owner;
    private IAttackStrategy _strategy;
    private IUnitAnimator _anim; // 1. Kết hợp hộp đen Animation vào đây

    // Biến tạm để ghi nhớ mục tiêu trong lúc chờ Animation vung kiếm đến frame gây sát thương
    private BaseUnit _currentTarget;

    // --- [OBSERVER] ---
    public event Action<BaseUnit, float> OnAttackPerformed;

    public AttackSystem(BaseUnit owner, IAttackStrategy initialStrategy, IUnitAnimator anim)
    {
        _owner = owner;
        _strategy = initialStrategy;
        _anim = anim;

        // 2. Đăng ký nghe sự kiện "khung hình hành động" từ hộp đen Animation
        if (_anim != null)
        {
            _anim.OnActionTriggered += HandleDamageFrame;
        }
    }

    // Nhớ gọi hàm này khi Unit bị hủy để tránh rò rỉ bộ nhớ
    public void Cleanup()
    {
        if (_anim != null)
        {
            _anim.OnActionTriggered -= HandleDamageFrame;
        }
    }

    public void SetStrategy(IAttackStrategy newStrategy) => _strategy = newStrategy;

    public void Update()
    {
        _owner.stats.UpdateAttackTimer();
    }

    public void TryExecuteAttack(BaseUnit target)
    {
        if (target == null || target.Health.IsDead) return;

        // Kiểm tra xem đã đến lúc được đánh chưa
        if (_owner.stats.IsAttackReady())
        {
            // Bước A: Ghi nhớ mục tiêu hiện tại
            _currentTarget = target;

            // Bước B: PHÁT ANIMATION TẠI ĐÂY (Bắn và quên)
            // Khóa hoạt họa lại để không bị các trạng thái như chạy bộ đè lên giữa chừng
            _anim.Play(UnitAnimState.Attack, lockAnim: true, forceReplay: true);
           // _strategy.ExecuteAttack(_owner, _currentTarget, _owner.stats.currentDamage);

            // Bước C: Reset thời gian chờ lập tức
            _owner.stats.UpdateAttackTimer();
        }
    }

    // --- NƠI THỰC SỰ GÂY SÁT THƯƠNG ---
    // Hàm này sẽ tự động kích hoạt khi Clip Animation chạy đến đúng Frame chém trúng/buông cung
    private void HandleDamageFrame()
    {
        // Kiểm tra lại xem trong lúc vung kiếm thì mục tiêu có bị đồng đội khác đánh chết trước chưa
        if (_currentTarget != null && !_currentTarget.Health.IsDead)
        {
            // 1. Thực thi logic gây dame (Strategy) ĐÚNG NHỊP HÌNH ẢNH
            _strategy.ExecuteAttack(_owner, _currentTarget, _owner.stats.currentDamage);

            // 2. Phát tín hiệu cho VFX/SFX/UI nổ hiệu ứng đúng vị trí
            OnAttackPerformed?.Invoke(_currentTarget, _owner.stats.currentDamage);
        }
    }
}