using System.Collections.Generic;

public static class PendingPopups
{
    private static readonly Queue<ChapterData> _chapterUnlocks = new();

    public static void AddChapterUnlock(ChapterData chapter)
    {
        _chapterUnlocks.Enqueue(chapter);
    }

    public static bool TryGetNextChapterUnlock(out ChapterData chapter)
    {
        if (_chapterUnlocks.Count > 0)
        {
            chapter = _chapterUnlocks.Dequeue();
            return true;
        }

        chapter = null;
        return false;
    }
}