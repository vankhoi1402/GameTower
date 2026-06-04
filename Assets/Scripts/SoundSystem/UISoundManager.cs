using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance { get; private set; }

    [Header("Cấu hình Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Cấu hình Pool cho UI (Chỉ cần nhỏ)")]
    [SerializeField] private List<SoundDataSO> uiSoundRegistry;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int uiPoolSize = 5; // UI chỉ cần 5 cái là đủ

    private Dictionary<SoundType, SoundDataSO> _uiSoundDictionary;
    private Queue<PooledAudioSource> _uiPoolQueue;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeRegistry();
        InitializePool();
    }

    private void Start()
    {
        // Tự động load lại cài đặt âm lượng khi mở game
        LoadVolumeSettings();
    }

    private void OnEnable() => UIEvents.OnPlayUISound += PlayUISoundHandler;
    private void OnDisable() => UIEvents.OnPlayUISound -= PlayUISoundHandler;

    private void InitializeRegistry()
    {
        _uiSoundDictionary = new Dictionary<SoundType, SoundDataSO>();
        foreach (var data in uiSoundRegistry)
        {
            if (data != null) { data.ResetRuntimeData(); _uiSoundDictionary[data.soundType] = data; }
        }
    }

    private void InitializePool()
    {
        _uiPoolQueue = new Queue<PooledAudioSource>();
        for (int i = 0; i < uiPoolSize; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab, transform);
            PooledAudioSource source = obj.GetComponent<PooledAudioSource>() ?? obj.AddComponent<PooledAudioSource>();
            obj.SetActive(false);
            _uiPoolQueue.Enqueue(source);
        }
    }

    private void PlayUISoundHandler(SoundType type)
    {
        if (!_uiSoundDictionary.TryGetValue(type, out var data) || !data.CanPlay()) return;

        PooledAudioSource source = _uiPoolQueue.Count == 0 ? CreateNewInstance() : _uiPoolQueue.Dequeue();
        source.Play2D(data, ReturnToPool);
    }

    private PooledAudioSource CreateNewInstance()
    {
        GameObject obj = Instantiate(audioSourcePrefab, transform);
        var source = obj.GetComponent<PooledAudioSource>() ?? obj.AddComponent<PooledAudioSource>();
        obj.SetActive(false);
        return source;
    }

    private void ReturnToPool(PooledAudioSource source) => _uiPoolQueue.Enqueue(source);

    // --- HỆ THỐNG QUẢN LÝ VOLUME SETTINGS & PLAYERPREFS ---
    public void SetVolume(AudioChannel channel, float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        float dB = Mathf.Log10(value) * 20f; // Đổi sang Decibel

        // Đảm bảo tên tham số truyền vào trùng khớp với tên bạn đổi trong Exposed Parameters của Mixer
        switch (channel)
        {
            case AudioChannel.Master: mainMixer.SetFloat("MasterVol", dB); break;
            case AudioChannel.BGM: mainMixer.SetFloat("BGMVol", dB); break;
            case AudioChannel.UI_SFX: mainMixer.SetFloat("UISFXVol", dB); break;
            case AudioChannel.Battle_SFX: mainMixer.SetFloat("BattleSFXVol", dB); break;
        }

        PlayerPrefs.SetFloat(channel.ToString() + "_Volume", value);
        PlayerPrefs.Save();
    }

    public float GetSavedVolume(AudioChannel channel)
    {
        return PlayerPrefs.GetFloat(channel.ToString() + "_Volume", 0.75f);
    }

    private void LoadVolumeSettings()
    {
        SetVolume(AudioChannel.Master, GetSavedVolume(AudioChannel.Master));
        SetVolume(AudioChannel.BGM, GetSavedVolume(AudioChannel.BGM));
        SetVolume(AudioChannel.UI_SFX, GetSavedVolume(AudioChannel.UI_SFX));
        SetVolume(AudioChannel.Battle_SFX, GetSavedVolume(AudioChannel.Battle_SFX));
    }
}