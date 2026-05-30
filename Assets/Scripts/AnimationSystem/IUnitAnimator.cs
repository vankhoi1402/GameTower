using System;

public interface IUnitAnimator
{
    event Action OnActionTriggered; // Bắn ra đúng frame hành động (buông cung/chém)

    // Nhận lệnh phát hoạt họa (Chỉ cần truyền tên trạng thái và cờ cấu hình)
    void Play(UnitAnimState state, bool lockAnim = false, bool forceReplay = false);

    void Unlock();
}