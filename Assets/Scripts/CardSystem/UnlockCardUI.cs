using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

[RequireComponent(typeof(UIDocument))]
public class UnlockCardUI : MonoBehaviour
{
    [Header("--- THỜI GIAN HIỆU ỨNG (CHỈNH TRÊN INSPECTOR) ---")]
    [Tooltip("Thời gian nền tối dần và card hiện hình lên (Khuyên dùng: 0.5 - 0.7)")]
    [SerializeField] private float _fadeInDuration = 0.6f;

    [Tooltip("Thời gian cái card phóng to nảy ra (Khuyên dùng: 0.6 - 0.8)")]
    [SerializeField] private float _scaleInDuration = 1f;

    [Tooltip("Thời gian toàn bộ UI thu nhỏ và biến mất khi bấm Confirm (Khuyên dùng: 0.3 - 0.4)")]
    [SerializeField] private float _fadeOutDuration = 0.4f;

    private UIDocument _document;
    private VisualElement _overlay;
    private VisualElement _cardContainer;

    private VisualElement _chapterIcon;
    private VisualElement _rewardIcon;
    private Label _chapterName;
    private Label _chapterDescription;
    private Label _rewardName;
    private Button _confirmButton;

    private Action _onClosed;
    private bool _isInitialized = false;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_isInitialized) return;

        _document = GetComponent<UIDocument>();
        if (_document == null || _document.rootVisualElement == null) return;

        VisualElement root = _document.rootVisualElement;
        bool hasError = false;

        T FindElement<T>(string id) where T : VisualElement
        {
            T element = root.Q<T>(id);
            if (element == null)
            {
                Debug.LogError($"[UnlockCardUI] LỖI: Không tìm thấy thành phần UI có ID là: \"{id}\"");
                hasError = true;
            }
            return element;
        }

        _overlay = FindElement<VisualElement>("overlay");
        _cardContainer = FindElement<VisualElement>("card-container");
        _chapterIcon = FindElement<VisualElement>("chapter-icon");
        _rewardIcon = FindElement<VisualElement>("reward-icon");
        _chapterName = FindElement<Label>("chapter-name");
        _chapterDescription = FindElement<Label>("chapter-description");
        _rewardName = FindElement<Label>("reward-name");
        _confirmButton = FindElement<Button>("confirm-button");

        if (hasError) return;

        _confirmButton.clicked -= Confirm;
        _confirmButton.clicked += Confirm;
        _overlay.style.display = DisplayStyle.None;

        _isInitialized = true;
    }

    private void OnDestroy()
    {
        if (_confirmButton != null)
            _confirmButton.clicked -= Confirm;

        DOTween.Kill(_cardContainer);
        DOTween.Kill(_overlay);
    }

    public void Show(ChapterData completedChapter, ChapterData nextChapter, Action onClosed)
    {
        Initialize();
        BGMManager.Instance.PlayBGM(SoundType.SO_BGM_Reward);

        if (!_isInitialized) return;

        _onClosed = onClosed;

        _chapterName.text = "ĐÃ MỞ KHÓA: " + nextChapter.chapterName;
        _chapterDescription.text = nextChapter.chapterDescription;
        _rewardName.text = "PHẦN THƯỞNG: " + completedChapter.rewardName;

        if (nextChapter.chapterIcon != null)
            _chapterIcon.style.backgroundImage = new StyleBackground(nextChapter.chapterIcon);
        if (completedChapter.rewardIcon != null)
            _rewardIcon.style.backgroundImage = new StyleBackground(completedChapter.rewardIcon);

        _overlay.style.display = DisplayStyle.Flex;
        _overlay.pickingMode = PickingMode.Position;
        _overlay.BringToFront();

        // Reset về trạng thái ẩn hoàn toàn trước khi chạy animation
        _cardContainer.style.scale = new StyleScale(new Vector2(0.3f, 0.3f)); // Thu nhỏ sâu hơn để thấy rõ độ phóng to
        _cardContainer.style.opacity = 0f;
        _overlay.style.backgroundColor = new Color(0.04f, 0.05f, 0.08f, 0f);

        // --- ANIMATION XUẤT HIỆN ---

        // Nền tối dần
        DOTween.To(() => _overlay.style.backgroundColor.value,
                   x => _overlay.style.backgroundColor = x,
                   new Color(0.04f, 0.05f, 0.08f, 0.9f), _fadeInDuration).SetUpdate(true);

        // Card hiện rõ dần
        DOTween.To(() => _cardContainer.style.opacity.value,
                   x => _cardContainer.style.opacity = x,
                   1f, _fadeInDuration).SetUpdate(true);

        // Card phóng to nảy ra (Sử dụng biến _scaleInDuration mới)
        DOTween.To(() => _cardContainer.style.scale.value.value,
                   (Vector2 x) => _cardContainer.style.scale = new StyleScale(x),
                   Vector2.one, _scaleInDuration).SetEase(Ease.OutBack).SetUpdate(true);
    }

    private void Confirm()
    {
        if (!_isInitialized) return;
        BGMManager.Instance.PlayBGM(SoundType.SO_BGM_Menu);

        _overlay.pickingMode = PickingMode.Ignore;

        // --- ANIMATION BIẾN MẤT (Sử dụng biến _fadeOutDuration mới) ---

        // Nền mờ dần
        DOTween.To(() => _overlay.style.backgroundColor.value,
                   x => _overlay.style.backgroundColor = x,
                   new Color(0, 0, 0, 0), _fadeOutDuration).SetUpdate(true);

        // Card mờ dần
        DOTween.To(() => _cardContainer.style.opacity.value,
                   x => _cardContainer.style.opacity = x,
                   0f, _fadeOutDuration).SetUpdate(true);

        // Card thu nhỏ biến mất
        DOTween.To(() => _cardContainer.style.scale.value.value,
                   (Vector2 x) => _cardContainer.style.scale = new StyleScale(x),
                   new Vector2(0.4f, 0.4f), _fadeOutDuration).SetEase(Ease.InBack).SetUpdate(true)
            .OnComplete(() =>
            {
                _overlay.style.display = DisplayStyle.None;
                _onClosed?.Invoke();
            });
    }
}