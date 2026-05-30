using System;

public static class GameEvents
{
    // =========================================================
    // GAME STATE
    // =========================================================

    // Yêu cầu đổi state
    public static event Action<GameState> RequestStateChange;

    // Đã đổi state thành công
    public static event Action<GameState> OnStateChanged;

    public static void CallStateChange(GameState newState)
    {
        RequestStateChange?.Invoke(newState);
    }

    public static void TriggerStateChanged(GameState newState)
    {
        OnStateChanged?.Invoke(newState);
    }


    // =========================================================
    // BATTLE RESULT
    // =========================================================

    public static event Action<BattleResult> OnBattleEnded;

    public static void TriggerBattleEnded(BattleResult result)
    {
        OnBattleEnded?.Invoke(result);
    }
}


// =========================================================
// GAME STATE
// =========================================================

public enum GameState
{
    Prepare,
    Playing,
    Paused
}


// =========================================================
// BATTLE RESULT
// =========================================================

public enum BattleResult
{
    Win,
    Lose,
    Draw
}