using UnityEngine;

public class ArcherUnit : BaseUnit
{
    [SerializeField] private Projectile arrowPrefab; // Kéo Prefab mũi tên vào đây
    [SerializeField] private Transform bowFirePoint; // Kéo vị trí đầu cây cung vào đây

    protected override void Awake()
    {
        base.Awake();
        //IUnitAnimator anim = GetComponent<IUnitAnimator>();
        Targeting = new TargetingSystem(this, new ClosestTargetStrategy());

        // Khởi tạo chiến thuật bắn xa có tích hợp sẵn Object Pool bên trong
        IAttackStrategy rangedStrategy = new RangedStrategy(arrowPrefab, bowFirePoint);

        // Nạp chiến thuật vào hệ thống Combat của bạn
        Attack = new AttackSystem(this, rangedStrategy, Anim);
    }
    protected override void Start()
    {
        base.Start();
        // Bắt đầu ở trạng thái đứng đợi
        StateMachine.ChangeState(new IdleState(this));
    }
}