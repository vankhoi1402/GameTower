using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class LevelGridView
{
    private ScrollView gridContainer;
    private List<VisualElement> cellElements = new List<VisualElement>();

    // Sự kiện bắn ra khi người chơi click vào một màn chơi
    public event Action<LevelData> OnLevelClicked;

    public LevelGridView(ScrollView container)
    {
        gridContainer = container;
    }

    public void Render(ChapterData chapter)
    {
        gridContainer.Clear();
        cellElements.Clear();
        bool previousLevelCompleted = true; // Màn 1 luôn mở

        for (int i = 0; i < chapter.levels.Count; i++)
        {
            LevelData level = chapter.levels[i];
            if (level == null) continue;

            bool isPassed = SaveSystem.IsLevelPassed(level.levelName);
            bool isLevelUnlocked = previousLevelCompleted;
            previousLevelCompleted = isPassed;

            VisualElement cell = CreateLevelCell(level, i + 1, isLevelUnlocked, isPassed);
            gridContainer.Add(cell);
            cellElements.Add(cell);
        }
    }

    private VisualElement CreateLevelCell(LevelData level, int levelIndex, bool isUnlocked, bool isPassed)
    {
        VisualElement cell = new VisualElement();
        cell.AddToClassList("level-cell");

        Label numTxt = new Label(levelIndex.ToString());
        numTxt.AddToClassList("level-num-txt");
        cell.Add(numTxt);

        Label statusTxt = new Label();
        statusTxt.AddToClassList("level-status-txt");
        cell.Add(statusTxt);

        if (!isUnlocked)
        {
            cell.AddToClassList("level-cell--locked");
            statusTxt.text = "LOCKED 🔒";
        }
        else if (isPassed)
        {
            cell.AddToClassList("level-cell--victory");
            statusTxt.text = $"★ {SaveSystem.GetLevelStars(level.levelName)}";
            cell.RegisterCallback<ClickEvent>(evt => HandleLevelSelection(level, cell));
        }
        else
        {
            statusTxt.text = "READY";
            cell.RegisterCallback<ClickEvent>(evt => HandleLevelSelection(level, cell));
        }

        return cell;
    }

    private void HandleLevelSelection(LevelData level, VisualElement selectedCell)
    {
        // Gỡ class selected cũ khỏi toàn bộ cell
        foreach (var cell in cellElements) cell.RemoveFromClassList("level-cell--selected");

        // Thêm highlight cho cell vừa chọn
        selectedCell.AddToClassList("level-cell--selected");

        // Báo cho Controller biết màn này đã được chọn
        OnLevelClicked?.Invoke(level);
    }
}