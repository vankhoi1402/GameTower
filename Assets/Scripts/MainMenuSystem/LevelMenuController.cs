using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class LevelMenuController : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField] private List<ChapterData> chaptersList = new List<ChapterData>();

    // Trạng thái (State)
    private ChapterData selectedChapter;
    private LevelData selectedLevel;

    // Các thành phần View (chịu trách nhiệm vẽ)
    private ChapterSidebarView sidebarView;
    private LevelGridView gridView;

    // UI Elements cục bộ (của riêng Controller)
    private Label txtCurrentChapterTitle;
    private Label txtRewardName;
    private VisualElement imgRewardIcon;
    private Button btnPlay;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 1. Ánh xạ UI
        txtCurrentChapterTitle = root.Q<Label>("TxtCurrentChapterTitle");
        txtRewardName = root.Q<Label>("TxtRewardName");
        imgRewardIcon = root.Q<VisualElement>("ImgRewardIcon");
        btnPlay = root.Q<Button>("BtnPlay");

        btnPlay.clicked += OnPlayButtonClicked;
        btnPlay.SetEnabled(false);

        // 2. Khởi tạo các Views và lắng nghe sự kiện
        sidebarView = new ChapterSidebarView(root.Q<ScrollView>("ChapterContainer"));
        sidebarView.OnChapterClicked += HandleChapterSelected;

        gridView = new LevelGridView(root.Q<ScrollView>("LevelGridContainer"));
        gridView.OnLevelClicked += HandleLevelSelected;

        // 3. Render lần đầu tiên
        RefreshMenuSystem();
    }

    public void RefreshMenuSystem()
    {
        // Chọn mặc định chương đầu tiên nếu chưa có
        if (chaptersList.Count > 0 && selectedChapter == null)
        {
            HandleChapterSelected(chaptersList[0]);
        }
        else
        {
            sidebarView.Render(chaptersList, selectedChapter);
        }
    }

    private void HandleChapterSelected(ChapterData chapter)
    {
        selectedChapter = chapter;
        selectedLevel = null;
        btnPlay.SetEnabled(false);

        // Cập nhật Header & Reward UI
        txtCurrentChapterTitle.text = $"{chapter.chapterName.ToUpper()} - LEVEL SELECT";
        txtRewardName.text = chapter.rewardName;

        if (chapter.rewardIcon != null) imgRewardIcon.style.backgroundImage = new StyleBackground(chapter.rewardIcon);
        else imgRewardIcon.style.backgroundImage = null;

        // Yêu cầu các Views vẽ lại màn hình
        sidebarView.Render(chaptersList, selectedChapter);
        gridView.Render(chapter);
    }

    private void HandleLevelSelected(LevelData level)
    {
        selectedLevel = level;
        btnPlay.SetEnabled(true); // Chỉ khi chọn level mới được phép bấm Play
    }

    private void OnPlayButtonClicked()
    {
        if (selectedLevel != null && !string.IsNullOrEmpty(selectedLevel.sceneToLoad))
        {
            // Bàn giao cho GameManager vận chuyển dữ liệu vào Scene trận đấu
            GameMenuManager.Instance.StartBattle(selectedLevel, selectedLevel.sceneToLoad);
        }
    }
}