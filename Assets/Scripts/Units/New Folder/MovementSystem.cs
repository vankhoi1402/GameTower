using UnityEngine;

public class MovementSystem
{
    private BaseUnit _owner;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Lưu hash của Parameter để tối ưu hiệu suất thay vì dùng string
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

    public MovementSystem(BaseUnit owner)
    {
        _owner = owner;
        _spriteRenderer = owner.GetComponentInChildren<SpriteRenderer>();
        _animator = owner.GetComponentInChildren<Animator>();
    }

    // Hàm di chuyển chính - Sẽ được gọi trong MoveState.OnUpdate()
    public void MoveTo(Vector2 targetPos, float speed)
    {
        // 1. Tính toán hướng di chuyển
        Vector2 currentPos = _owner.transform.position;
        _owner.transform.position = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);

        // 2. Lật Sprite dựa trên hướng x
        HandleFlip(targetPos.x - currentPos.x);

        // 3. Cập nhật Animation
        UpdateAnimation(true);
    }

    // Hàm dừng lại - Sẽ được gọi trong IdleState.OnEnter() hoặc AttackState.OnEnter()
    public void Stop()
    {
        UpdateAnimation(false);
    }

    private void HandleFlip(float horizontalDirection)
    {
        if (horizontalDirection > 0.01f) _spriteRenderer.flipX = false;
        else if (horizontalDirection < -0.01f) _spriteRenderer.flipX = true;
    }

    private void UpdateAnimation(bool isMoving)
    {
        if (_animator != null)
        {
            _animator.SetBool(IsMovingHash, isMoving);
        }
    }
}