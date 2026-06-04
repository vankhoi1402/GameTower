using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PooledAudioSource : MonoBehaviour
{
    private AudioSource _audioSource;
    private SoundDataSO _currentData;
    private Action<PooledAudioSource> _onReturnToPool;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Dùng cho Battle (Phát 3D có vị trí)
    public void Play3D(SoundDataSO data, Vector3 position, Action<PooledAudioSource> returnCallback)
    {
        _currentData = data;
        _onReturnToPool = returnCallback;
        transform.position = position;

        ConfigureAudioSource(data);
        _audioSource.spatialBlend = 1.0f; // 3D Sound

        ExecutePlayback();
    }

    // Dùng cho UI (Phát 2D phẳng)
    public void Play2D(SoundDataSO data, Action<PooledAudioSource> returnCallback)
    {
        _currentData = data;
        _onReturnToPool = returnCallback;

        ConfigureAudioSource(data);
        _audioSource.spatialBlend = 0.0f; // 2D Sound

        ExecutePlayback();
    }

    private void ConfigureAudioSource(SoundDataSO data)
    {
        _audioSource.clip = data.GetRandomClip();
        _audioSource.volume = data.volume;
        _audioSource.pitch = UnityEngine.Random.Range(data.minPitch, data.maxPitch);
        _audioSource.outputAudioMixerGroup = data.mixerGroup;

        // Cấu hình khoảng cách cho Battle
        _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        _audioSource.minDistance = 5f;
        _audioSource.maxDistance = 35f;
    }

    private void ExecutePlayback()
    {
        gameObject.SetActive(true);
        _audioSource.Play();
        _currentData.TrackPlay();

        StartCoroutine(WaitUntilFinished(_audioSource.clip.length));
    }

    private IEnumerator WaitUntilFinished(float duration)
    {
        yield return new WaitForSeconds(duration + 0.02f);

        _audioSource.Stop();
        if (_currentData != null) _currentData.TrackStop();

        gameObject.SetActive(false);
        _onReturnToPool?.Invoke(this);
    }
}