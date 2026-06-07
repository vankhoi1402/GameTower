using UnityEngine;

public static class SaveSystem
{
    private const string COMPLETED_KEY = "Level_Completed_";
    private const string STARS_KEY = "Level_Stars_";

    // Thêm một Key mới để lưu trạng thái hiển thị Popup
    private const string POPUP_SHOWN_KEY = "UnlockPopup_";

    // Đánh dấu một màn chơi là đã CHIẾN THẮNG
    public static void SetLevelVictory(string levelName, int starsEarned)
    {
        PlayerPrefs.SetInt(COMPLETED_KEY + levelName, 1); // 1 tức là đã Thắng

        // Chỉ cập nhật nếu số sao lần này cao hơn lần trước
        int currentStars = PlayerPrefs.GetInt(STARS_KEY + levelName, 0);
        if (starsEarned > currentStars)
        {
            PlayerPrefs.SetInt(STARS_KEY + levelName, starsEarned);
        }

        PlayerPrefs.Save();
    }

    // Kiểm tra xem màn chơi đó ĐÃ THẮNG CHƯA
    public static bool IsLevelPassed(string levelName)
    {
        return PlayerPrefs.GetInt(COMPLETED_KEY + levelName, 0) == 1;
    }

    // Lấy số sao đã đạt được của màn đó
    public static int GetLevelStars(string levelName)
    {
        return PlayerPrefs.GetInt(STARS_KEY + levelName, 0);
    }

    // ---------------------------------------------------------
    // QUẢN LÝ POPUP MỞ KHÓA CHƯƠNG
    // ---------------------------------------------------------

    // Kiểm tra xem đã show popup mở khóa cho chương này chưa
    public static bool HasShownUnlockPopup(string chapterName)
    {
        return PlayerPrefs.GetInt(POPUP_SHOWN_KEY + chapterName, 0) == 1;
    }

    // Ghi nhớ lại việc đã show popup để lần sau không hiển thị đè nữa
    public static void SetUnlockPopupShown(string chapterName)
    {
        PlayerPrefs.SetInt(POPUP_SHOWN_KEY + chapterName, 1);
        PlayerPrefs.Save(); // Lưu ngay lập tức để đảm bảo an toàn dữ liệu
    }
}