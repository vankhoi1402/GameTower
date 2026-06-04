using System.Collections.Generic;
using UnityEngine;

public class BattleSoundManager : MonoBehaviour
{
    [Header("Cấu hình Pool cho Lính (Quy mô 30-40 lính)")]
    [SerializeField] private List<SoundDataSO> battleSoundRegistry;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int battlePoolSize = 20; // 20 là dư dả cho lính chém nhau trùng lặp

    private Dictionary<SoundType, SoundDataSO> _battleSoundDictionary;
    private Queue<PooledAudioSource> _battlePoolQueue;

    private void Awake()
    {
        InitializeRegistry();
        InitializePool();
    }

    private void OnEnable() => BattleEvents.OnPlayBattleSound3D += PlayBattleSoundHandler;
    private void OnDisable() => BattleEvents.OnPlayBattleSound3D -= PlayBattleSoundHandler;

    private void InitializeRegistry()
    {
        _battleSoundDictionary = new Dictionary<SoundType, SoundDataSO>();
        foreach (var data in battleSoundRegistry)
        {
            if (data != null) { data.ResetRuntimeData(); _battleSoundDictionary[data.soundType] = data; }
        }
    }

    private void InitializePool()
    {
        _battlePoolQueue = new Queue<PooledAudioSource>();
        for (int i = 0; i < battlePoolSize; i++)
        {
            CreateNewPoolInstance();
        }
    }

    private PooledAudioSource CreateNewPoolInstance()
    {
        GameObject obj = Instantiate(audioSourcePrefab, transform);
        PooledAudioSource source = obj.GetComponent<PooledAudioSource>() ?? obj.AddComponent<PooledAudioSource>();
        obj.SetActive(false);
        _battlePoolQueue.Enqueue(source);
        return source;
    }

    private void PlayBattleSoundHandler(SoundType type, Vector3 position)
    {
        if (!_battleSoundDictionary.TryGetValue(type, out var data) || !data.CanPlay()) return;

        PooledAudioSource source = _battlePoolQueue.Count == 0 ? CreateNewPoolInstance() : _battlePoolQueue.Dequeue();
        source.Play3D(data, position, ReturnToPool);
    }

    private void ReturnToPool(PooledAudioSource source) => _battlePoolQueue.Enqueue(source);
}