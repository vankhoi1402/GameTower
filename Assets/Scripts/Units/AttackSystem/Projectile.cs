using UnityEngine;
using UnityEngine.Pool; // Bắt buộc phải có namespace này

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private BaseUnit _target;
    private float _damage;
    private BaseUnit _caster;
    private bool _isInitialized = false;

    [SerializeField] private float speed = 12f;

    // Lưu trữ Pool quản lý chính mũi tên này
    private IObjectPool<Projectile> _originPool;

    // Thêm tham chiếu pool vào hàm Setup
    public void Setup(BaseUnit target, float damage, BaseUnit caster, IObjectPool<Projectile> pool)
    {
        _target = target;
        _damage = damage;
        _caster = caster;
        _originPool = pool;
        _isInitialized = true;

        gameObject.SetActive(true); // Đảm bảo mũi tên được bật lên khi lấy từ Pool out
        LookAtTarget();
    }

    private void Update()
    {
        if (!_isInitialized) return;

        // Nếu mục tiêu chết trước khi tên bay tới -> Trả mũi tên về Pool an toàn
        if (_target == null || _target.Health.IsDead)
        {
            ReturnToPool();
            return;
        }

        Vector3 direction = (_target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        LookAtTarget();
    }

    private void LookAtTarget()
    {
        if (_target == null) return;
        Vector3 dir = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseUnit hitUnit = collision.GetComponent<BaseUnit>();

        if (hitUnit != null && hitUnit == _target)
        {
            hitUnit.Health.TakeDamage(_damage);

            // Thay vì Destroy, ta trả nó về Pool
            ReturnToPool();
        }
    }

    // Hàm phụ trách việc dọn dẹp và trả về bể chứa
    private void ReturnToPool()
    {
        _isInitialized = false;
        _target = null;
        _caster = null;

        // Trả về pool để ẩn đi
        _originPool?.Release(this);
    }
}