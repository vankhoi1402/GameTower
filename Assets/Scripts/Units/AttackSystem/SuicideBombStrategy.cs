using UnityEngine;

public class SuicideBombStrategy : IAttackStrategy
{
    private float _explosionRadius;
    private GameObject _explosionAnimPrefab;

    // Constructor nhận dữ liệu từ Unit truyền vào
    public SuicideBombStrategy(float explosionRadius, GameObject explosionAnimPrefab)
    {
        _explosionRadius = explosionRadius;
        _explosionAnimPrefab = explosionAnimPrefab;
    }

    public void ExecuteAttack(BaseUnit owner, BaseUnit target, float damage)
    {
        Debug.Log($"[BOM] ExecuteAttack được gọi! Sát thương gốc: {damage}");
        // 1. Sinh ra hiệu ứng nổ tại vị trí lính
        if (_explosionAnimPrefab != null)
        {
            GameObject vfx = Object.Instantiate(_explosionAnimPrefab, owner.transform.position, Quaternion.identity);
            Object.Destroy(vfx, 1f);
        }

        // 2. Quét mục tiêu và gây sát thương AoE
        // Lưu ý: Dùng owner.enemyLayer đã có sẵn ở BaseUnit
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(owner.transform.position, _explosionRadius, owner.enemyLayer);
        Debug.Log($"[BOM] Quét được {hitTargets.Length} vật thể trong LayerMask.");
        foreach (Collider2D hit in hitTargets)
        {
            // Bỏ qua chính bản thân nó
            if (hit.gameObject == owner.gameObject) continue;

            BaseUnit targetUnit = hit.GetComponent<BaseUnit>();
            Debug.Log($"[BOM] Check mục tiêu {targetUnit.name}: Phe địch = {targetUnit.Team}, Phe bom = {owner.Team}");

            if (targetUnit != null && targetUnit.Team != owner.Team && !targetUnit.Health.IsDead)
            {
                // Dùng chính tham số 'damage' được truyền vào từ hàm
                targetUnit.TakeDamage(damage);
                Debug.Log($"[CHECK BOM] THÀNH CÔNG: Đã gây {damage} dame lên {targetUnit.name}!");
            }
        }

        // 3. Sau khi gây sát thương xong, lính cảm tử tự sát
        owner.TakeDamage(owner.stats.currentHP);
    }
}