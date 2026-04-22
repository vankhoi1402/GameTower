using UnityEditor;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    [Header("Data & Config")]
    public UnitData data;
    public LayerMask enemyLayer;

    // --- CÁC HỆ THỐNG CON (Composition) ---
    public HealthSystem Health { get; private set; }
    public MovementSystem MoveSystem { get; private set; }
    public TargetingSystem Targeting { get; protected set; }
    public AttackSystem Attack { get; protected set; }
    public StateMachine StateMachine { get; private set; }
    public UnitBrain Brain { get; private set; }

    [Header("Runtime Stats")]
    public UnitStats stats;
    protected UnitState currentState;

    protected virtual void Awake()
    {
        // 1. Khởi tạo dữ liệu
        stats = new UnitStats(data);
        currentState = UnitState.Idle;

        // 2. Khởi tạo các hệ thống con
        Health = new HealthSystem(this, stats.currentHP);
        MoveSystem = new MovementSystem(this);

        // Mặc định lính sẽ tìm mục tiêu gần nhất
        // Bạn có thể override hàm này ở lớp Tướng để đổi Strategy khác
        Targeting = new TargetingSystem(this, new ClosestTargetStrategy());

        StateMachine = new StateMachine();

        Brain = new UnitBrain(this);

        Attack = new AttackSystem(this, new MeleeStrategy());
        // 3. Đăng ký sự kiện (Observer Pattern)
        Health.OnDeath += HandleDeath;
    }

    protected virtual void Update()
    {
        if (Health.IsDead) return;
        Targeting.UpdateTick();
        if (Targeting == null)
        {
            Debug.LogError($"[LỖI] Targeting của {gameObject.name} đang bị NULL!");
            return;
        }
        Brain?.Think();

        
        StateMachine?.Update();
        
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (!Targeting.HasTarget)
        {
            currentState = UnitState.Idle;
            MoveSystem.Stop();
            return;
        }

        // Dùng toán học check tầm đánh thay vì dùng thêm vật lý
        if (Targeting.IsTargetInAttackRange())
        {
            currentState = UnitState.Attack;
            MoveSystem.Stop();
           // ExecuteAttackLogic();
        }
        else
        {
            currentState = UnitState.Move;
            MoveSystem.MoveTo(Targeting.CurrentTarget.transform.position, stats.currentMoveSpeed);
        }
    }

    // --- CÁC HÀM CỐT LÕI ---

    // Lớp con (MeleeUnit / RangedUnit) sẽ định nghĩa cách tấn công cụ thể
   // protected abstract void ExecuteAttackLogic();

    public virtual void TakeDamage(float amount)
    {
        Health.TakeDamage(amount);
    }

    protected virtual void HandleDeath()
    {
        // Logic chung khi bất kỳ đơn vị nào chết (chạy hiệu ứng, xóa Object)
        Debug.Log($"{gameObject.name} đã chết.");
        Destroy(gameObject);
    }

    // [STRATEGY PATTERN] Cho phép thay đổi "não" tìm mục tiêu bất cứ lúc nào
    public void ChangeTargetingStrategy(ITargetSearchStrategy newStrategy)
    {
        Targeting.SetStrategy(newStrategy);
    }

    // Để nhìn rõ tầm Detect và Attack trong Editor
    protected virtual void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}