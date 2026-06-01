using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chapter", menuName = "RTS/Chapter Data")]
public class ChapterData : ScriptableObject
{
    public string chapterName = "Tên Vùng Đất";
    [TextArea(2, 4)] public string chapterDescription = "Mô tả ngắn...";
    public Sprite chapterIcon;

    [Header("Reward Info")]
    public string rewardName = "ARCHER";
    public Sprite rewardIcon;

    [Header("Levels List")]
    public List<LevelData> levels = new List<LevelData>();

    // Tính xem vùng này có bao nhiêu màn đã vượt qua
    public int GetCompletedLevelsCount()
    {
        int count = 0;
        foreach (var lvl in levels)
        {
            if (lvl != null && SaveSystem.IsLevelPassed(lvl.levelName)) count++;
        }
        return count;
    }

    // Kiểm tra xem đã thắng sạch toàn bộ màn trong chương chưa
    public bool IsChapterFullyCompleted()
    {
        if (levels == null || levels.Count == 0) return false;
        foreach (var lvl in levels)
        {
            if (lvl != null && !SaveSystem.IsLevelPassed(lvl.levelName)) return false;
        }
        return true;
    }
}