using UnityEngine;
using System.Collections.Generic;

public class ClosestTargetStrategy : ITargetSearchStrategy
{
    // Tham số LayerMask giờ đây có thể bỏ qua (hoặc giữ lại trong interface nếu bạn dùng cho việc khác)
    public BaseUnit SelectTarget(BaseUnit owner, float detectRange, LayerMask enemyLayer)
    {
        // 1. Xác định phe địch dựa vào phe của Owner
        // Nếu owner là Player, lấy list Enemy. Ngược lại, lấy list Player.
        IReadOnlyList<BaseUnit> enemyList = (owner.Team == TeamType.Player)
            ? BattleManager.Instance.EnemyUnits
            : BattleManager.Instance.PlayerUnits;
       // Debug.Log($"{owner.name} đang tìm mục tiêu. Kẻ địch hiện có trong List: {enemyList.Count}");

        BaseUnit bestTarget = null;
        float closestDistance = Mathf.Infinity;

        // 2. Duyệt qua tất cả lính địch đang có trên sân
        foreach (var potentialTarget in enemyList)
        {
            // Bỏ qua nếu mục tiêu bị null hoặc đã chết 
            // (Dù BattleManager đã xóa lính chết khỏi List, check thêm vẫn an toàn hơn)
            if (potentialTarget == null || potentialTarget.Health.IsDead) continue;

            // Tính khoảng cách từ Owner đến mục tiêu
            float dist = Vector2.Distance(owner.transform.position, potentialTarget.transform.position);

            // 3. Kiểm tra xem mục tiêu có NẰM TRONG TẦM NHÌN và GẦN NHẤT không
            if (dist <= detectRange && dist < closestDistance)
            {
                closestDistance = dist;
                bestTarget = potentialTarget;
            }
        }

       // Debug.Log(Có thể bật lên để test, nhưng nên tắt khi build game để tối ưu)
         if (bestTarget != null)
        {
           // Debug.Log($"<color=green>[Targeting]</color> {owner.name} đã chọn: {bestTarget.name} (Cách {closestDistance:F2}m)");
        }

        return bestTarget;
    }
}