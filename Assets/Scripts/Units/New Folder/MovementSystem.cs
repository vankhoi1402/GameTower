using UnityEngine;

public class MovementSystem
{
    private BaseUnit _owner;
    private SpriteRenderer _spriteRenderer;
    

    // --- CẤU HÌNH TÁCH BẦY (Chuyển từ State sang đây) ---
    private float _separationRadius = 1.0f;
    private float _separationWeight = 1.5f;
    private Collider2D[] _neighbors = new Collider2D[10];

    public MovementSystem(BaseUnit owner)
    {
        _owner = owner;
        _spriteRenderer = owner.GetComponentInChildren<SpriteRenderer>();
        
    }

    // Hàm MoveTo nhận tọa độ đích, TỰ ĐỘNG tính toán việc né đồng đội
    public void MoveTo(Vector3 targetPos, float speed)
    {
        Vector3 currentPos = _owner.transform.position;

        // 1. Tính hướng đi thẳng tới mục tiêu
        Vector3 directionToTarget = (targetPos - currentPos).normalized;

        // 2. Tính lực né đồng đội
        Vector3 separationDirection = CalculateSeparation(currentPos);

        // 3. Tổng hợp hướng đi cuối cùng
        Vector3 finalDirection = (directionToTarget + (separationDirection * _separationWeight)).normalized;

        // 4. Thực thi di chuyển bằng Transform
        _owner.transform.position += finalDirection * (speed * Time.deltaTime);

        // 5. Cập nhật hình ảnh & Hoạt họa
        HandleFlip(finalDirection.x);

      
    }

    public void Stop()
    {
        
    }

    private void HandleFlip(float horizontalDirection)
    {
        if (horizontalDirection > 0.01f) _spriteRenderer.flipX = false;
        else if (horizontalDirection < -0.01f) _spriteRenderer.flipX = true;
    }

    // --- HÀM TÍNH TOÁN LỰC ĐẨY KÍN BÊN TRONG ---
    private Vector3 CalculateSeparation(Vector3 currentPos)
    {
        Vector3 separationMove = Vector3.zero;
        int neighborCount = 0;

        // 1. Tạo một bộ lọc (Filter) cơ bản. NoFilter() nghĩa là quét mọi thứ.
        // (Sau này bạn có thể tối ưu thêm bằng cách thiết lập filter.SetLayerMask để chỉ quét trúng layer "Quân lính")
        ContactFilter2D filter = ContactFilter2D.noFilter;

        // 2. Dùng hàm OverlapCircle kiểu mới: Truyền thêm filter và mảng _neighbors vào
        // Vì bạn truyền mảng _neighbors vào, Unity tự hiểu đây là cách quét "NonAlloc" (Không sinh rác bộ nhớ)
        int hitCount = Physics2D.OverlapCircle((Vector2)currentPos, _separationRadius, filter, _neighbors);

        // Đoạn dưới này giữ nguyên 100% không cần đổi
        for (int i = 0; i < hitCount; i++)
        {
            Collider2D neighbor = _neighbors[i];

            if (neighbor.gameObject != _owner.gameObject)
            {
                Vector3 awayFromNeighbor = currentPos - neighbor.transform.position;
                float distance = awayFromNeighbor.magnitude;

                if (distance > 0 && distance < _separationRadius)
                {
                    separationMove += awayFromNeighbor.normalized / distance;
                    neighborCount++;
                }
            }
        }

        if (neighborCount > 0)
        {
            separationMove /= neighborCount;
        }

        return separationMove;
    }
}