using UnityEngine;

public class AttackState : IState
{
    private BaseUnit _unit;

    public AttackState(BaseUnit unit)
    {
        _unit = unit;
    }

    public void OnEnter()
    {
        Debug.Log($"<color=red>[Attack]</color> {_unit.data.unitName} vào thế chiến đấu!");
    }

    public void OnUpdate()
    {
        // 1. Kiểm tra mục tiêu còn tồn tại không
        if (!_unit.Targeting.HasTarget) return;

        BaseUnit target = _unit.Targeting.CurrentTarget;

        // 2. Quay mặt về phía mục tiêu (Xử lý Flip Sprite)
       // LookAtTarget(target);

        // 3. Thực hiện tấn công thông qua hệ thống AttackSystem
        // Hệ thống này sẽ tự quản lý Cooldown (tốc độ đánh) cho bạn
        _unit.Attack.TryExecuteAttack(target);
    }

    public void OnExit()
    {
        Debug.Log($"<color=red>[Attack]</color> {_unit.data.unitName} ngừng tấn công.");
    }

    private void LookAtTarget(BaseUnit target)
    {
        if (target == null) return;

        // Logic quay mặt cơ bản dựa trên vị trí X
        float direction = target.transform.position.x - _unit.transform.position.x;
        if (direction > 0) _unit.transform.localScale = new Vector3(1, 1, 1);
        else if (direction < 0) _unit.transform.localScale = new Vector3(-1, 1, 1);
    }
}