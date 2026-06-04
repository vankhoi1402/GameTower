using System;

public static class GlobalEventBus
{
    // Quản lý vòng đời thực thể
    public static Action<BaseUnit> OnUnitSpawned;
    public static Action<BaseUnit> OnUnitDied;

    // Cập nhật giao diện UI
    public static Action<int, int> OnPlacementCapacityChanged;
    public static Action<int, int> OnLiveArmyCountChanged;

    // Trạng thái trận đấu
    public static Action<MatchResult> OnMatchEnded;
    // true = có quân (mở nút), false = hết quân (khóa nút)
    public static Action<bool> OnPlayerUnitsAvailabilityChanged;
}