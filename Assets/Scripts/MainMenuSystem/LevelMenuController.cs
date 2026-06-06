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

    // ---- ĐÃ THÊM 2 DÒNG NÀY ----
    private Button btnSettings;
    private Button btnBack;
    private string homeScene = "HomeScene";

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

        // ---- ĐÃ THÊM ÁNH XẠ VÀ ĐĂNG KÝ SỰ KIỆN CHO 2 NÚT HEADER ----
        btnSettings = root.Q<Button>("BtnSettings");
        btnBack = root.Q<Button>("BtnBack");

        if (btnSettings != null) btnSettings.clicked += OnSettingsButtonClicked;
        if (btnBack != null) btnBack.clicked += OnBackButtonClicked;

        // 2. Khởi tạo các Views và lắng nghe sự kiện
        sidebarView = new ChapterSidebarView(root.Q<ScrollView>("ChapterContainer"));
        sidebarView.OnChapterClicked += HandleChapterSelected;

        gridView = new LevelGridView(root.Q<ScrollView>("LevelGridContainer"));
        gridView.OnLevelClicked += HandleLevelSelected;

        // 3. Render lần đầu tiên
        RefreshMenuSystem();
    }

    // ---- ĐỪNG QUÊN HỦY ĐĂNG KÝ SỰ KIỆN TRONG ONDISABLE ĐỂ TRÁNH LỖI ----
    void OnDisable()
    {
        if (btnPlay != null) btnPlay.clicked -= OnPlayButtonClicked;
        if (btnSettings != null) btnSettings.clicked -= OnSettingsButtonClicked;
        if (btnBack != null) btnBack.clicked -= OnBackButtonClicked;

        if (sidebarView != null) sidebarView.OnChapterClicked -= HandleChapterSelected;
        if (gridView != null) gridView.OnLevelClicked -= HandleLevelSelected;
    }

    public void RefreshMenuSystem()
    {
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

        txtCurrentChapterTitle.text = $"{chapter.chapterName.ToUpper()} - LEVEL SELECT";
        txtRewardName.text = chapter.rewardName;

        if (chapter.rewardIcon != null) imgRewardIcon.style.backgroundImage = new StyleBackground(chapter.rewardIcon);
        else imgRewardIcon.style.backgroundImage = null;

        sidebarView.Render(chaptersList, selectedChapter);
        gridView.Render(chapter);
    }

    private void HandleLevelSelected(LevelData level)
    {
        selectedLevel = level;
        btnPlay.SetEnabled(true);
    }

    private void OnPlayButtonClicked()
    {
        if (selectedLevel != null && !string.IsNullOrEmpty(selectedLevel.sceneToLoad))
        {
            GameMenuManager.Instance.StartBattle(selectedLevel, selectedLevel.sceneToLoad);
        }
        UIEvents.RaisePlaySound(SoundType.UI_Click);
    }

    // ---- ĐÃ THÊM LOGIC CHO 2 NÚT MỚI ----
    private void OnSettingsButtonClicked()
    {
        Debug.Log("Mở bảng cài đặt (Settings)...");
        
        SettingsManager.Instance.Open();
        UIEvents.RaisePlaySound(SoundType.UI_Click);
    }

    private void OnBackButtonClicked()
    {
        Debug.Log("Quay lại màn hình trước đó...");
        // Gọi logic quay lại Main Menu của bạn ở đây
        GameMenuManager.Instance.BackToMenu(homeScene);
        UIEvents.RaisePlaySound(SoundType.UI_Click);
    }
}