using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    public static ArmyManager Instance { get; private set; }

    public int MaxCapacity { get; private set; }
    private int _currentUsedCapacity = 0;
    public int CurrentUsedCapacity => _currentUsedCapacity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanPlaceUnit(UnitData data)
    {
        if (data == null) return false;
        return (_currentUsedCapacity + data.capacityCost) <= MaxCapacity;
    }

    public void ConsumeCapacity(UnitData data)
    {
        _currentUsedCapacity += data.capacityCost;
        GlobalEventBus.OnPlacementCapacityChanged?.Invoke(_currentUsedCapacity, MaxCapacity);
    }

    public void SetupCapacity(int newMaxCapacity)
    {
        MaxCapacity = newMaxCapacity;
        _currentUsedCapacity = 0;
        GlobalEventBus.OnPlacementCapacityChanged?.Invoke(_currentUsedCapacity, MaxCapacity);
    }
}