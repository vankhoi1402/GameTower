using UnityEngine;

public class SuicideBomberUnit : BaseUnit
{
    [Header("Kamikaze Settings")]
    public float explosionRadius = 3f;
    public GameObject explosionAnimPrefab;

    protected override void Awake()
    {
        base.Awake();

        // Truyền bán kính và Prefab hiệu ứng nổ vào Strategy
        Attack = new AttackSystem(this, new SuicideBombStrategy(explosionRadius, explosionAnimPrefab), Anim);
    }

    // Bạn có thể xóa luôn HandleDeath() đi vì không cần tùy chỉnh gì khi chết nữa

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}