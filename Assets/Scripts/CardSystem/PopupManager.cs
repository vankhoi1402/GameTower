using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private UnlockCardUI unlockCardUI;

    // SỬA DÒNG NÀY: Thay vì Queue<ChapterData> cũ
    private readonly Queue<(ChapterData completed, ChapterData unlocked)> _chapterQueue = new();

    private bool _isShowing;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Thay đổi tham số nhận vào thành 2 Chapter
    public void ShowChapterUnlock(ChapterData completed, ChapterData unlocked)
    {
        if (completed == null || unlocked == null) return;

        _chapterQueue.Enqueue((completed, unlocked));
        TryShowNext();
    }

    private void TryShowNext()
    {
        if (_isShowing || _chapterQueue.Count == 0)
            return;

        _isShowing = true;

        // 1. Lấy cặp dữ liệu (Chương cũ, Chương mới) ra khỏi hàng đợi
        var currentPair = _chapterQueue.Dequeue();

        // 2. SỬA Ở ĐÂY (DÒNG 48): Truyền đủ 3 tham số vào hàm Show
        unlockCardUI.Show(
            currentPair.completed,   // Tham số 1: ChapterData hoàn thành
            currentPair.unlocked,    // Tham số 2: ChapterData mới mở khóa
            OnPopupClosed            // Tham số 3: Action khi đóng popup
        );
    }

    private void OnPopupClosed()
    {
        _isShowing = false;
        TryShowNext();
    }
}