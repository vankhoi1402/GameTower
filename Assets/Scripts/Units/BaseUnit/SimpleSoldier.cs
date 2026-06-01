using UnityEngine;

public class SimpleSoldier : BaseUnit
{
    protected override void Awake()
    {
        // Gọi Awake của BaseUnit để khởi tạo Stats và StateMachine
        base.Awake();

        // 1. Lắp bộ não tìm mục tiêu: Tìm kẻ địch gần nhất
        // (Giả sử bạn đã có ClosestTargetStrategy)
        Targeting = new TargetingSystem(this, new ClosestTargetStrategy());

        // 2. Lắp vũ khí: Đánh cận chiến đơn mục tiêu
        Attack = new AttackSystem(this, new MeleeStrategy(),Anim);

        // 3. Đăng ký Observer: Khi đánh thì chạy Animation và Log
        Attack.OnAttackPerformed += HandleAttackVisuals;
    }

    protected override void Start()
    {   
        base.Start();
        // Bắt đầu ở trạng thái đứng đợi
        StateMachine.ChangeState(new IdleState(this));
    }
   
    private void HandleAttackVisuals(BaseUnit target, float damage)
    {
        // Test nhanh bằng Debug và Animation
       // Debug.Log($"<color=cyan>[TEST]</color> {data.unitName} đã vung kiếm chém {target.data.unitName}!");

        // Nếu có Animator thì trigger
        // GetComponent<Animator>().SetTrigger("Attack");
    }

    // Hàm này để bạn Test sát thương nhanh trong Console
    [ContextMenu("Test Take Damage")]
    public void TestDamage()
    {
        Health.TakeDamage(10);
        Debug.Log($"Máu hiện tại của {data.unitName}: {stats.currentHP}");
    }
}