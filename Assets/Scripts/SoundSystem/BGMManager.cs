using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [Header("Danh sách cấu hình Nhạc nền")]
    [SerializeField] private List<SoundDataSO> bgmRegistry;

    private Dictionary<SoundType, SoundDataSO> _bgmDictionary;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeRegistry();

        // Tự tạo AudioSource riêng cho nhạc nền
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 0f; // Nhạc nền luôn là 2D
        _audioSource.loop = true;       // Luôn lặp lại
    }
    private void Start()
    {
        // Vừa vào game là tự kích hoạt bài nhạc nền đầu tiên (Menu)
        PlayBGM(SoundType.SO_BGM_Menu);
    }

    private void InitializeRegistry()
    {
        _bgmDictionary = new Dictionary<SoundType, SoundDataSO>();
        foreach (var data in bgmRegistry)
        {
            if (data != null) { _bgmDictionary[data.soundType] = data; }
        }
    }

    // Hàm gọi phát nhạc nền từ bất cứ đâu
    public void PlayBGM(SoundType type)
    {
        if (!_bgmDictionary.TryGetValue(type, out var data)) return;

        AudioClip clipToPlay = data.clips[0]; // BGM thường lấy clip đầu tiên
        if (clipToPlay == null) return;

        // Nếu bài này đang phát rồi thì bỏ qua
        if (_audioSource.clip == clipToPlay && _audioSource.isPlaying) return;

        _audioSource.Stop();
        _audioSource.outputAudioMixerGroup = data.mixerGroup; // Đi theo Slider BGM của Mixer
        _audioSource.clip = clipToPlay;
        _audioSource.volume = data.volume;
        _audioSource.Play();
    }

    public void StopBGM()
    {
        _audioSource.Stop();
    }
}