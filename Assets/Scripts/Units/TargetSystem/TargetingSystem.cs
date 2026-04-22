using UnityEngine;
using System;

public class TargetingSystem
{
    private BaseUnit _owner;
    private ITargetSearchStrategy _strategy;

    // Thuộc tính để lấy mục tiêu hiện tại
    public BaseUnit CurrentTarget { get; private set; }
    public bool HasTarget => CurrentTarget != null;

    // Observer: Các hệ thống khác (như Movement) sẽ lắng nghe sự kiện này
    public Action<BaseUnit> OnTargetChanged;
    public Action OnTargetLost;

    public TargetingSystem(BaseUnit owner, ITargetSearchStrategy defaultStrategy)
    {
        _owner = owner;
        _strategy = defaultStrategy;
    }

    // [STRATEGY PATTERN] Cho phép thay đổi chiến thuật lúc đang chạy game
    public void SetStrategy(ITargetSearchStrategy newStrategy)
    {
        _strategy = newStrategy;
        CurrentTarget = null; // Reset để tìm lại theo tiêu chí mới
    }

    // Hàm này được gọi trong Update() của BaseUnit
    public void UpdateTick()
    {
        //Debug.Log($"[TargetSystem] {_owner.name} đang tick...");
        // 1. Kiểm tra nếu mục tiêu cũ đã chết hoặc đi quá xa tầm nhìn
        if (CurrentTarget != null)
        {
            float dist = Vector2.Distance(_owner.transform.position, CurrentTarget.transform.position);
          //-  Debug.Log($"Đang có mục tiêu: {CurrentTarget.name}. Khoảng cách: {dist}. Chết chưa: {CurrentTarget.Health.IsDead}");
            if (CurrentTarget.Health.IsDead || dist > _owner.data.detectRange)
            {
                CurrentTarget = null;
                OnTargetLost?.Invoke();
            }
        }

        // 2. Nếu chưa có mục tiêu, thực hiện tìm kiếm bằng Strategy hiện tại
        if (CurrentTarget == null)
        {
           // Debug.Log($"<color=orange>[TargetSystem]</color> {_owner.name} đang gọi Strategy để tìm mục tiêu mới...");
            BaseUnit newTarget = _strategy.SelectTarget(_owner, _owner.data.detectRange, _owner.enemyLayer);
            if (newTarget != null)
            {
                CurrentTarget = newTarget;
                OnTargetChanged?.Invoke(CurrentTarget);
            }
        }
    }

    // Tiện ích để kiểm tra khoảng cách đánh
    public bool IsTargetInAttackRange()
    {
        if (CurrentTarget == null) return false;
        float dist = Vector2.Distance(_owner.transform.position, CurrentTarget.transform.position);
        return dist <= _owner.stats.currentAttackRange;
    }
}