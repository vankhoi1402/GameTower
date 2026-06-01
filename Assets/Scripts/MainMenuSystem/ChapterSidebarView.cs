using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ChapterSidebarView
{
    private ScrollView chapterContainer;

    // Sự kiện bắn ra khi người chơi click vào một thẻ Chapter
    public event Action<ChapterData> OnChapterClicked;

    public ChapterSidebarView(ScrollView container)
    {
        chapterContainer = container;
    }

    public void Render(List<ChapterData> chaptersList, ChapterData selectedChapter)
    {
        chapterContainer.Clear();
        bool previousChapterCompleted = true; // Chương đầu tiên luôn mở

        for (int i = 0; i < chaptersList.Count; i++)
        {
            ChapterData chapter = chaptersList[i];
            bool isChapterUnlocked = previousChapterCompleted;
            previousChapterCompleted = chapter.IsChapterFullyCompleted();

            VisualElement card = CreateChapterCard(chapter, isChapterUnlocked, selectedChapter == chapter);
            chapterContainer.Add(card);
        }
    }

    private VisualElement CreateChapterCard(ChapterData chapter, bool isUnlocked, bool isSelected)
    {
        VisualElement card = new VisualElement();
        card.AddToClassList("chapter-card");

        if (!isUnlocked)
        {
            card.AddToClassList("chapter-card--locked");
        }
        else
        {
            if (isSelected) card.AddToClassList("chapter-card--selected");
            // Bắn sự kiện ra ngoài khi bị click
            card.RegisterCallback<ClickEvent>(evt => OnChapterClicked?.Invoke(chapter));
        }

        VisualElement icon = new VisualElement();
        icon.AddToClassList("chapter-icon");
        if (chapter.chapterIcon != null) icon.style.backgroundImage = new StyleBackground(chapter.chapterIcon);
        card.Add(icon);

        VisualElement infoGroup = new VisualElement();
        infoGroup.AddToClassList("chapter-info-holder");

        Label nameLbl = new Label(isUnlocked ? chapter.chapterName : "🔒 KHU VỰC BỊ KHÓA");
        nameLbl.AddToClassList("chapter-name");
        infoGroup.Add(nameLbl);

        if (isUnlocked)
        {
            Label progressLbl = new Label($"Hoàn thành: {chapter.GetCompletedLevelsCount()}/{chapter.levels.Count}");
            progressLbl.AddToClassList("chapter-progress");
            infoGroup.Add(progressLbl);
        }

        card.Add(infoGroup);
        return card;
    }
}