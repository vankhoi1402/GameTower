public class UnitBrain
{
    private BaseUnit _unit;

    public UnitBrain(BaseUnit unit)
    {
        _unit = unit;
    }

    // Hàm này sẽ được gọi liên tục trong Update() của BaseUnit
    public void Think()
    {
        // 1. Nếu đang chết thì không nghĩ ngợi gì nữa
        if (_unit.Health.IsDead) return;

        // 2. Lấy thông tin từ hệ thống giác quan (Targeting)
        bool hasTarget = _unit.Targeting.HasTarget;
        bool inRange = _unit.Targeting.IsTargetInAttackRange();

        // 3. QUYẾT ĐỊNH CHUYỂN STATE (Cây logic)
        if (!hasTarget)
        {
            // Nếu không có địch -> Nghỉ ngơi
            if (!(_unit.StateMachine.CurrentState is IdleState))
            {
                _unit.StateMachine.ChangeState(new IdleState(_unit));
            }
        }
        else if (hasTarget && !inRange)
        {
            // Có địch nhưng ở xa -> Đuổi theo
            if (!(_unit.StateMachine.CurrentState is MoveState))
            {
                _unit.StateMachine.ChangeState(new MoveState(_unit));
            }
        }
        else if (hasTarget && inRange)
        {
            // Có địch và ở gần -> Đánh
            if (!(_unit.StateMachine.CurrentState is AttackState))
            {
                _unit.StateMachine.ChangeState(new AttackState(_unit));
            }
        }
    }
}