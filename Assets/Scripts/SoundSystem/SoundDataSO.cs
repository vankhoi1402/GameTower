using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Audio/Sound Data")]
public class SoundDataSO : ScriptableObject
{
    public SoundType soundType;
    public AudioClip[] clips;

    [Range(0f, 1f)] public float volume = 0.8f;
    [Range(0.1f, 2f)] public float minPitch = 0.9f;
    [Range(0.1f, 2f)] public float maxPitch = 1.1f;

    [Header("Mixer Output")]
    public AudioMixerGroup mixerGroup;

    [Header("T?i ?u hóa s? l??ng phát")]
    public int maxSimultaneousSounds = 3;
    public float minTimeBetweenPlays = 0.05f;

    private float _lastPlayedTime;
    private int _currentPlayingCount;

    public void ResetRuntimeData()
    {
        _lastPlayedTime = -999f;
        _currentPlayingCount = 0;
    }

    public bool CanPlay()
    {
        if (clips == null || clips.Length == 0) return false;
        if (Time.time - _lastPlayedTime < minTimeBetweenPlays) return false;
        if (_currentPlayingCount >= maxSimultaneousSounds) return false;
        return true;
    }

    public void TrackPlay()
    {
        _lastPlayedTime = Time.time;
        _currentPlayingCount++;
    }

    public void TrackStop()
    {
        _currentPlayingCount = Mathf.Max(0, _currentPlayingCount - 1);
    }

    public AudioClip GetRandomClip()
    {
        if (clips.Length == 1) return clips[0];
        return clips[Random.Range(0, clips.Length)];
    }
}