using UnityEngine;

public static class SaveSystem
{
    private const string COMPLETED_KEY = "Level_Completed_";
    private const string STARS_KEY = "Level_Stars_";

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
}