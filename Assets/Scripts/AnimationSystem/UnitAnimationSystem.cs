using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnimationSystem : MonoBehaviour, IUnitAnimator
{
    private Animator _animator;

    // Quản lý State bằng Hash ID (Tối ưu hiệu năng CPU)
    private int _currentStateHash;
    private int _bufferedStateHash; // NƠI LƯU LỆNH CHỜ: Nhớ xem Logic muốn gì khi đang bị Khóa
    private int _defaultStateHash;  // TRẠNG THÁI DỰ PHÒNG: Thường là Idle

    private bool _isLocked;
    private const float CrossFadeDuration = 0.15f; // Thời gian hòa trộn mượt mặc định

    public event Action OnActionTriggered;

    // Cache sẵn Hash ID để không phải duyệt String liên tục mỗi frame
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int MoveHash = Animator.StringToHash("Move");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _defaultStateHash = IdleHash;
        _currentStateHash = IdleHash;
        _bufferedStateHash = IdleHash;
    }

    // --- CỔNG NHẬN LỆNH TỪ LOGIC (FIRE-AND-FORGET) ---
    public void Play(UnitAnimState state, bool lockAnim = false, bool forceReplay = false)
    {
        int targetHash = GetHashFromState(state);

        // Trường hợp 1: Hệ thống đang bị KHÓA (Ví dụ: đang dở một cú chém quan trọng)
        if (_isLocked)
        {
            // Nếu lệnh mới đến là "Chết", phá khóa lập tức để ưu tiên hoạt họa Chết
            if (state == UnitAnimState.Die)
            {
                Unlock();
            }
            else
            {
                // Ghi nhớ lệnh này vào bộ đệm, chờ đánh xong sẽ lôi ra xử lý
                _bufferedStateHash = targetHash;
                return;
            }
        }

        // Trường hợp 2: Hệ thống đang TỰ DO
        if (_currentStateHash == targetHash && !forceReplay)
            return; // Đang chạy đúng hoạt họa đó rồi thì thôi

        // Thực hiện chuyển đổi hoạt họa mượt mà
        if (forceReplay)
        {
            _animator.Play(targetHash, 0, 0f); // Ép phát lại ngay lập tức từ frame 0
        }
        else
        {
            _animator.CrossFadeInFixedTime(targetHash, CrossFadeDuration);
        }

        // Cập nhật trạng thái hiện tại và bộ đệm
        _currentStateHash = targetHash;
        _bufferedStateHash = targetHash;
        _isLocked = lockAnim;
    }

    public void Unlock()
    {
        _isLocked = false;
    }

    private int GetHashFromState(UnitAnimState state)
    {
        return state switch
        {
            UnitAnimState.Idle => IdleHash,
            UnitAnimState.Move => MoveHash,
            UnitAnimState.Attack => AttackHash,
            UnitAnimState.Die => DieHash,
            _ => IdleHash
        };
    }

    // ==============================================================
    // ANIMATION EVENTS (Hệ thống tự gọi ngầm từ Clip của Unity)
    // ==============================================================

    // Gắn vào frame giữa của Clip Tấn công (Lúc buông dây cung / Kiếm chạm địch)
    public void UnityEvent_OnActionFrame()
    {
        // Bắn tín hiệu ra cho AttackSystem nghe để gây sát thương
        OnActionTriggered?.Invoke();
    }

    // Gắn vào frame CUỐI CÙNG của Clip Tấn công (Lúc thu vũ khí xong)
    public void UnityEvent_OnAnimationFinished()
    {
        // 1. Mở khóa hệ thống
        Unlock();

        // 2. TỰ XỬ LÝ NỘI BỘ: Kiểm tra bộ đệm xem trong lúc mình đang đánh, 
        // Logic bên ngoài có ra lệnh cho mình làm gì khác không (Ví dụ: lệnh Di chuyển)
        if (_bufferedStateHash != _currentStateHash)
        {
            // Nếu có lệnh mới đang chờ (VD: Move), phát ngay lập tức
            _animator.CrossFadeInFixedTime(_bufferedStateHash, CrossFadeDuration);
            _currentStateHash = _bufferedStateHash;
        }
        else
        {
            // Nếu không có lệnh mới nào (Logic vẫn im lặng), tự động quay về Idle đứng chờ
            _animator.CrossFadeInFixedTime(_defaultStateHash, CrossFadeDuration);
            _currentStateHash = _defaultStateHash;
            _bufferedStateHash = _defaultStateHash;
        }
    }
}