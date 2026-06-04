using UnityEngine;
using UnityEngine.Pool; // Sử dụng thư viện Pool của Unity

public class RangedStrategy : IAttackStrategy
{
    private Projectile _arrowPrefab;
    private Transform _firePoint;

    // Hệ thống Bể chứa thông minh của Unity
    private IObjectPool<Projectile> _projectilePool;
    private Transform _poolContainer;

    public RangedStrategy(Projectile arrowPrefab, Transform firePoint)
    {
        _arrowPrefab = arrowPrefab;
        _firePoint = firePoint;
        // THÊM DÒNG NÀY: Tự động tạo 1 GameObject rỗng trong Hierarchy tên là "Arrow_Pool"
        _poolContainer = new GameObject("Arrow_Pool").transform;

        // Khởi tạo Object Pool với các quy tắc hoạt động
        _projectilePool = new ObjectPool<Projectile>(
            createFunc: CreateNewProjectile,       // Hàm tạo mới khi thiếu đạn
            actionOnGet: OnGetProjectile,         // Hàm xử lý khi lấy đạn ra
            actionOnRelease: OnReleaseProjectile, // Hàm xử lý khi cất đạn đi
            actionOnDestroy: OnDestroyProjectile, // Hàm xóa hẳn nếu vượt quá giới hạn bộ nhớ
            collectionCheck: true,                // Kiểm tra lỗi bảo mật (tránh release 2 lần)
            defaultCapacity: 10,                  // Số lượng đạn ước tính ban đầu
            maxSize: 30                           // Số lượng đạn tối đa trong bể chứa
        );
    }

    // --- CÁC HÀM VẬN HÀNH NỘI BỘ CỦA POOL ---
    private Projectile CreateNewProjectile()
    {
        // Khi trong bể hết đạn, Pool sẽ tự gọi hàm này để sinh thêm
        return Object.Instantiate(_arrowPrefab, _poolContainer);
    }

    private void OnGetProjectile(Projectile projectile)
    {
        // Khi lấy đạn ra: Đặt lại vị trí bắn và bật nó lên
        projectile.transform.position = _firePoint.position;
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        // Khi cất đạn đi: Tắt GameObject để ẩn khỏi màn hình
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        // Hàm dọn dẹp nếu dôi dư
        Object.Destroy(projectile.gameObject);
    }

    // --- HÀM THỰC THI TẤN CÔNG CHÍNH ---
    public void ExecuteAttack(BaseUnit owner, BaseUnit target, float damage)
    {
        if (_projectilePool == null) return;

        // Lấy một mũi tên rảnh rỗi từ trong Pool ra thay vì Instantiate mới
        Projectile arrow = _projectilePool.Get();
        BattleEvents.RaisePlaySound3D(SoundType.Battle_ArrowShoot, owner.transform.position);

        if (arrow != null)
        {
            // Bơm dữ liệu VÀ truyền kèm chính cái Pool quản lý nó vào
            arrow.Setup(target, damage, owner, _projectilePool);
        }
    }
}