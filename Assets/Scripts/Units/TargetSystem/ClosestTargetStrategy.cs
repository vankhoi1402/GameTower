using UnityEngine;

public class ClosestTargetStrategy : ITargetSearchStrategy
{
    public BaseUnit SelectTarget(BaseUnit owner, float detectRange, LayerMask enemyLayer)
    {
        // Quét tất cả vật thể trong tầm nhìn
        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, detectRange, enemyLayer);
       // Debug.Log($"[Strategy] Tìm thấy {hits.Length} Collider xung quanh.");
        BaseUnit bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            BaseUnit potentialTarget = hit.GetComponent<BaseUnit>();
            if (potentialTarget == null || potentialTarget.Health.IsDead) continue;

            float dist = Vector2.Distance(owner.transform.position, potentialTarget.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                bestTarget = potentialTarget;
            }
            
        }
        //if (bestTarget != null)
        //{
        //    Debug.Log($"<color=green>[Targeting]</color> {owner.name} đã chọn mục tiêu: {bestTarget.name} (Cách {closestDistance:F2}m)");
        //}

        return bestTarget;
    }
}