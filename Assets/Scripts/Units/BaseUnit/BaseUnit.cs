using UnityEditor;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    [Header("Data & Config")]
    public UnitData data;
    public LayerMask enemyLayer;
    public TeamType Team;

    // --- CÁC HỆ THỐNG CON (Composition) ---
    public HealthSystem Health { get; private set; }
    public MovementSystem MoveSystem { get; private set; }
    public TargetingSystem Targeting { get; protected set; }
    public AttackSystem Attack { get; protected set; }
    public StateMachine StateMachine { get; private set; }
    public UnitBrain Brain { get; private set; }
    public IUnitAnimator Anim { get; private set; }

    [Header("Runtime Stats")]
    public UnitStats stats;
    protected UnitState currentState;

    protected virtual void Awake()
    {
        // 1. Khởi tạo dữ liệu
        stats = new UnitStats(data);
        currentState = UnitState.Idle;
        Anim = GetComponentInChildren<IUnitAnimator>();
        // 2. Khởi tạo các hệ thống con
        Health = new HealthSystem(this, stats.currentHP);
        MoveSystem = new MovementSystem(this);

        // Mặc định lính sẽ tìm mục tiêu gần nhất
        // Bạn có thể override hàm này ở lớp Tướng để đổi Strategy khác
        Targeting = new TargetingSystem(this, new ClosestTargetStrategy());

        StateMachine = new StateMachine();

        Brain = new UnitBrain(this);

       // Attack = new AttackSystem(this, new MeleeStrategy(),Anim);
        // 3. Đăng ký sự kiện (Observer Pattern)
        Health.OnDeath += HandleDeath;
    }

    protected virtual void Update()
    {
        if (Health.IsDead) return;
        
        
        if (GameManager.Instance != null && GameManager.Instance.IsPlaying)
        {
            Targeting.UpdateTick();
            
            Brain?.Think();
        }
        else
        {
            // Nếu đang Prepare hoặc Pause, ép lính về trạng thái Idle (Đứng im)
            currentState = UnitState.Idle;
            // (Tuỳ vào cách bạn viết hàm ChangeState, có thể bạn truyền Enum hoặc Type vào đây)
        }

        // Vẫn luôn chạy StateMachine để lính có thể thực hiện các hành động thụ động 
        // (như chạy Animation đứng thở trong lúc chờ đợi)
        StateMachine?.Update();


        
        
        HandleStateMachine();
    }
    protected virtual void Start()
    {
        // 3. Thông báo TOÀN CỤC: Tôi vừa ra sân
        GlobalEventBus.OnUnitSpawned?.Invoke(this);
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
        // 4. Thông báo TOÀN CỤC: Tôi đã chết, các Manager hãy cập nhật List
        GlobalEventBus.OnUnitDied?.Invoke(this);

        Destroy(gameObject);
    }
    protected virtual void OnDestroy()
    {
        // Hủy lắng nghe nội bộ để dọn dẹp bộ nhớ
        if (Health != null)
        {
            Health.OnDeath -= HandleDeath;
        }
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