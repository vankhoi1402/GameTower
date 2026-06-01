using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private readonly List<BaseUnit> _playerUnits = new List<BaseUnit>();
    private readonly List<BaseUnit> _enemyUnits = new List<BaseUnit>();

    public IReadOnlyList<BaseUnit> PlayerUnits => _playerUnits;
    public IReadOnlyList<BaseUnit> EnemyUnits => _enemyUnits;

    private bool _isCombatActive = false;
    private bool _isMatchOver = false;
    public MatchResult LastMatchResult { get; private set; } // THÊM DÒNG NÀY

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GlobalEventBus.OnUnitSpawned += HandleUnitSpawned;
        GlobalEventBus.OnUnitDied += HandleUnitDied;
    }

    private void OnDisable()
    {
        GlobalEventBus.OnUnitSpawned -= HandleUnitSpawned;
        GlobalEventBus.OnUnitDied -= HandleUnitDied;
    }

    public void StartCombatPhase()
    {
        _isCombatActive = true;
        _isMatchOver = false;
    }

    private void HandleUnitSpawned(BaseUnit unit)
    {
        if (unit.Team == TeamType.Player) _playerUnits.Add(unit);
        else _enemyUnits.Add(unit);

        GlobalEventBus.OnLiveArmyCountChanged?.Invoke(_playerUnits.Count, _enemyUnits.Count);
    }

    private void HandleUnitDied(BaseUnit unit)
    {
        if (unit.Team == TeamType.Player) _playerUnits.Remove(unit);
        else _enemyUnits.Remove(unit);

        GlobalEventBus.OnLiveArmyCountChanged?.Invoke(_playerUnits.Count, _enemyUnits.Count);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (!_isCombatActive || _isMatchOver) return;

        if (_playerUnits.Count == 0 && _enemyUnits.Count > 0) EndMatch(MatchResult.Defeat);
        else if (_enemyUnits.Count == 0 && _playerUnits.Count > 0) EndMatch(MatchResult.Victory);
        else if (_playerUnits.Count == 0 && _enemyUnits.Count == 0) EndMatch(MatchResult.Defeat);
    }

    private void EndMatch(MatchResult result)
    {
        _isMatchOver = true;
        _isCombatActive = false;
        LastMatchResult = result;
        GlobalEventBus.OnMatchEnded?.Invoke(result);
    }

    public void ResetBattleData()
    {
        _playerUnits.Clear();
        _enemyUnits.Clear();
        _isCombatActive = false;
        _isMatchOver = false;
        LastMatchResult = MatchResult.Defeat;
    }
}