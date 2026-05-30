using UnityEngine;

public class PlayingState : GameBaseState
{
    public override void EnterState(GameManager game)
    {
        Time.timeScale = 1f;
        GameEvents.TriggerStateChanged(GameState.Playing); // Phát loa cho UI mở HUD, cho phép lính di chuyển
    }
    public override void UpdateState(GameManager game) { }
    public override void ExitState(GameManager game) { }
}