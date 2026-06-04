using System.Collections;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private void OnEnable()
    {
        // 1. Lắng nghe sự kiện kết thúc từ Event Bus khi script được kích hoạt
        GlobalEventBus.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        // Hủy lắng nghe để tránh lỗi bộ nhớ (Memory Leak)
        GlobalEventBus.OnMatchEnded -= HandleMatchEnded;
    }

    private void HandleMatchEnded(MatchResult result)
    {
        // 2. Gửi yêu cầu đổi trạng thái Game sang EndGame để UIManager bật UI EndGamePanel lên
        GameEvents.CallStateChange(GameState.EndGame);

        // 3. Phân nhánh xử lý logic lưu dữ liệu dựa trên kết quả Thắng / Thua
        if (result == MatchResult.Victory)
        {
            OnPlayerVictory(); // Đã bỏ truyền số Sao (3) đi theo ý bạn
        }
        else
        {
            OnPlayerDefeat();
        }
    }

    // LÔGIC KHI CHIẾN THẮNG
    public void OnPlayerVictory()
    {
        if (GameMenuManager.Instance != null &&
            GameMenuManager.Instance.SelectedLevelData != null)
        {
            string currentLevelName = GameMenuManager.Instance.SelectedLevelData.levelName;

            // Lưu tiến trình (Nếu hàm SetLevelVictory bắt buộc nhận int, bạn truyền tạm số 1 nhé)
            SaveSystem.SetLevelVictory(currentLevelName, 1);

            Debug.Log($"[BattleController] Đã lưu chiến thắng cho màn: {currentLevelName}.");
            BGMManager.Instance.PlayBGM(SoundType.SO_BGM_GameWin);
        }

        // BỎ: Không gọi StartCoroutine(ReturnToMenuAfterDelay()) ở đây nữa.
        // Việc chuyển scene giờ sẽ do người chơi chủ động bấm nút trên UI Toolkit quyết định.
    }

    // LÔGIC KHI THẤT BẠI
    public void OnPlayerDefeat()
    {
        Debug.Log("[BattleController] Thất bại! Không lưu tiến trình.");

        // BỎ: Không tự động quay về menu sau 2 giây nữa, để người chơi nhìn thấy bảng DEFEAT.
    }
}